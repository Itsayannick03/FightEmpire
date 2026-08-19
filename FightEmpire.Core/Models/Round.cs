using FightEmpire.Core.Models;

namespace FightEmpire;
public class Round
{
    public List<ExchangeSumary> Exchanges = new();

    private Fighter _fighter1;
    private Fighter _fighter2;
    // Round result/state
    private Fighter? Winner;
    private Fighter? Looser;
    private RoundOutcome Result;

    public bool IsFinish = false;
    public ActionOutcome? FinishOutcome;
    public RoundSummary Summary = null!;

    private int _secondsLeft = 300;

    private FightPosition _currentPosition = FightPosition.Standing;
    private int _roundNumber;

    public Round(Fighter fighter1, Fighter fighter2, int roundNumber)
    {
        _fighter1 = fighter1;
        _fighter2 = fighter2;

        _roundNumber = roundNumber;

    }

    public void Run()
    {
        ResetFighters();
        RunExchanges();
        DetermainRoundWinner();
        CreateSummary();
    }

    private void ResetFighters()
    {
        _fighter1.RoundReset();
        _fighter2.RoundReset();
    }

    private void RunExchanges()
    {

        while(_secondsLeft > 0)
        {
            Exchange exchange = new(_fighter1, _fighter2, _currentPosition, _secondsLeft);

            exchange.Run();

            Exchanges.Add(exchange.Sumary);

            if(exchange.IsFinish)
            {
                IsFinish = true;

                Winner = exchange.Winner;
                Looser = exchange.Looser;

                FinishOutcome = exchange.FinishOutcome;

                Result = RoundOutcome.Finish;

                return;
            }

            _currentPosition = exchange.CurrentPosition;
            _secondsLeft -= exchange.TimeTaken;
        }

        return;
    }

    private void DetermainRoundWinner()
    {
        if(IsFinish)
            return;
        
        if(_fighter1.RoundScore == _fighter2.RoundScore)
            GetRandomWinner();

        else if(_fighter1.RoundScore > _fighter2.RoundScore)
        {
            Winner = _fighter1;
            Looser = _fighter2;
        }
        else
        {
            Winner = _fighter2;
            Looser = _fighter1;
        }

        Winner!.WinRound();

        if(Winner.RoundScore >= Looser!.RoundScore * 2)
        {
            Result = RoundOutcome.TenEight;
            Looser.LoseRound10_8();
        }
        else
        {
            Result = RoundOutcome.TenNine;
            Looser.LoseRound();
        }
    }

    private void GetRandomWinner()
    {
        if(Random.Shared.Next(2) == 0)
        {
            Winner = _fighter1;
            Looser = _fighter2;
            
            return;
        }

        Winner = _fighter2;
        Looser = _fighter1;
    }

    private void CreateSummary()
{
    int secondsUsed = 300 - _secondsLeft;

    Summary = new RoundSummary(
        _roundNumber,
        Winner!,
        Looser!,
        Result,
        IsFinish,
        FinishOutcome,
        secondsUsed,
        _currentPosition,
        Exchanges
    );
}
}