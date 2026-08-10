using Godot;
using System.Collections.Generic;
using System.Threading.Tasks;
using FightEmpire.Core.Generation;
using FightEmpire.Core.Models;
using FightEmpire.Core.Persistence;
using FightEmpire;
using System.Linq;

public partial class Main : Node2D
{
	private int _fighterPopulation = 10;
	private int _numberOfRounds = 3;
	private double _waitTime = 0.3;

	private List<Fighter> _rankedFighters = new();

	private List<Fighter> _fighters = new();

	private Fighter _fighter1 = null!;
	private Fighter _fighter2 = null!;

	// Main UI
	private VBoxContainer _mainLayout = null!;

	// Popup UI
	private ConfirmationDialog _popupWindow = null!;

	// Ranking UI
	private HBoxContainer _rankingPanel = null!;
	private ItemList _rankingList = null!;
	private Button _rankingsButton = null!;
	private Button _backButton = null!;

	// Fight UI
	private Button _simulateButton = null!;
	private Button _saveButton = null!;
	private Button _loadButton = null!;

	private RichTextLabel _resultLabel = null!;

	// Fighter 1 UI
	private ItemList _fighter1List = null!;
	private VBoxContainer _fighter1Details = null!;
	private RichTextLabel _fighter1Stats = null!;
	private Button _fighter1BackButton = null!;

	// Fighter 2 UI
	private ItemList _fighter2List = null!;
	private VBoxContainer _fighter2Details = null!;
	private RichTextLabel _fighter2Stats = null!;
	private Button _fighter2BackButton = null!;


	public override void _Ready()
	{
		GetNodes();
		SetupUI();
		GenerateFighters();
		ConnectSignals();
	}


	// --------------------
	// Setup
	// --------------------

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

		// Popup
		_popupWindow.Visible = false;

		// Rankings
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


	// --------------------
	// Fighter population
	// --------------------

	private void GenerateFighters()
	{
		FighterGenerator generator = new();

		_fighters = generator.generateFighters(_fighterPopulation);

		PopulateFighterLists();
	}

	private void PopulateFighterLists()
	{
		_rankedFighters = _fighters
			.OrderByDescending(f => f.RankingPoints)
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

		List<Fighter> sortedFighterList = _fighters.OrderByDescending(fighter => fighter.RankingPoints).ToList();

		int ranking = 1;
		foreach(Fighter fighter in sortedFighterList)
		{
			string rankingEntry = $"""{ranking}. {fighter.FirstName} "{fighter.Nickname}" {fighter.LastName} """;

			_rankingList.AddItem(rankingEntry);
			ranking++;
		}
	}

	private void ClearFighterLists()
	{
		_fighter1List.Clear();
		_fighter2List.Clear();
	}


	// --------------------
	// Fighter 1 selection
	// --------------------

	private void OnFighter1Selected(long index)
	{
		Fighter fighter = _rankedFighters[(int)index];

		if (fighter == _fighter2)
		{
			_fighter1List.Deselect((int)index);
			return;
		}

		_fighter1 = fighter;

		_fighter1Stats.Text = fighter.ToString();

		_fighter1List.Visible = false;
		_fighter1Details.Visible = true;
	}

	private void OnFighter1Back()
	{
		_fighter1 = null!;

		_fighter1Details.Visible = false;
		_fighter1List.Visible = true;

		_fighter1List.DeselectAll();
	}


	// --------------------
	// Fighter 2 selection
	// --------------------

	private void OnFighter2Selected(long index)
	{
		Fighter fighter = _rankedFighters[(int)index];

		if (fighter == _fighter1)
		{
			_fighter2List.Deselect((int)index);
			return;
		}

		_fighter2 = fighter;

		_fighter2Stats.Text = fighter.ToString();

		_fighter2List.Visible = false;
		_fighter2Details.Visible = true;
	}

	private void OnFighter2Back()
	{
		_fighter2 = null!;

		_fighter2Details.Visible = false;
		_fighter2List.Visible = true;

		_fighter2List.DeselectAll();
	}


	// --------------------
	// Fight
	// --------------------

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

		_resultLabel.Text =
			$"{fighter1.FirstName} {fighter1.LastName}\n" +
			"VS\n" +
			$"{fighter2.FirstName} {fighter2.LastName}";

		await Wait(_waitTime);

		fight.Run();

		foreach (RoundSummary round in fight.RoundSummaries)
		{
			await ShowRound(round);
		}

		ShowFightResult(fight);

		ResetFighterSelections();
		UpdateFighterLists();

		_simulateButton.Disabled = false;
	}

	private void UpdateFighterLists()
	{
		_fighter1List.Clear();
		_fighter2List.Clear();

		PopulateFighterLists();
	}

	private async Task ShowRound(RoundSummary round)
	{
		_resultLabel.Text =
			$"ROUND {round.RoundNumber}";

		await Wait(_waitTime);

		foreach (Exchange exchange in round.Exchanges)
		{
			await ShowExchange(exchange);
		}

		if (!round.isFinish)
		{
			_resultLabel.Text =
				$"{round.Winner.FirstName} " +
				$"{round.Winner.LastName}\n\n" +
				$"wins Round {round.RoundNumber}.";

			await Wait(_waitTime);
		}

		_resultLabel.Clear();
	}

	private async Task ShowExchange(Exchange exchange)
	{
		string text =
			$"ROUND: {exchange.RoundNumber}\n" +
			$" ({exchange.Minute}:{exchange.Second:00})\n\n" +

			$"{exchange.StyleWinner!.FirstName} " +
			$"{exchange.StyleWinner.LastName} wins the style battle.\n\n" +

			$"The exchange becomes {exchange.Style}.\n\n";

		if (exchange.IsFinish)
		{
			text +=
				$"{exchange.Winner!.FirstName} " +
				$"{exchange.Winner.LastName} finishes " +
				$"{exchange.Looser!.FirstName} " +
				$"{exchange.Looser.LastName}\n\n" +
				$"via {exchange.Outcome}!";
		}
		else
		{
			text +=
				$"{exchange.Winner!.FirstName} " +
				$"{exchange.Winner.LastName} wins the exchange.";
		}

		_resultLabel.Text = text;

		await Wait(_waitTime);

		_resultLabel.Clear();
	}

	private void ShowFightResult(Fight fight)
	{
		if (fight.Result == RoundOutcome.Decision)
		{
			_resultLabel.Text =
				$"FIGHT RESULT\n\n" +
				$"{fight.Winner!.FirstName} " +
				$"{fight.Winner.LastName} wins by Decision.\n\n" +
				$"Score: {fight.Winner.Points}-{fight.Looser!.Points}";

			return;
		}

		_resultLabel.Text =
			$"FIGHT RESULT\n\n" +
			$"{fight.Winner!.FirstName} " +
			$"{fight.Winner.LastName} defeats " +
			$"{fight.Looser!.FirstName} " +
			$"{fight.Looser.LastName}\n\n" +
			$"In {5 - fight.FinishMinute} Minutes and {60 - fight.FinishSecond} Seconds \n\n" +
			$"via {fight.Result}\n" +
			$"Round {fight.FinishRound}";
	}

	private void ResetFighterSelections()
	{
		OnFighter1Back();
		OnFighter2Back();
	}


	// --------------------
	// Persistence
	// --------------------

	private async void Save()
	{
		bool confirmation = await Popup(
			"Are you sure you want to save?",
			"Cancel",
            "Confirm"
		);

		if (!confirmation)
			return;

		string path =
			ProjectSettings.GlobalizePath("user://fighters.json");

		SaveManager.SaveFighters(_fighters, path);

		GD.Print($"Saved fighters to: {path}");
	}

	private async void Load()
	{
		bool confirmation = await Popup(
			"Are you sure you want to load?",
			"Cancel",
            "Confirm"
		);

		if (!confirmation)
			return;

		string path =
			ProjectSettings.GlobalizePath("user://fighters.json");

		List<Fighter> savedFighters =
			SaveManager.LoadFighters(path);

		if (savedFighters == null)
		{
			GD.Print("No save file found");
			return;
		}

		ClearFighterLists();

		_fighters = savedFighters;

		PopulateFighterLists();

		ResetFighterSelections();

		GD.Print($"Loaded fighters from {path}");
	}


	// --------------------
	// Popup
	// --------------------

	private async Task<bool> Popup(
		string mainText,
		string cancelText,
		string okText)
	{
		bool? result = null;

		void OnConfirmed() => result = true;
		void OnCanceled() => result = false;

		_popupWindow.DialogText = mainText;

		_popupWindow.GetOkButton().Text = okText;
		_popupWindow.GetCancelButton().Text = cancelText;

		_popupWindow.Confirmed += OnConfirmed;
		_popupWindow.Canceled += OnCanceled;

		_popupWindow.PopupCentered();

		while (result == null)
		{
			await ToSignal(
				GetTree(),
				SceneTree.SignalName.ProcessFrame
			);
		}

		_popupWindow.Confirmed -= OnConfirmed;
		_popupWindow.Canceled -= OnCanceled;

		_popupWindow.Hide();

		return result.Value;
	}

	// --------------------
	// Ranking
	// --------------------
	private void Ranking()
	{
		_mainLayout.Visible = false;
		_rankingPanel.Visible = true;

		PopulateFighterRankings();
	}

	private void Back()
	{
		_mainLayout.Visible = true;
		_rankingPanel.Visible = false;

		UpdateFighterLists();
	}

	// --------------------
	// Helpers
	// --------------------

	private async Task Wait(double seconds)
	{
		await ToSignal(
			GetTree().CreateTimer(seconds),
			SceneTreeTimer.SignalName.Timeout
		);
	}
}
