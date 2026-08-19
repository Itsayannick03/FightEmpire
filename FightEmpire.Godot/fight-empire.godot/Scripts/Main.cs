using Godot;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

using FightEmpire;
using FightEmpire.Core.Generation;
using FightEmpire.Core.Models;
using FightEmpire.Core.Persistence;


public partial class Main : Node2D
{
	private int _fighterPopulation = 10;
	private int _numberOfRounds = 3;
	private double _waitTime = 0.05;

	private List<Fighter> _rankedFighters = new();
	private List<Fighter> _fighters = new();

	private Fighter? _fighter1;
	private Fighter? _fighter2;


	// ========================================================
	// Main UI
	// ========================================================

	private VBoxContainer _mainLayout = null!;


	// ========================================================
	// Popup UI
	// ========================================================

	private ConfirmationDialog _popupWindow = null!;


	// ========================================================
	// Ranking UI
	// ========================================================

	private HBoxContainer _rankingPanel = null!;
	private ItemList _rankingList = null!;
	private Button _rankingsButton = null!;
	private Button _backButton = null!;


	// ========================================================
	// Fight UI
	// ========================================================

	private Button _simulateButton = null!;
	private Button _saveButton = null!;
	private Button _loadButton = null!;

	private RichTextLabel _resultLabel = null!;

	private Button _nextButton = null!;
	private Button _previousButton = null!;


	// ========================================================
	// Fighter 1 UI
	// ========================================================

	private ItemList _fighter1List = null!;
	private VBoxContainer _fighter1Details = null!;
	private RichTextLabel _fighter1Stats = null!;
	private Button _fighter1BackButton = null!;


	// ========================================================
	// Fighter 2 UI
	// ========================================================

	private ItemList _fighter2List = null!;
	private VBoxContainer _fighter2Details = null!;
	private RichTextLabel _fighter2Stats = null!;
	private Button _fighter2BackButton = null!;



	// ========================================================
	// READY
	// ========================================================

	public override void _Ready()
	{
		GetNodes();
		SetupUI();
		GenerateFighters();
		ConnectSignals();
	}



	// ========================================================
	// SETUP
	// ========================================================

	private void GetNodes()
	{
		// Fighter 1

		_fighter1List = GetNode<ItemList>(
            "UI/Screen/Margin/MainLayout/FighterColumns/Fighter1Column/Control/Fighter1List"
		);

		_fighter1Details = GetNode<VBoxContainer>(
            "UI/Screen/Margin/MainLayout/FighterColumns/Fighter1Column/Control/Fighter1Details"
		);

		_fighter1Stats = GetNode<RichTextLabel>(
            "UI/Screen/Margin/MainLayout/FighterColumns/Fighter1Column/Control/Fighter1Details/Panel/Fighter1Stats"
		);

		_fighter1BackButton = GetNode<Button>(
            "UI/Screen/Margin/MainLayout/FighterColumns/Fighter1Column/Control/Fighter1Details/Button"
		);


		// Fighter 2

		_fighter2List = GetNode<ItemList>(
            "UI/Screen/Margin/MainLayout/FighterColumns/Fighter2Column/Control/Fighter2List"
		);

		_fighter2Details = GetNode<VBoxContainer>(
            "UI/Screen/Margin/MainLayout/FighterColumns/Fighter2Column/Control/Fighter2Details"
		);

		_fighter2Stats = GetNode<RichTextLabel>(
            "UI/Screen/Margin/MainLayout/FighterColumns/Fighter2Column/Control/Fighter2Details/Panel/Fighter2Stats"
		);

		_fighter2BackButton = GetNode<Button>(
            "UI/Screen/Margin/MainLayout/FighterColumns/Fighter2Column/Control/Fighter2Details/Button"
		);


		// Fight

		_simulateButton = GetNode<Button>(
            "UI/Screen/Margin/MainLayout/SimulateButton"
		);

		_saveButton = GetNode<Button>(
            "UI/Screen/Margin/MainLayout/SaveButton"
		);

		_loadButton = GetNode<Button>(
            "UI/Screen/Margin/MainLayout/LoadButton"
		);

		_resultLabel = GetNode<RichTextLabel>(
            "UI/Screen/Margin/MainLayout/FighterColumns/ResultColumn/ResultContent/Panel/ResultLabel"
		);

		_nextButton = GetNode<Button>(
			"UI/Screen/Margin/MainLayout/FighterColumns/ResultColumn/ResultContent/HBoxContainer/NextButton"
		);
		_previousButton = GetNode<Button>(
			"UI/Screen/Margin/MainLayout/FighterColumns/ResultColumn/ResultContent/HBoxContainer/PreviousButton"
		);


		// Main

		_mainLayout = GetNode<VBoxContainer>(
            "UI/Screen/Margin/MainLayout"
		);


		// Popup

		_popupWindow = GetNode<ConfirmationDialog>(
            "UI/Screen/Margin/Popup"
		);


		// Rankings

		_rankingPanel = GetNode<HBoxContainer>(
            "UI/Screen/Margin/RankingPanel"
		);

		_rankingList = GetNode<ItemList>(
            "UI/Screen/Margin/RankingPanel/RankingContainer/RankingList"
		);

		_backButton = GetNode<Button>(
            "UI/Screen/Margin/RankingPanel/RankingContainer/ButtonContainer/BackButton"
		);

		_rankingsButton = GetNode<Button>(
            "UI/Screen/Margin/MainLayout/RankingsButton"
		);
	}


	private void SetupUI()
	{
		_fighter1Details.Visible = false;
		_fighter1List.Visible = true;

		_fighter2Details.Visible = false;
		_fighter2List.Visible = true;

		_mainLayout.Visible = true;

		_popupWindow.Visible = false;

		_rankingPanel.Visible = false;
	}


	private void ConnectSignals()
	{
		_fighter1List.ItemSelected += OnFighter1Selected;
		_fighter1BackButton.Pressed += OnFighter1Back;

		_fighter2List.ItemSelected += OnFighter2Selected;
		_fighter2BackButton.Pressed += OnFighter2Back;

		_simulateButton.Pressed += RunFight;

		_saveButton.Pressed += Save;
		_loadButton.Pressed += Load;

		_rankingsButton.Pressed += Ranking;
		_backButton.Pressed += Back;
	}



	// ========================================================
	// FIGHTER POPULATION
	// ========================================================

	private void GenerateFighters()
	{
		FighterGenerator generator = new();

		_fighters = generator.generateFighters(
			_fighterPopulation
		);

		PopulateFighterLists();
	}


	private void PopulateFighterLists()
	{
		_rankedFighters = _fighters
			.OrderByDescending(
				fighter => fighter.RankingPoints
			)
			.ToList();

		for (int i = 0; i < _rankedFighters.Count; i++)
		{
			Fighter fighter = _rankedFighters[i];

			string name =
				$"#{i + 1} " +
				$"{fighter.FirstName} " +
				$"\"{fighter.Nickname}\" " +
				$"{fighter.LastName}";

			_fighter1List.AddItem(name);
			_fighter2List.AddItem(name);
		}
	}


	private void PopulateFighterRankings()
	{
		_rankingList.Clear();

		List<Fighter> sortedFighterList =
			_fighters
				.OrderByDescending(
					fighter => fighter.RankingPoints
				)
				.ToList();

		int ranking = 1;

		foreach (Fighter fighter in sortedFighterList)
		{
			string rankingEntry =
				$"{ranking}. " +
				$"{fighter.FirstName} " +
				$"\"{fighter.Nickname}\" " +
				$"{fighter.LastName} " +
				$"({fighter.RankingPoints})";

			_rankingList.AddItem(rankingEntry);

			ranking++;
		}
	}


	private void ClearFighterLists()
	{
		_fighter1List.Clear();
		_fighter2List.Clear();
	}



	// ========================================================
	// FIGHTER 1 SELECTION
	// ========================================================

	private void OnFighter1Selected(long index)
	{
		Fighter fighter =
			_rankedFighters[(int)index];

		if (fighter == _fighter2)
		{
			_fighter1List.Deselect((int)index);
			return;
		}

		_fighter1 = fighter;

		_fighter1Stats.Text =
			fighter.ToString();

		_fighter1List.Visible = false;
		_fighter1Details.Visible = true;
	}


	private void OnFighter1Back()
	{
		_fighter1 = null;

		_fighter1Details.Visible = false;
		_fighter1List.Visible = true;

		_fighter1List.DeselectAll();
	}



	// ========================================================
	// FIGHTER 2 SELECTION
	// ========================================================

	private void OnFighter2Selected(long index)
	{
		Fighter fighter =
			_rankedFighters[(int)index];

		if (fighter == _fighter1)
		{
			_fighter2List.Deselect((int)index);
			return;
		}

		_fighter2 = fighter;

		_fighter2Stats.Text =
			fighter.ToString();

		_fighter2List.Visible = false;
		_fighter2Details.Visible = true;
	}


	private void OnFighter2Back()
	{
		_fighter2 = null;

		_fighter2Details.Visible = false;
		_fighter2List.Visible = true;

		_fighter2List.DeselectAll();
	}



	// ========================================================
	// FIGHT
	// ========================================================

	private async void RunFight()
	{
		if (_fighter1 == null || _fighter2 == null)
			return;

		Fighter fighter1 = _fighter1;
		Fighter fighter2 = _fighter2;

		Fight fight = new(
			fighter1,
			fighter2,
			_numberOfRounds
		);

		_simulateButton.Disabled = true;


		// --------------------
		// Introduction
		// --------------------

		_resultLabel.Text =
			$"{GetFighterName(fighter1)}\n\n" +
			"VS\n\n" +
			$"{GetFighterName(fighter2)}";

		await Wait(_waitTime * 2);


		// --------------------
		// Run simulation
		// --------------------

		fight.Run();

		FightSummary fightSummary =
			fight.Summary;


		// --------------------
		// Replay fight
		// --------------------

		foreach (RoundSummary round in fightSummary.Rounds)
		{
			await ShowRound(round);

			if (round.IsFinish)
				break;
		}


		// --------------------
		// Final result
		// --------------------

		await ShowFightResult(fightSummary);


		// --------------------
		// Reset UI
		// --------------------

		ResetFighterSelections();
		UpdateFighterLists();

		_simulateButton.Disabled = false;
	}



	// ========================================================
	// ROUND
	// ========================================================

	private async Task ShowRound(
		RoundSummary round)
	{
		_resultLabel.Text =
			$"ROUND {round.RoundNumber}";

		await Wait(_waitTime);


		int secondsElapsed = 300;


		foreach (ExchangeSumary exchange in round.Exchanges)
		{
			bool clickThrough = false;

			if (clickThrough)
			{
				int i = 0;

				while (i >= 0 && i < exchange.Actions.Count)
				{
					ActionSumary action = exchange.Actions[i];

					await ShowAction(
						action,
						round.RoundNumber,
						secondsElapsed
					);

					if (action.IsFinish)
						return;

					NavigationAction navigation =
						await WaitForNavigation();

					if (navigation == NavigationAction.Next)
					{
						secondsElapsed -= action.TimeTaken;

						i++;
					}
					else if (navigation == NavigationAction.Previous)
					{
						if (i > 0)
						{
							i--;

							secondsElapsed += exchange.Actions[i].TimeTaken;
						}
					}
				}
			}
			else
			{
				foreach (ActionSumary action in exchange.Actions)
				{
					secondsElapsed -= action.TimeTaken;

					await ShowAction(
						action,
						round.RoundNumber,
						secondsElapsed
					);

					if (action.IsFinish)
						return;
				}
			}
		}


		// --------------------
		// Normal round ending
		// --------------------

		if (!round.IsFinish)
		{
			string score =
				round.Result switch
				{
					RoundOutcome.TenNine =>
						"10-9",

					RoundOutcome.TenEight =>
						"10-8",

					_ =>
                        ""
				};

			_resultLabel.Text =
				$"END OF ROUND {round.RoundNumber}\n\n" +
				$"{GetFighterName(round.Winner)}\n\n" +
				$"wins the round {score}";

			await Wait(_waitTime * 2);
		}


		_resultLabel.Clear();
	}



	// ========================================================
	// ACTION COMMENTARY
	// ========================================================

	private async Task ShowAction(
		ActionSumary action,
		int roundNumber,
		int secondsElapsed)
	{
		int minutes =
			secondsElapsed / 60;

		int seconds =
			secondsElapsed % 60;


		string actor =
			GetFighterName(action.Actor);

		string defender =
			GetFighterName(action.Defender);


		string commentary =
			action.Outcome switch
			{
				ActionOutcome.StrikeLanded =>
					$"{actor} lands a clean strike on {defender}!",


				ActionOutcome.StrikeBlocked =>
					$"{defender} blocks {actor}'s strike.",


				ActionOutcome.KickLanded =>
					$"{actor} lands a kick on {defender}!",


				ActionOutcome.KickBlocked =>
					$"{defender} blocks the kick!",


				ActionOutcome.Takedown =>
					$"{actor} shoots in and gets the takedown!",


				ActionOutcome.TakedownBlocked =>
					$"{defender} stuffs the takedown attempt!",


				ActionOutcome.GetUp =>
					$"{actor} gets back to the feet!",


				ActionOutcome.GetUpDenied =>
					$"{defender} keeps {actor} on the ground!",


				ActionOutcome.GroundStrikeLanded =>
					$"{actor} lands a ground strike!",


				ActionOutcome.GroundStrikeBlocked =>
					$"{defender} blocks the ground strike!",


				ActionOutcome.SubmissionProgress =>
					$"{actor} attacks a submission!\n\n" +
					$"{defender} is in danger!",


				ActionOutcome.SubmissionDefense =>
					$"{defender} successfully defends the submission!",


				ActionOutcome.Knockout =>
					$"{actor} CONNECTS!\n\n" +
					$"{defender} IS OUT!",


				ActionOutcome.TKO =>
					$"{actor} is pouring on the damage!\n\n" +
					$"THE REFEREE HAS SEEN ENOUGH!",


				ActionOutcome.Submission =>
					$"{actor} locks in the submission!\n\n" +
					$"{defender} TAPS!",


				_ =>
					$"{actor} attacks {defender}."
			};


		_resultLabel.Text =
			$"ROUND {roundNumber}\n" +
			$"{minutes}:{seconds:00}\n\n" +
			commentary;


		await Wait(_waitTime);


		// --------------------
		// Finish
		// --------------------

		if (action.IsFinish)
		{
			await ShowFinish(
				action,
				roundNumber,
				secondsElapsed
			);
		}
	}



	// ========================================================
	// FINISH
	// ========================================================

	private async Task ShowFinish(
		ActionSumary action,
		int roundNumber,
		int secondsElapsed)
	{
		int minutes =
			secondsElapsed / 60;

		int seconds =
			secondsElapsed % 60;


		string finishType =
			action.Outcome switch
			{
				ActionOutcome.Knockout =>
					"KNOCKOUT",

				ActionOutcome.TKO =>
					"TECHNICAL KNOCKOUT",

				ActionOutcome.Submission =>
					"SUBMISSION",

				_ =>
                    "FINISH"
			};


		_resultLabel.Text =
			$"{finishType}!\n\n" +
			$"{GetFighterName(action.Winner!)}\n\n" +
			$"WINS!\n\n" +
			$"Round {roundNumber}\n" +
			$"{minutes}:{seconds:00}";


		await Wait(_waitTime * 3);
	}



	// ========================================================
	// FIGHT RESULT
	// ========================================================

	private async Task ShowFightResult(
		FightSummary fight)
	{
		// --------------------
		// Draw
		// --------------------

		if (fight.Result == FightResult.Draw)
		{
			_resultLabel.Text =
				$"FIGHT RESULT\n\n" +
				$"{GetFighterName(fight.Fighter1)}\n" +
				$"VS\n" +
				$"{GetFighterName(fight.Fighter2)}\n\n" +
				$"DRAW";

			await Wait(_waitTime * 3);

			return;
		}


		// --------------------
		// Decision
		// --------------------

		if (fight.Result == FightResult.Decision)
		{
			_resultLabel.Text =
				$"FIGHT RESULT\n\n" +
				$"{GetFighterName(fight.Winner!)}\n\n" +
				$"WINS BY DECISION";

			await Wait(_waitTime * 3);

			return;
		}


		// --------------------
		// Finish
		// --------------------

		string finishType =
			fight.FinishOutcome switch
			{
				ActionOutcome.Knockout =>
					"KO",

				ActionOutcome.TKO =>
					"TKO",

				ActionOutcome.Submission =>
					"SUBMISSION",

				_ =>
                    "FINISH"
			};


		_resultLabel.Text =
			$"FIGHT RESULT\n\n" +
			$"{GetFighterName(fight.Winner!)}\n\n" +
			$"defeats\n\n" +
			$"{GetFighterName(fight.Loser!)}\n\n" +
			$"{finishType}\n\n" +
			$"Round {fight.FinishRound}\n" +
			$"{fight.FinishMinute}:{fight.FinishSecond:00}";


		await Wait(_waitTime * 3);
	}



	// ========================================================
	// UPDATE FIGHTER LISTS
	// ========================================================

	private void UpdateFighterLists()
	{
		_fighter1List.Clear();
		_fighter2List.Clear();

		PopulateFighterLists();
	}


	private void ResetFighterSelections()
	{
		OnFighter1Back();
		OnFighter2Back();
	}



	// ========================================================
	// PERSISTENCE
	// ========================================================

	private async void Save()
	{
		bool confirmation =
			await Popup(
				"Are you sure you want to save?",
				"Cancel",
                "Confirm"
			);

		if (!confirmation)
			return;


		string path =
			ProjectSettings.GlobalizePath(
                "user://fighters.json"
			);


		SaveManager.SaveFighters(
			_fighters,
			path
		);


		GD.Print(
			$"Saved fighters to: {path}"
		);
	}


	private async void Load()
	{
		bool confirmation =
			await Popup(
				"Are you sure you want to load?",
				"Cancel",
                "Confirm"
			);

		if (!confirmation)
			return;


		string path =
			ProjectSettings.GlobalizePath(
                "user://fighters.json"
			);


		List<Fighter> savedFighters =
			SaveManager.LoadFighters(path);


		if (savedFighters == null)
		{
			GD.Print(
                "No save file found"
			);

			return;
		}


		ClearFighterLists();

		_fighters =
			savedFighters;

		PopulateFighterLists();

		ResetFighterSelections();


		GD.Print(
			$"Loaded fighters from {path}"
		);
	}



	// ========================================================
	// POPUP
	// ========================================================

	private async Task<bool> Popup(
		string mainText,
		string cancelText,
		string okText)
	{
		bool? result = null;


		void OnConfirmed()
		{
			result = true;
		}


		void OnCanceled()
		{
			result = false;
		}


		_popupWindow.DialogText =
			mainText;

		_popupWindow
			.GetOkButton()
			.Text = okText;

		_popupWindow
			.GetCancelButton()
			.Text = cancelText;


		_popupWindow.Confirmed +=
			OnConfirmed;

		_popupWindow.Canceled +=
			OnCanceled;


		_popupWindow.PopupCentered();


		while (result == null)
		{
			await ToSignal(
				GetTree(),
				SceneTree.SignalName.ProcessFrame
			);
		}


		_popupWindow.Confirmed -=
			OnConfirmed;

		_popupWindow.Canceled -=
			OnCanceled;


		_popupWindow.Hide();


		return result.Value;
	}



	// ========================================================
	// RANKINGS
	// ========================================================

	private void Ranking()
	{
		_mainLayout.Visible =
			false;

		_rankingPanel.Visible =
			true;

		PopulateFighterRankings();
	}


	private void Back()
	{
		_mainLayout.Visible =
			true;

		_rankingPanel.Visible =
			false;

		UpdateFighterLists();
	}



	// ========================================================
	// HELPERS
	// ========================================================

	private string GetFighterName(
		Fighter fighter)
	{
		if (
			string.IsNullOrWhiteSpace(
				fighter.Nickname
			)
		)
		{
			return
				$"{fighter.FirstName} " +
				$"{fighter.LastName}";
		}


		return
			$"{fighter.FirstName} " +
			$"\"{fighter.Nickname}\" " +
			$"{fighter.LastName}";
	}


	private async Task Wait(
		double seconds)
	{
		await ToSignal(
			GetTree().CreateTimer(seconds),
			SceneTreeTimer.SignalName.Timeout
		);
	}

	private enum NavigationAction
	{
		Next,
		Previous
	}

	private async Task<NavigationAction> WaitForNavigation()
	{
		NavigationAction? result = null;

		void OnNext()
		{
			result = NavigationAction.Next;
		}

		void OnPrevious()
		{
			result = NavigationAction.Previous;
		}

		_nextButton.Pressed += OnNext;
		_previousButton.Pressed += OnPrevious;

		while (result == null)
		{
			await ToSignal(
				GetTree(),
				SceneTree.SignalName.ProcessFrame
			);
		}

		_nextButton.Pressed -= OnNext;
		_previousButton.Pressed -= OnPrevious;

		return result.Value;
	}
}
