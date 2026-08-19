using FightEmpire;
using FightEmpire.Core.Generation;
using FightEmpire.Core.Models;


// ============================================================
// SETUP
// ============================================================

FighterGenerator fighterGenerator = new();

List<Fighter> fighters = fighterGenerator.generateFighters(2);

Fighter fighter1 = fighters[0];
Fighter fighter2 = fighters[1];


// ============================================================
// PICK WHAT TO RUN
// ============================================================

//RunFightCommentary(fighter1, fighter2, 3);

// RunDebugFight(fighter1, fighter2, 3);
RunSimulations(5, 10000);
// TestFightRecord();



// ============================================================
// NORMAL FIGHT COMMENTARY
// ============================================================

void RunFightCommentary(
    Fighter fighter1,
    Fighter fighter2,
    int numberOfRounds)
{
    Fight fight = new(
        fighter1,
        fighter2,
        numberOfRounds
    );

    Console.WriteLine($"""
    ========================================

    {GetFighterName(fighter1)}
                        VS
    {GetFighterName(fighter2)}

    ========================================

    """);

    fight.Run();

    FightSummary summary = fight.Summary;

    foreach (RoundSummary round in summary.Rounds)
    {
        PrintRoundCommentary(round);
    }

    PrintFightResult(summary);
}



// ============================================================
// ROUND COMMENTARY
// ============================================================

void PrintRoundCommentary(RoundSummary round)
{
    Console.WriteLine();
    Console.WriteLine(
        $"================ ROUND {round.RoundNumber} ================"
    );
    Console.WriteLine();

    int secondsElapsed = 0;

    foreach (ExchangeSumary exchange in round.Exchanges)
    {
        foreach (ActionSumary action in exchange.Actions)
        {
            secondsElapsed += action.TimeTaken;

            PrintActionCommentary(
                action,
                round.RoundNumber,
                secondsElapsed
            );

            if (action.IsFinish)
            {
                Console.WriteLine();
                PrintFinish(
                    action,
                    round.RoundNumber,
                    secondsElapsed
                );

                return;
            }
        }
    }

    Console.WriteLine();

    if (!round.IsFinish)
    {
        Console.WriteLine(
            $"--- END OF ROUND {round.RoundNumber} ---"
        );

        Console.WriteLine(
            $"{GetFighterName(round.Winner)} wins the round " +
            $"{FormatRoundResult(round.Result)}."
        );
    }

    Console.WriteLine();
}



// ============================================================
// ACTION COMMENTARY
// ============================================================

void PrintActionCommentary(
    ActionSumary action,
    int roundNumber,
    int secondsElapsed)
{
    int minutes = secondsElapsed / 60;
    int seconds = secondsElapsed % 60;

    string actor = GetFighterName(action.Actor);
    string defender = GetFighterName(action.Defender);

    string commentary = action.Outcome switch
    {
        ActionOutcome.StrikeLanded =>
            $"{actor} lands a clean strike on {defender}!",

        ActionOutcome.StrikeBlocked =>
            $"{defender} blocks the strike from {actor}.",

        ActionOutcome.KickLanded =>
            $"{actor} lands a kick!",

        ActionOutcome.KickBlocked =>
            $"{defender} blocks the kick from {actor}.",

        ActionOutcome.Takedown =>
            $"{actor} gets the takedown!",

        ActionOutcome.TakedownBlocked =>
            $"{defender} stuffs the takedown attempt!",

        ActionOutcome.GetUp =>
            $"{actor} gets back to the feet!",

        ActionOutcome.GetUpDenied =>
            $"{defender} keeps {actor} on the ground.",

        ActionOutcome.GroundStrikeLanded =>
            $"{actor} lands a ground strike!",

        ActionOutcome.GroundStrikeBlocked =>
            $"{defender} blocks the ground strike.",

        ActionOutcome.SubmissionProgress =>
            $"{actor} is working on a submission! " +
            $"{defender} is in trouble!",

        ActionOutcome.SubmissionDefense =>
            $"{defender} escapes the submission attempt!",

        ActionOutcome.Knockout =>
            $"{actor} lands a HUGE shot! {defender} is out!",

        ActionOutcome.TKO =>
            $"{actor} pours on the damage! " +
            $"The referee has seen enough!",

        ActionOutcome.Submission =>
            $"{actor} locks in the submission! {defender} taps!",

        _ =>
            $"{actor} attacks {defender}."
    };

    Console.WriteLine(
        $"[R{roundNumber} {minutes}:{seconds:00}] {commentary}"
    );
}



// ============================================================
// FINISH ANNOUNCEMENT
// ============================================================

void PrintFinish(
    ActionSumary action,
    int roundNumber,
    int secondsElapsed)
{
    int minutes = secondsElapsed / 60;
    int seconds = secondsElapsed % 60;

    string result = action.Outcome switch
    {
        ActionOutcome.Knockout => "KNOCKOUT",
        ActionOutcome.TKO => "TECHNICAL KNOCKOUT",
        ActionOutcome.Submission => "SUBMISSION",

        _ => "FINISH"
    };

    Console.WriteLine(
        "========================================"
    );

    Console.WriteLine(result);

    Console.WriteLine(
        $"{GetFighterName(action.Actor)} WINS!"
    );

    Console.WriteLine(
        $"Round {roundNumber} - {minutes}:{seconds:00}"
    );

    Console.WriteLine(
        "========================================"
    );
}



// ============================================================
// OFFICIAL FIGHT RESULT
// ============================================================

void PrintFightResult(FightSummary fight)
{
    Console.WriteLine();
    Console.WriteLine(
        "=============== OFFICIAL RESULT ==============="
    );
    Console.WriteLine();

    if (fight.Result == FightResult.Draw)
    {
        Console.WriteLine(
            $"{GetFighterName(fight.Fighter1)} vs " +
            $"{GetFighterName(fight.Fighter2)}"
        );

        Console.WriteLine();
        Console.WriteLine("DRAW");

        return;
    }

    Console.WriteLine(
        $"Winner: {GetFighterName(fight.Winner!)}"
    );

    Console.WriteLine(
        $"Loser:  {GetFighterName(fight.Loser!)}"
    );

    if (fight.Result == FightResult.Decision)
    {
        Console.WriteLine();
        Console.WriteLine("Method: Decision");

        return;
    }

    Console.WriteLine();
    Console.WriteLine(
        $"Method: {FormatFinishOutcome(fight.FinishOutcome)}"
    );

    Console.WriteLine(
        $"Round: {fight.FinishRound}"
    );

    Console.WriteLine(
        $"Time: {fight.FinishMinute}:{fight.FinishSecond:00}"
    );
}



// ============================================================
// DEBUG FIGHT
// ============================================================

void RunDebugFight(
    Fighter fighter1,
    Fighter fighter2,
    int numberOfRounds)
{
    Fight fight = new(
        fighter1,
        fighter2,
        numberOfRounds
    );

    Console.WriteLine($"""
    ================= DEBUG FIGHT =================

    Fighter 1:
    {GetFighterName(fighter1)}

    Striking:  {fighter1.Striking}
    Wrestling: {fighter1.Wrestling}
    Grappling: {fighter1.Grappling}
    Cardio:    {fighter1.Cardio}

    Preferred style:
    {fighter1.PreferredStyle}


    Fighter 2:
    {GetFighterName(fighter2)}

    Striking:  {fighter2.Striking}
    Wrestling: {fighter2.Wrestling}
    Grappling: {fighter2.Grappling}
    Cardio:    {fighter2.Cardio}

    Preferred style:
    {fighter2.PreferredStyle}

    ===============================================

    """);

    fight.Run();

    FightSummary summary = fight.Summary;

    foreach (RoundSummary round in summary.Rounds)
    {
        Console.WriteLine($"""
        ================= ROUND {round.RoundNumber} =================

        Exchanges:      {round.Exchanges.Count}
        Round winner:   {GetFighterName(round.Winner)}
        Result:         {round.Result}
        Finish:         {round.IsFinish}
        Finish outcome: {round.FinishOutcome}
        Seconds used:   {round.SecondsUsed}

        """);

        int exchangeNumber = 1;

        foreach (ExchangeSumary exchange in round.Exchanges)
        {
            Console.WriteLine($"""
            -------- EXCHANGE {exchangeNumber} --------

            Actions:       {exchange.Actions.Count}
            Winner:        {GetNullableFighterName(exchange.Winner)}
            Finish:        {exchange.IsFinish}
            FinishOutcome: {exchange.FinishOutcome}
            Time Taken:    {exchange.TimeTaken}s
            End Position:  {exchange.CurrentPosition}

            """);

            int actionNumber = 1;

            foreach (ActionSumary action in exchange.Actions)
            {
                Console.WriteLine($"""
                Action {actionNumber}

                Actor:      {GetFighterName(action.Actor)}
                Defender:   {GetFighterName(action.Defender)}

                Action:     {action.Action}
                Outcome:    {action.Outcome}

                Time:       {action.TimeTaken}s
                Finish:     {action.IsFinish}

                --------------------------
                """);

                actionNumber++;
            }

            exchangeNumber++;
        }
    }

    Console.WriteLine();

    PrintFightResult(summary);
}



// ============================================================
// BULK SIMULATIONS
// ============================================================

void RunSimulations(
    int numberOfRounds,
    int numberOfSimulations)
{
    FighterGenerator generator = new();

    Dictionary<FightResult, int> resultCounts = new()
    {
        { FightResult.Decision, 0 },
        { FightResult.Finish, 0 },
        { FightResult.Draw, 0 }
    };

    Dictionary<ActionOutcome, int> finishCounts = new()
    {
        { ActionOutcome.Knockout, 0 },
        { ActionOutcome.TKO, 0 },
        { ActionOutcome.Submission, 0 }
    };

    int fighter1Wins = 0;
    int fighter2Wins = 0;
    int draws = 0;

    int totalRounds = 0;
    int totalExchanges = 0;
    int totalActions = 0;

    int totalFighter1Stats = 0;
    int totalFighter2Stats = 0;


    for (int i = 0; i < numberOfSimulations; i++)
    {
        List<Fighter> fighters =
            generator.generateFighters(2);

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

        FightSummary summary = fight.Summary;

        resultCounts[summary.Result]++;


        // --------------------
        // Winner stats
        // --------------------

        if (summary.Result == FightResult.Draw)
        {
            draws++;
        }
        else if (summary.Winner == fighter1)
        {
            fighter1Wins++;
        }
        else if (summary.Winner == fighter2)
        {
            fighter2Wins++;
        }


        // --------------------
        // Finish stats
        // --------------------

        if (
            summary.IsFinish &&
            summary.FinishOutcome is not null &&
            finishCounts.ContainsKey(summary.FinishOutcome.Value)
        )
        {
            finishCounts[summary.FinishOutcome.Value]++;
        }


        // --------------------
        // Simulation size stats
        // --------------------

        totalRounds += summary.Rounds.Count;

        foreach (RoundSummary round in summary.Rounds)
        {
            totalExchanges += round.Exchanges.Count;

            foreach (ExchangeSumary exchange in round.Exchanges)
            {
                totalActions += exchange.Actions.Count;
            }
        }
    }


    // ========================================================
    // CALCULATIONS
    // ========================================================

    double fighter1WinRate =
        (double)fighter1Wins /
        numberOfSimulations *
        100;

    double fighter2WinRate =
        (double)fighter2Wins /
        numberOfSimulations *
        100;

    double drawRate =
        (double)draws /
        numberOfSimulations *
        100;


    double averageFighter1Stats =
        (double)totalFighter1Stats /
        numberOfSimulations;

    double averageFighter2Stats =
        (double)totalFighter2Stats /
        numberOfSimulations;


    double averageRounds =
        (double)totalRounds /
        numberOfSimulations;

    double averageExchanges =
        (double)totalExchanges /
        numberOfSimulations;

    double averageActions =
        (double)totalActions /
        numberOfSimulations;


    double averageExchangesPerRound =
        totalRounds == 0
            ? 0
            : (double)totalExchanges / totalRounds;


    double averageActionsPerExchange =
        totalExchanges == 0
            ? 0
            : (double)totalActions / totalExchanges;



    // ========================================================
    // RESULTS
    // ========================================================

    Console.WriteLine($"""
    ================= SIMULATION RESULTS =================


    FIGHTER 1

    Average stats:
    {averageFighter1Stats:F1}

    Wins:
    {fighter1Wins}

    Win rate:
    {fighter1WinRate:F1}%


    FIGHTER 2

    Average stats:
    {averageFighter2Stats:F1}

    Wins:
    {fighter2Wins}

    Win rate:
    {fighter2WinRate:F1}%


    DRAWS

    {draws} ({drawRate:F1}%)


    ------------------------------------------------------

    FIGHT RESULTS

    Decisions:
    {GetPercentage(
        resultCounts,
        FightResult.Decision,
        numberOfSimulations
    )}

    Finishes:
    {GetPercentage(
        resultCounts,
        FightResult.Finish,
        numberOfSimulations
    )}

    Draws:
    {GetPercentage(
        resultCounts,
        FightResult.Draw,
        numberOfSimulations
    )}


    ------------------------------------------------------

    FINISH TYPES

    Knockouts:
    {GetPercentage(
        finishCounts,
        ActionOutcome.Knockout,
        numberOfSimulations
    )}

    Technical knockouts:
    {GetPercentage(
        finishCounts,
        ActionOutcome.TKO,
        numberOfSimulations
    )}

    Submissions:
    {GetPercentage(
        finishCounts,
        ActionOutcome.Submission,
        numberOfSimulations
    )}


    ------------------------------------------------------

    SIMULATION SIZE

    Average rounds per fight:
    {averageRounds:F2}

    Average exchanges per fight:
    {averageExchanges:F2}

    Average exchanges per round:
    {averageExchangesPerRound:F2}

    Average actions per fight:
    {averageActions:F2}

    Average actions per exchange:
    {averageActionsPerExchange:F2}


    ======================================================
    """);
}



// ============================================================
// FIGHT HISTORY TEST
// ============================================================

void TestFightRecord()
{
    Fighter fighter1 = new(
        "Fighter",
        "1",
        "The Beast",
        25,
        Nationality.USA,
        70,
        50,
        40,
        60
    );

    Fighter fighter2 = new(
        "Fighter",
        "2",
        "Mushrooms",
        26,
        Nationality.Russia,
        45,
        65,
        55,
        60
    );


    for (int i = 0; i < 5; i++)
    {
        Fight fight = new(
            fighter1,
            fighter2,
            3
        );

        fight.Run();
    }


    Console.WriteLine(
        "============= FIGHT HISTORY ============="
    );

    Console.WriteLine();

    Console.WriteLine(
        GetFighterName(fighter1)
    );

    Console.WriteLine(
        $"Record: " +
        $"{fighter1.Wins}-" +
        $"{fighter1.Losses}-" +
        $"{fighter1.Draws}"
    );

    Console.WriteLine();


    foreach (FightSummary fight in fighter1.FightHistory)
    {
        Fighter opponent =
            fight.Fighter1 == fighter1
                ? fight.Fighter2
                : fight.Fighter1;


        string result;

        if (fight.Result == FightResult.Draw)
        {
            result = "D";
        }
        else
        {
            result =
                fight.Winner == fighter1
                    ? "W"
                    : "L";
        }


        string method;

        if (fight.IsFinish)
        {
            method =
                $"{FormatFinishOutcome(fight.FinishOutcome)} " +
                $"R{fight.FinishRound} " +
                $"{fight.FinishMinute}:{fight.FinishSecond:00}";
        }
        else
        {
            method = fight.Result.ToString();
        }


        Console.WriteLine(
            $"{result}\t" +
            $"{GetFighterName(opponent)}\t" +
            $"{method}"
        );
    }
}



// ============================================================
// HELPERS
// ============================================================

int GetStatTotal(Fighter fighter)
{
    return fighter.Striking
        + fighter.Wrestling
        + fighter.Grappling;
}


string GetFighterName(Fighter fighter)
{
    if (string.IsNullOrWhiteSpace(fighter.Nickname))
    {
        return
            $"{fighter.FirstName} " +
            $"{fighter.LastName}";
    }

    return
        $"{fighter.FirstName} " +
        $"\"{fighter.Nickname}\" " +
        $"{fighter.LastName}";
}


string GetNullableFighterName(Fighter? fighter)
{
    return fighter is null
        ? "None"
        : GetFighterName(fighter);
}


string FormatRoundResult(RoundOutcome outcome)
{
    return outcome switch
    {
        RoundOutcome.TenNine => "10-9",
        RoundOutcome.TenEight => "10-8",
        RoundOutcome.Finish => "Finish",

        _ => outcome.ToString()
    };
}


string FormatFinishOutcome(ActionOutcome? outcome)
{
    return outcome switch
    {
        ActionOutcome.Knockout => "KO",
        ActionOutcome.TKO => "TKO",
        ActionOutcome.Submission => "Submission",

        _ => "Unknown"
    };
}


string GetPercentage<T>(
    Dictionary<T, int> counts,
    T value,
    int total)
    where T : notnull
{
    int count = counts[value];

    double percentage =
        total == 0
            ? 0
            : (double)count / total * 100;

    return $"{count} ({percentage:F1}%)";
}