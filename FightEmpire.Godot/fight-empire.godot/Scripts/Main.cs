using Godot;
using System.Collections.Generic;
using FightEmpire.Core.Generation;
using FightEmpire.Core.Models;
using System.Threading.Tasks;
using FightEmpire.Core.Persistence;

public partial class Main : Node2D
{
	private int _fighterPopulation = 10;
	private int _numberOfRounds = 3;

	private List<Fighter> _fighters = new();

	private Fighter _fighter1 = null!;
	private Fighter _fighter2 = null!;

	// Main UI
	private VBoxContainer _mainLayout = null!;

	// Popup UI
	private ConfirmationDialog _popupWindow = null!;

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
			"UI/Screen/Margin/MainLayout");

		// Popup
		_popupWindow = GetNode<ConfirmationDialog>(
			"UI/Screen/Margin/Popup"
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
		foreach (Fighter fighter in _fighters)
		{
			string name =
				$"{fighter.FirstName} " +
				$"\"{fighter.Nickname}\" " +
				$"{fighter.LastName}";

			_fighter1List.AddItem(name);
			_fighter2List.AddItem(name);
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
		Fighter fighter = _fighters[(int)index];

		if (fighter == _fighter2)
			return;

		_fighter1 = fighter;

		_fighter1Stats.Text = fighter.ToString();

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


	// --------------------
	// Fighter 2 selection
	// --------------------

	private void OnFighter2Selected(long index)
	{
		Fighter fighter = _fighters[(int)index];

		if (fighter == _fighter1)
			return;

		_fighter2 = fighter;

		_fighter2Stats.Text = fighter.ToString();

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


	// --------------------
	// Fight
	// --------------------

	private async void RunFight()
	{
		if (_fighter1 == null || _fighter2 == null)
			return;

		Fight fight = new(_fighter1, _fighter2, _numberOfRounds);

		fight.Run();

		foreach (RoundSummary round in fight.RoundSummaries)
		{
			await ShowRoundSummary(round);
		}

		if(fight.Result == RoundOutcome.Decision)
		{
			_resultLabel.Text = $"Winner: {fight.Winner!.FirstName} {fight.Winner.LastName} by Dcision with a score of {fight.Winner.Points}-{fight.Looser!.Points}";
		}
		else
		{
			_resultLabel.Text = $"Winner: {fight.Winner!.FirstName} {fight.Winner.LastName} by {fight.Result}";
		}


		ResetFighterSelections();
	}

	private string GetRoundSummary(RoundSummary round)
	{
		return $"""
		Round {round.RoundNumber}

		{round.StyleWinner.FirstName} {round.StyleWinner.LastName}
		wins the style battle and makes it a {round.style} round.

		{round.Winner.FirstName} {round.Winner.LastName}
		wins the round.

		Outcome: {round.outcome}
		""";
	}

	private async Task ShowRoundSummary(RoundSummary round)
	{
		_resultLabel.Text = GetRoundSummary(round);
		await Wait(1);
		_resultLabel.Clear();
	}

	private void ResetFighterSelections()
	{
		OnFighter1Back();
		OnFighter2Back();
	}

	// --------------------
	// Persistance
	// --------------------

	private async void Save()
	{
		bool confirmation = await Popup("Are you sure you want to save?", "Cancel", "Confirm");

		if(!confirmation)
			return;

		string path = ProjectSettings.GlobalizePath("user://fighters.json");

		SaveManager.SaveFighters(_fighters, path);

		GD.Print($"Saved fighters to: {path}");
	}

	private async void Load()
	{
		bool confirmation = await Popup("Are you sure you want to load?", "Cancel", "Confirm");

		if(!confirmation)
			return;

		string path = ProjectSettings.GlobalizePath("user://fighters.json");
		List<Fighter> savedFighters = SaveManager.LoadFighters(path);
		if(savedFighters == null)
		{
			GD.Print($"No save file found");
		}
		else
		{
			ClearFighterLists();
			_fighters = savedFighters;
			PopulateFighterLists();
			GD.Print($"Loaded fighters from {path}");

		}
	}

	// --------------------
	// Popup
	// --------------------

	private async Task<bool> Popup(string mainText, string cancelText, string okText)
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
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		}

		_popupWindow.Confirmed -= OnConfirmed;
		_popupWindow.Canceled -= OnCanceled;

		_popupWindow.Hide();

		return result.Value;
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
