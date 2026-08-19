using FightEmpire.Core.Models;

namespace FightEmpire;

public class RoundSummary
{
    public int RoundNumber { get; }

    public Fighter Winner { get; }
    public Fighter Looser { get; }

    public RoundOutcome Result { get; }

    public bool IsFinish { get; }

    public ActionOutcome? FinishOutcome { get; }

    public int SecondsUsed { get; }

    public FightPosition EndPosition { get; }

    public List<ExchangeSumary> Exchanges { get; }


    public RoundSummary(
        int roundNumber,
        Fighter winner,
        Fighter looser,
        RoundOutcome result,
        bool isFinish,
        ActionOutcome? finishOutcome,
        int secondsUsed,
        FightPosition endPosition,
        List<ExchangeSumary> exchanges)
    {
        RoundNumber = roundNumber;

        Winner = winner;
        Looser = looser;

        Result = result;

        IsFinish = isFinish;
        FinishOutcome = finishOutcome;

        SecondsUsed = secondsUsed;

        EndPosition = endPosition;

        Exchanges = exchanges;
    }
}