using FightEmpire;
using FightEmpire.Core.Models;

public class Fight(Fighter Fighter1, Fighter Fighter2, int NumberOfRounds)
{

    public Fighter? Winner { get; private set; }
    public Fighter? Looser { get; private set; }
    public RoundOutcome? Result { get; private set; }

    public int FinishRound { get; private set; }
    public int FinishMinute { get; private set; }
    public int FinishSecond { get; private set; }

    public FightSummary FightSummary;

    public int RankingPointChange;

    public List<RoundSummary> RoundSummaries { get; } = new();

    Random random = new Random();
    public void Run()
    {
        ResetPoints();

        for(int roundNumber = 1; roundNumber <= NumberOfRounds; roundNumber++)
        {
            RoundSummary summary = RunRound(roundNumber);

            RoundSummaries.Add(summary);

            if(summary.isFinish)
            {
                FinishFight(summary);

                UpdateFigtherStats();

                return;
            }
        }

        DetermainWinnerByDecision();
        FinishRound = NumberOfRounds;

        UpdateFigtherStats();

        FightSummary = new()
        {
          Winner = this.Winner!,
          Looser = this.Looser!,
          Result = this.Result,
          FinishRound = this.FinishRound
        };

    }

    private RoundSummary RunRound(int roundNumber)
    {
        Round round = new(Fighter1, Fighter2, roundNumber);

        round.Run();

        return round.Summary!;
    }

    private void FinishFight(RoundSummary summary)
    {
        Winner = summary.Winner;
        Looser = summary.Looser;
        Result = summary.outcome;
        FinishRound = summary.RoundNumber;

        FinishMinute = summary.Exchanges[^1].Minute;
        FinishSecond = summary.Exchanges[^1].Second;

    }

    private void DetermainWinnerByDecision()
    {
        if(Fighter1.Points > Fighter2.Points)
        {
            Winner = Fighter1;
            Looser = Fighter2;
        }
        else
        {
            Winner = Fighter2;
            Looser = Fighter1;
        }

        Result = RoundOutcome.Decision;
    }

    private void UpdateFigtherStats()
    {
        CalculateRankingPointChange();

        Winner!.WinFight(this);
        Looser!.LooseFight(this);
    }

    private void CalculateRankingPointChange()
    {
        int difference =
            Looser!.RankingPoints - Winner!.RankingPoints;

        int gain = 20 + (difference / 10);

        if(!(Result == RoundOutcome.Decision))
            RankingPointChange *= 2;

        RankingPointChange = Math.Clamp(gain, 5, 50);
    }

    private void ResetPoints()
    {
        Fighter1.ResetPoints();
        Fighter2.ResetPoints();
    }

    
}