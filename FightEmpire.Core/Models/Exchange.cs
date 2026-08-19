using FightEmpire.Core.Models;

namespace FightEmpire;

public class Exchange
{
    private readonly Fighter _fighter1;
    private readonly Fighter _fighter2;

    private readonly int _timeAvailable;

    public FightPosition CurrentPosition;

    public List<ActionSumary> Actions = new();

    public Fighter? Winner;
    public Fighter? Looser;

    public bool IsFinish;

    public ActionOutcome? FinishOutcome;

    private int _minActions = 3;
    private int _maxActions = 10;

    public int Fighter1Score = 0;
    public int Fighter2Score = 0;

    public int TimeTaken = 0;

    public ExchangeSumary Sumary = null!;


    public Exchange(
        Fighter fighter1,
        Fighter fighter2,
        FightPosition startingPosition,
        int timeAvailable)
    {
        _fighter1 = fighter1;
        _fighter2 = fighter2;

        CurrentPosition = startingPosition;

        _timeAvailable = timeAvailable;
    }


    public void Run()
    {
        int numberOfActions =
            Random.Shared.Next(
                _minActions,
                _maxActions + 1
            );

        for (int i = 0; i < numberOfActions; i++)
        {
            CombatAction action = new(
                _fighter1,
                _fighter2,
                CurrentPosition
            );

            action.Run();


            // Would this action go beyond the end of the round?
            if (TimeTaken + action.TimeTaken > _timeAvailable)
            {
                TimeTaken = _timeAvailable;
                break;
            }


            Actions.Add(action.Sumary);

            TimeTaken += action.TimeTaken;

            CurrentPosition = action.CurentPosition;


            // --------------------
            // Finish
            // --------------------

            if (action.IsFinish)
            {
                IsFinish = true;

                Winner = action.Winner;
                Looser = action.Looser;

                FinishOutcome = action.Outcome;

                break;
            }


            // --------------------
            // Score action
            // --------------------

            if (action.Winner == _fighter1)
            {
                Fighter1Score++;
            }
            else if (action.Winner == _fighter2)
            {
                Fighter2Score++;
            }
        }


        // --------------------
        // Exchange winner
        // --------------------

        if (!IsFinish)
        {
            DetermineWinner();
        }


        // --------------------
        // Summary
        // --------------------

        Sumary = new()
        {
            CurrentPosition = CurrentPosition,
            Actions = Actions,

            Winner = Winner,
            Looser = Looser,

            IsFinish = IsFinish,
            FinishOutcome = FinishOutcome,

            TimeTaken = TimeTaken
        };
    }


    private void DetermineWinner()
    {
        if (Fighter1Score > Fighter2Score)
        {
            Winner = _fighter1;
            Looser = _fighter2;

            _fighter1.RoundScore++;

            return;
        }

        if (Fighter2Score > Fighter1Score)
        {
            Winner = _fighter2;
            Looser = _fighter1;

            _fighter2.RoundScore++;

            return;
        }


        // Drawn exchange — random winner
        if (Random.Shared.Next(2) == 0)
        {
            Winner = _fighter1;
            Looser = _fighter2;

            _fighter1.RoundScore++;
        }
        else
        {
            Winner = _fighter2;
            Looser = _fighter1;

            _fighter2.RoundScore++;
        }
    }
}