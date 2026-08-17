using FightEmpire.Core.Models;

namespace FightEmpire;

public class FightSummary
{
    public Fighter Fighter1 { get; }
    public Fighter Fighter2 { get; }

    public Fighter? Winner { get; }
    public Fighter? Loser { get; }

    public FightResult Result { get; }

    public bool IsFinish { get; }
    public ActionOutcome? FinishOutcome { get; }

    public int? FinishRound { get; }
    public int? FinishMinute { get; }
    public int? FinishSecond { get; }

    public int RankingPointChange { get; }

    public IReadOnlyList<RoundSummary> Rounds { get; }

    public FightSummary(
        Fighter fighter1,
        Fighter fighter2,
        Fighter? winner,
        Fighter? loser,
        FightResult result,
        bool isFinish,
        ActionOutcome? finishOutcome,
        int? finishRound,
        int? finishMinute,
        int? finishSecond,
        int rankingPointChange,
        List<RoundSummary> rounds)
    {
        Fighter1 = fighter1;
        Fighter2 = fighter2;

        Winner = winner;
        Loser = loser;

        Result = result;

        IsFinish = isFinish;
        FinishOutcome = finishOutcome;

        FinishRound = finishRound;
        FinishMinute = finishMinute;
        FinishSecond = finishSecond;

        RankingPointChange = rankingPointChange;

        Rounds = rounds.AsReadOnly();
    }
}