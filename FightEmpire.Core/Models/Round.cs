using FightEmpire.Core.Models;

namespace FightEmpire;
public class Round(Fighter fighter1, Fighter fighter2, int roundNumber)
{
    // Round result/state
    private Fighter? Winner = null;
    private Fighter? Looser = null;
    private RoundOutcome? Result = null;

    private bool IsFinish = false;
    public bool hasBeenRun = false;

    public RoundSummary? Summary;

    private int _secondsLeft = 300;


    // Exchange tracking
    private int _numberOfExchanges;


    // Other
    private Random random = new();

    public void Run()
    {
        ResetFighters();

        DetermainNumberOfExchanges();

        List<Exchange> exchanges = RunExchanges();

        if(IsFinish)
        {
            SetFinishResult(exchanges[^1]);
        }
        else
        {
            DetermainRoundWinner(exchanges);
            Result = RoundOutcome.NoFinish;
        }

        Summary = new RoundSummary
        {
            RoundNumber = roundNumber,
            Winner = Winner!,
            Looser = Looser!,
            outcome = Result,
            isFinish = IsFinish,
            Exchanges = exchanges
        };

        hasBeenRun = true;
    }

    

    private void DetermainNumberOfExchanges()
    {
        _numberOfExchanges = random.Next(5,15);
    }

    private List<Exchange> RunExchanges()
    {
        List<Exchange> exchangeList = new();

        while(_secondsLeft > 0)
        {
            Exchange exchange = new(fighter1, fighter2, _secondsLeft, roundNumber);

            exchange.Run();

            exchangeList.Add(exchange);

            if(exchange.IsFinish)
            {
                IsFinish = true;
                break;
            }

            _secondsLeft -= random.Next(5,15);
        }

        return exchangeList;
    }

    private void SetFinishResult(Exchange exchange)
    {
        Winner = exchange.Winner;
        Looser = exchange.Looser;

        Result = exchange.Outcome;
    }

    private void DetermainRoundWinner(List<Exchange> exchanges)
    {
        foreach(Exchange exchange in exchanges)
        {
            exchange.Winner!.ExchangeWins++;
        }

        if (fighter1.ExchangeWins == fighter2.ExchangeWins)
        {
            GetRandomWinner();
        }

        else if(fighter1.ExchangeWins > fighter2.ExchangeWins)
        {
            Winner = fighter1;
            Looser = fighter2;
        }
        else
        {
            Winner = fighter2;
            Looser = fighter1;
        }

        Winner!.WinRound();
        Looser!.LoseRound();
    }

    private void GetRandomWinner()
    {
        if(random.Next(0,101) > 50)
        {
            Winner = fighter1;
            Looser = fighter2;

            return;
        }

        Winner = fighter2;
        Looser = fighter1; 
    }

    private void ResetFighters()
    {
        fighter1.ResetExchangeWins();
        fighter2.ResetExchangeWins();
    }
}