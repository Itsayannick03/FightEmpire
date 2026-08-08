using FightEmpire;
using FightEmpire.Core.Models;

public class Fight(Fighter Fighter1, Fighter Fighter2, int NumberOfRounds)
{

    public Fighter? Winner { get; private set; }
    public Fighter? Looser { get; private set; }
    public RoundOutcome? Result { get; private set; }

    public int FinishRound { get; private set; }

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

                UpdateRecords();
                UpdateHistories();

                return;
            }
        }

        DetermainWinnerByDecision();
        FinishRound = NumberOfRounds;

        UpdateRecords();
        UpdateHistories();

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

    private void UpdateRecords()
    {
        Winner!.Wins++;
        Looser!.Losses++;
    }

    private void UpdateHistories()
    {
        Winner!.UpdateFightHistory(this);
        Looser!.UpdateFightHistory(this);
    }

    private void ResetPoints()
    {
        Fighter1.ResetPoints();
        Fighter2.ResetPoints();
    }

    
}