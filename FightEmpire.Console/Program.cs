using FightEmpire;
using FightEmpire.Core.Generation;
using FightEmpire.Core.Models;

FighterGenerator fighterGenerator = new();

List<Fighter> fighters = fighterGenerator.generateFighters(10);

Fighter Fighter1 = fighters[0];
Fighter Fighter2 = fighters[1];

//RunFight(fighter1, fighter2, 3);
//RunDebugFight(fighter1, fighter2, 3);
RunSimulations(3, 10000);

void RunFight(Fighter fighter1, Fighter fighter2, int numberOfRounds)
{
    Fight fight = new(fighter1, fighter2, numberOfRounds);

    fight.Run();

    foreach (RoundSummary round in fight.RoundSummaries)
    {
        PrintRoundSummary(round);
    }

    if(fight.Result == RoundOutcome.Decision)
    {
        Console.WriteLine(
        $"Winner: {fight.Winner!.FirstName} {fight.Winner.LastName} by Dcision with a score of {fight.Winner.Points}-{fight.Looser!.Points}");

        return;
    }

    Console.WriteLine(
        $"Winner: {fight.Winner!.FirstName} {fight.Winner.LastName} by {fight.Result}"
    );
}

void RunDebugFight(Fighter fighter1, Fighter fighter2, int numberOfRounds)
{
    Fight fight = new(fighter1, fighter2, numberOfRounds);

    Console.WriteLine($"""
    ===== DEBUG FIGHT =====

    Fighter 1:
    {fighter1.FirstName} {fighter1.LastName}
    Striking: {fighter1.Striking}
    Wrestling: {fighter1.Wrestling}
    Grappling: {fighter1.Grappling}
    Preferred style: {fighter1.PreferredStyle}

    Fighter 2:
    {fighter2.FirstName} {fighter2.LastName}
    Striking: {fighter2.Striking}
    Wrestling: {fighter2.Wrestling}
    Grappling: {fighter2.Grappling}
    Preferred style: {fighter2.PreferredStyle}

    =======================
    """);

    fight.Run();

    foreach (RoundSummary round in fight.RoundSummaries)
    {
        Console.WriteLine($"""
        

        Style winner:
        {round.StyleWinner.FirstName} {round.StyleWinner.LastName}

        Round style:
        {round.style}

        Round winner:
        {round.Winner.FirstName} {round.Winner.LastName}

        Outcome:
        {round.outcome}

        Finish:
        {round.isFinish}

        --------------------
        """);
    }

    Console.WriteLine($"""
    ===== FIGHT RESULT =====

    Winner:
    {fight.Winner!.FirstName} {fight.Winner.LastName}

    Result:
    {fight.Result}

    ========================
    """);
}

void PrintRoundSummary(RoundSummary round)
{
    Console.WriteLine($"""
    Round {round.RoundNumber}

    {round.StyleWinner.FirstName} {round.StyleWinner.LastName}
    wins the style battle and makes it a {round.style} round.

    {round.Winner.FirstName} {round.Winner.LastName}
    wins the round.

    Outcome: {round.outcome}
    """);
}

void RunSimulations(
    int numberOfRounds,
    int numberOfSimulations)
{
    Fighter fighter1 = new Fighter
    {
        FirstName = "Fighter",
        Nickname = "The beast",
        LastName = "1",
        Age = 25,
        Striking = 50,
        Wrestling = 50,
        Grappling = 50,
        Cardio = 50

    };

    Fighter fighter2 = new Fighter
    {
        FirstName = "Fighter",
        Nickname = "Mushrooms",
        LastName = "2",
        Age = 25,
        Striking = 50,
        Wrestling = 50,
        Grappling = 50,
        Cardio = 50

    };

    Dictionary<RoundOutcome, int> resultCounts = new()
    {
        { RoundOutcome.Knockout, 0 },
        { RoundOutcome.TKO, 0 },
        { RoundOutcome.Submission, 0 },
        { RoundOutcome.Decision, 0 }
    };

    int fighter1WinsBefore = fighter1.Wins;
    int fighter2WinsBefore = fighter2.Wins;

    for (int i = 0; i < numberOfSimulations; i++)
    {
        Fight fight = new(fighter1, fighter2, numberOfRounds);

        fight.Run();

        if (fight.Result is not null)
        {
            resultCounts[fight.Result.Value]++;
        }
    }

    int fighter1SimulationWins = fighter1.Wins - fighter1WinsBefore;
    int fighter2SimulationWins = fighter2.Wins - fighter2WinsBefore;

    double fighter1WinRate =
        (double)fighter1SimulationWins / numberOfSimulations * 100;

    double fighter2WinRate =
        (double)fighter2SimulationWins / numberOfSimulations * 100;

    Console.WriteLine($"""
    {fighter1.FirstName} {fighter1.LastName} ({GetStatTotal(fighter1)})
    {fighter1SimulationWins} wins ({fighter1WinRate:F1}%)

    {fighter2.FirstName} {fighter2.LastName} ({GetStatTotal(fighter2)})
    {fighter2SimulationWins} wins ({fighter2WinRate:F1}%)

    Results
    Knockouts: {GetPercentage(resultCounts, RoundOutcome.Knockout, numberOfSimulations)}
    Technical knockouts: {GetPercentage(resultCounts, RoundOutcome.TKO, numberOfSimulations)}
    Submissions: {GetPercentage(resultCounts, RoundOutcome.Submission, numberOfSimulations)}
    Decisions: {GetPercentage(resultCounts, RoundOutcome.Decision, numberOfSimulations)}
    """);
}

int GetStatTotal(Fighter fighter)
{
    return fighter.Striking
        + fighter.Wrestling
        + fighter.Grappling;
}

string GetPercentage(
    Dictionary<RoundOutcome, int> counts,
    RoundOutcome outcome,
    int total)
{
    int count = counts[outcome];
    double percentage = (double)count / total * 100;

    return $"{count} ({percentage:F1}%)";
}