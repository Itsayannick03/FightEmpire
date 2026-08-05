using FightEmpire.Core.Generation;
using FightEmpire.Core.Models;

FighterGenerator fighterGenerator = new();

List<Fighter> fighters = fighterGenerator.generateFighters(5);

Fighter fighter1 = SelectFighter(
    fighters,
    "Select the first fighter:"
);

Fighter fighter2;

while (true)
{
    fighter2 = SelectFighter(
        fighters,
        "Select the second fighter:"
    );

    if (fighter2 != fighter1)
    {
        break;
    }

    Console.WriteLine();
    Console.WriteLine("A fighter cannot fight themselves.");
    Console.WriteLine("Please select another fighter.");
}

Console.WriteLine();
Console.WriteLine("Fight selected:");
Console.WriteLine($"{fighter1.FirstName} vs {fighter2.FirstName}");

int numFights = 1000;

for(int _ = 0; _ < numFights; _++)
{
    Fight fight = new(fighter1, fighter2);

    fight.run();
}

double f1WinRate = ((double)fighter1.Wins / numFights) * 100;
double f2WinRate = ((double)fighter2.Wins / numFights) * 100;

Console.WriteLine($"""
{numFights} simulations

{fighter1.FirstName} {fighter1.LastName}: {fighter1.Wins} ({f1WinRate}%)
{fighter2.FirstName} {fighter2.LastName}: {fighter2.Wins} ({f2WinRate}%)
""");

static Fighter SelectFighter(
    List<Fighter> fighters,
    string message
)
{
    while (true)
    {
        Console.WriteLine();
        Console.WriteLine(message);
        Console.WriteLine();

        for (int i = 0; i < fighters.Count; i++)
        {
            Fighter fighter = fighters[i];

            Console.WriteLine(
                $"""{i + 1}. {fighter.FirstName} "{fighter.Nickname}" {fighter.LastName}"""
            );
        }

        Console.WriteLine();
        Console.Write("Enter fighter number: ");

        if (
            !int.TryParse(Console.ReadLine(), out int selection) ||
            selection < 1 ||
            selection > fighters.Count
        )
        {
            Console.WriteLine(
                $"Invalid choice. Enter a number between 1 and {fighters.Count}."
            );

            continue;
        }

        Fighter selectedFighter = fighters[selection - 1];

        Console.WriteLine();
        Console.WriteLine("Selected fighter:");
        Console.WriteLine();
        Console.WriteLine(selectedFighter);

        while (true)
        {
            Console.WriteLine();
            Console.Write("Choose this fighter? (y/n): ");

            string answer = Console.ReadLine()?.Trim().ToLower() ?? "";

            if (answer == "y" || answer == "yes")
            {
                return selectedFighter;
            }

            if (answer == "n" || answer == "no")
            {
                Console.WriteLine();
                Console.WriteLine("Returning to fighter selection...");
                break;
            }

            Console.WriteLine("Please enter y or n.");
        }
    }
}