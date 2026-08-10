using FightEmpire;
using FightEmpire.Core.Generation;
using FightEmpire.Core.Models;


// --------------------
// Setup
// --------------------

FighterGenerator fighterGenerator = new();



// --------------------
// Pick what to run
// --------------------

// RunFightCommentary(fighter1, fighter2, 3);
// RunDebugFight(fighter1, fighter2, 3);
RunSimulations(3, 10000);
// TestFightRecord();


// --------------------
// Normal fight commentary
// --------------------

void RunFightCommentary(
    Fighter fighter1,
    Fighter fighter2,
    int numberOfRounds)
{
    Fight fight = new(fighter1, fighter2, numberOfRounds);

    Console.WriteLine($"""
    ========================================

    {fighter1.FirstName} "{fighter1.Nickname}" {fighter1.LastName}
    VS
    {fighter2.FirstName} "{fighter2.Nickname}" {fighter2.LastName}

    ========================================

    """);

    fight.Run();

    foreach (RoundSummary round in fight.RoundSummaries)
    {
        PrintRoundCommentary(round);
    }

    PrintFightResult(fight);
}


// --------------------
// Round commentary
// --------------------

void PrintRoundCommentary(RoundSummary round)
{
    Console.WriteLine($"""
    ===== ROUND {round.RoundNumber} =====

    """);

    foreach (Exchange exchange in round.Exchanges)
    {
        PrintExchangeCommentary(exchange);
    }

    if (!round.isFinish)
    {
        Console.WriteLine(
            $"{round.Winner.FirstName} {round.Winner.LastName} wins the round."
        );

        Console.WriteLine();
    }
}


// --------------------
// Exchange commentary
// --------------------

void PrintExchangeCommentary(Exchange exchange)
{
    Console.WriteLine($"Exchange {exchange.ExchangeNumber}:");

    Console.WriteLine(
        $"{exchange.StyleWinner!.FirstName} " +
        $"{exchange.StyleWinner.LastName} wins the style battle " +
        $"and makes it a {exchange.Style} exchange."
    );

    if (exchange.IsFinish)
    {
        Console.WriteLine(
            $"{exchange.Winner!.FirstName} " +
            $"{exchange.Winner.LastName} finishes " +
            $"{exchange.Looser!.FirstName} " +
            $"{exchange.Looser.LastName} by {exchange.Outcome}!"
        );

        Console.WriteLine();
        return;
    }

    Console.WriteLine(
        $"{exchange.Winner!.FirstName} " +
        $"{exchange.Winner.LastName} wins the exchange."
    );

    Console.WriteLine();
}


// --------------------
// Fight result
// --------------------

void PrintFightResult(Fight fight)
{
    Console.WriteLine("===== FIGHT RESULT =====");

    if (fight.Result == RoundOutcome.Decision)
    {
        Console.WriteLine(
            $"{fight.Winner!.FirstName} {fight.Winner.LastName} " +
            $"wins by Decision."
        );

        Console.WriteLine(
            $"Score: {fight.Winner.Points}-{fight.Looser!.Points}"
        );

        return;
    }

    Console.WriteLine(
        $"{fight.Winner!.FirstName} {fight.Winner.LastName} " +
        $"defeats {fight.Looser!.FirstName} {fight.Looser.LastName} " +
        $"by {fight.Result} in Round {fight.FinishRound}."
    );
}


// --------------------
// Debug fight
// --------------------

void RunDebugFight(
    Fighter fighter1,
    Fighter fighter2,
    int numberOfRounds)
{
    Fight fight = new(fighter1, fighter2, numberOfRounds);

    Console.WriteLine($"""
    ===== DEBUG FIGHT =====

    Fighter 1:
    {fighter1.FirstName} {fighter1.LastName}

    Striking: {fighter1.Striking}
    Wrestling: {fighter1.Wrestling}
    Grappling: {fighter1.Grappling}
    Cardio: {fighter1.Cardio}
    Preferred style: {fighter1.PreferredStyle}


    Fighter 2:
    {fighter2.FirstName} {fighter2.LastName}

    Striking: {fighter2.Striking}
    Wrestling: {fighter2.Wrestling}
    Grappling: {fighter2.Grappling}
    Cardio: {fighter2.Cardio}
    Preferred style: {fighter2.PreferredStyle}

    =======================

    """);

    fight.Run();

    foreach (RoundSummary round in fight.RoundSummaries)
    {
        Console.WriteLine($"""
        ===== ROUND {round.RoundNumber} =====

        Exchanges: {round.Exchanges.Count}
        Round winner: {round.Winner.FirstName} {round.Winner.LastName}
        Finish: {round.isFinish}
        Outcome: {round.outcome}

        """);

        foreach (Exchange exchange in round.Exchanges)
        {
            Console.WriteLine($"""
            Exchange {exchange.ExchangeNumber}

            Style:
            {exchange.Style}

            Style winner:
            {exchange.StyleWinner!.FirstName} {exchange.StyleWinner.LastName}

            Winner:
            {exchange.Winner!.FirstName} {exchange.Winner.LastName}

            Fighter 1 performance:
            {exchange.Fighter1Performance}

            Fighter 2 performance:
            {exchange.Fighter2Performance}

            Performance difference:
            {Math.Abs(
                exchange.Fighter1Performance
                - exchange.Fighter2Performance
            )}

            Finish:
            {exchange.IsFinish}

            Outcome:
            {exchange.Outcome}

            --------------------
            """);
        }
    }

    Console.WriteLine($"""
    ===== FIGHT RESULT =====

    Winner:
    {fight.Winner!.FirstName} {fight.Winner.LastName}

    Result:
    {fight.Result}

    Finish round:
    {fight.FinishRound}

    ========================
    """);
}


// --------------------
// Bulk simulations
// --------------------

void RunSimulations(
    int numberOfRounds,
    int numberOfSimulations)
{
    FighterGenerator generator = new();

    Dictionary<RoundOutcome, int> resultCounts = new()
    {
        { RoundOutcome.Knockout, 0 },
        { RoundOutcome.TKO, 0 },
        { RoundOutcome.Submission, 0 },
        { RoundOutcome.Decision, 0 }
    };

    int fighter1Wins = 0;
    int fighter2Wins = 0;

    int totalRounds = 0;
    int totalExchanges = 0;

    int totalFighter1Stats = 0;
    int totalFighter2Stats = 0;

    for (int i = 0; i < numberOfSimulations; i++)
    {
        List<Fighter> fighters = generator.generateFighters(2);

        Fighter fighter1 = fighters[0];
        Fighter fighter2 = fighters[1];

        totalFighter1Stats += GetStatTotal(fighter1);
        totalFighter2Stats += GetStatTotal(fighter2);

        Fight fight = new(
            fighter1,
            fighter2,
            numberOfRounds
        );

        fight.Run();

        if (fight.Winner == fighter1)
        {
            fighter1Wins++;
        }
        else if (fight.Winner == fighter2)
        {
            fighter2Wins++;
        }

        if (fight.Result is not null)
        {
            resultCounts[fight.Result.Value]++;
        }

        totalRounds += fight.RoundSummaries.Count;

        foreach (RoundSummary round in fight.RoundSummaries)
        {
            totalExchanges += round.Exchanges.Count;
        }
    }

    double fighter1WinRate =
        (double)fighter1Wins
        / numberOfSimulations
        * 100;

    double fighter2WinRate =
        (double)fighter2Wins
        / numberOfSimulations
        * 100;

    double averageFighter1Stats =
        (double)totalFighter1Stats
        / numberOfSimulations;

    double averageFighter2Stats =
        (double)totalFighter2Stats
        / numberOfSimulations;

    double averageRounds =
        (double)totalRounds
        / numberOfSimulations;

    double averageExchanges =
        (double)totalExchanges
        / numberOfSimulations;

    double averageExchangesPerRound =
        totalRounds == 0
            ? 0
            : (double)totalExchanges / totalRounds;

    Console.WriteLine($"""
    ===== SIMULATION RESULTS =====

    Fighter 1
    Average stats: {averageFighter1Stats:F1}
    Wins: {fighter1Wins}
    Win rate: {fighter1WinRate:F1}%

    Fighter 2
    Average stats: {averageFighter2Stats:F1}
    Wins: {fighter2Wins}
    Win rate: {fighter2WinRate:F1}%


    Results

    Knockouts:
    {GetPercentage(
        resultCounts,
        RoundOutcome.Knockout,
        numberOfSimulations
    )}

    Technical knockouts:
    {GetPercentage(
        resultCounts,
        RoundOutcome.TKO,
        numberOfSimulations
    )}

    Submissions:
    {GetPercentage(
        resultCounts,
        RoundOutcome.Submission,
        numberOfSimulations
    )}

    Decisions:
    {GetPercentage(
        resultCounts,
        RoundOutcome.Decision,
        numberOfSimulations
    )}


    Simulation stats

    Average rounds per fight:
    {averageRounds:F2}

    Average exchanges per fight:
    {averageExchanges:F2}

    Average exchanges per round:
    {averageExchangesPerRound:F2}

    ==============================
    """);
}


// --------------------
// Fight history test
// --------------------

void TestFightRecord()
{
    Fighter fighter1 = new()
    {
        FirstName = "Fighter",
        LastName = "1",
        Nickname = "The Beast",

        Age = 25,

        Striking = 70,
        Wrestling = 50,
        Grappling = 40,
        Cardio = 60
    };

    Fighter fighter2 = new()
    {
        FirstName = "Fighter",
        LastName = "2",
        Nickname = "Mushrooms",

        Age = 26,

        Striking = 45,
        Wrestling = 65,
        Grappling = 55,
        Cardio = 60
    };

    for (int i = 0; i < 5; i++)
    {
        Fight fight = new(
            fighter1,
            fighter2,
            3
        );

        fight.Run();
    }

    Console.WriteLine("===== FIGHT HISTORY =====");
    Console.WriteLine();

    Console.WriteLine(
        $"{fighter1.FirstName} {fighter1.LastName}"
    );

    Console.WriteLine(
        $"Record: {fighter1.Wins}-{fighter1.Losses}"
    );

    Console.WriteLine();

    foreach (Fight fight in fighter1.FightHistory)
    {
        bool won = fight.Winner == fighter1;

        Fighter opponent = won
            ? fight.Looser!
            : fight.Winner!;

        string result = won ? "W" : "L";

        Console.WriteLine(
            $"{result}\t" +
            $"{opponent.FirstName} {opponent.LastName}\t" +
            $"{fight.Result}\t" +
            $"R{fight.FinishRound}"
        );
    }
}


// --------------------
// Helpers
// --------------------

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

    double percentage =
        (double)count / total * 100;

    return $"{count} ({percentage:F1}%)";
}