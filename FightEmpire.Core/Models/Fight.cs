using FightEmpire;
using FightEmpire.Core.Models;

public class Fight
{

    private readonly Fighter _fighter1;
    private readonly Fighter _fighter2;
    private readonly int _numberOfRounds;

    private Fighter? _winner;
    private Fighter? _loser;

    private bool _isFinish = false;

    private ActionOutcome? _finishOutcome;

    private int _finishRound;
    private int _finishMinute;
    private int _finishSecond;

    private int _rankingPointChange;

    public readonly List<RoundSummary> _rounds = new();

    public FightSummary Summary { get; private set; } = null!;

    private FightResult _result;


    public Fight(
        Fighter fighter1,
        Fighter fighter2,
        int numberOfRounds)
    {
        _fighter1 = fighter1;
        _fighter2 = fighter2;
        _numberOfRounds = numberOfRounds;
    }

    Random random = new Random();
    public void Run()
    {
        ResetFighters();
        RunRounds();
        DetermainWinner();
        UpdateStats();
        CreateSumary();
        UpdateFightHistory();

    }

    private void ResetFighters()
    {
        _fighter1.Reset();
        _fighter2.Reset();
    }

    private void RunRounds()
    {
        for(int roundNumber = 1; roundNumber <= _numberOfRounds; roundNumber++)
        {
            Round round = new(_fighter1, _fighter2, roundNumber);

            round.Run();

            _rounds.Add(round.Summary);

            if(round.Summary.IsFinish)
            {
                _winner = round.Summary.Winner;
                _loser = round.Summary.Looser;

                _finishRound = roundNumber;
                _finishMinute = round.Summary.SecondsUsed / 60;
                _finishSecond = round.Summary.SecondsUsed % 60;

                _finishOutcome = round.Summary.FinishOutcome;

                _isFinish = true;

                _result = FightResult.Finish;

                return;
            }
        }
    }

    private void DetermainWinner()
    {
        if(_isFinish)
            return;
                
        if(_fighter1.Points == _fighter2.Points)
        {
            _result = FightResult.Draw;

            return;
        }

        _result = FightResult.Decision;

        if(_fighter1.Points > _fighter2.Points)
        {
            _winner = _fighter1;
            _loser = _fighter2;

            return;
        }

        _winner = _fighter2;
        _loser = _fighter1;

    }

    private void CalculateRankingPointChange()
    {
        int difference =
            _loser!.RankingPoints - _winner!.RankingPoints;

        int gain = 20 + (difference / 10);

        if (_result != FightResult.Decision)
            gain *= 2;

        _rankingPointChange = Math.Clamp(gain, 5, 50);

        _winner.RankingPoints += _rankingPointChange;
        _loser.RankingPoints -= _rankingPointChange;
    }

    private void CalculateDrawRankingPointChange()
    {
        int difference =
            _fighter1.RankingPoints - _fighter2.RankingPoints;

        int change = Math.Abs(difference) / 20;

        change = Math.Clamp(change, 0, 15);

        if (difference > 0)
        {
            // Fighter 1 was higher ranked
            _fighter1.RankingPoints -= change;
            _fighter2.RankingPoints += change;
        }
        else if (difference < 0)
        {
            // Fighter 2 was higher ranked
            _fighter1.RankingPoints += change;
            _fighter2.RankingPoints -= change;
        }

        // Equal ranking = no change
    }

    private void UpdateStats()
    {
        if(_result == FightResult.Draw)
        {
            
            _fighter1.Draws++;
            _fighter2.Draws++;

            CalculateDrawRankingPointChange();

            return;
        }

        _winner!.Wins++;
        _loser!.Losses++;

        CalculateRankingPointChange();
    }

    private void UpdateFightHistory()
    {
        _fighter1.FightHistory.Add(Summary);
        _fighter2.FightHistory.Add(Summary);
    }

    private void CreateSumary()
    {
        Summary = new FightSummary(
            _fighter1,
            _fighter2,
            _winner,
            _loser,
            _result,
            _isFinish,
            _finishOutcome,
            _isFinish ? _finishRound : null,
            _isFinish ? _finishMinute : null,
            _isFinish ? _finishSecond : null,
            _rankingPointChange,
            _rounds
        );
    }

    
}