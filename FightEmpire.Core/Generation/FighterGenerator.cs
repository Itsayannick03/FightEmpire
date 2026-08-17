using FightEmpire.Core.Models;

namespace FightEmpire.Core.Generation;

public class FighterGenerator
{
    private readonly Random random = new Random();
    public List<Fighter> generateFighters(int numberOfFighters)
    {
        List<Fighter> fighters = new List<Fighter>();

        for(int i = 0; i < numberOfFighters; i++)
        {
            Fighter fighter = GenerateFighter();

            fighters.Add(fighter);
        }

        return fighters;
    }

    public Fighter GenerateFighter()
    {
        Nationality nationality = Enum.GetValues<Nationality>()[Random.Shared.Next(Enum.GetValues<Nationality>().Length)];

        string country = nationality.ToString();

        string[] firstNames = FighterNames.FirstNames[country];
        string[] lastNames = FighterNames.LastNames[country];

        string FirstName =
            firstNames[Random.Shared.Next(firstNames.Length)];

        string LastName =
            lastNames[Random.Shared.Next(lastNames.Length)];

        string Nickname =
            FighterNames.Nicknames[
                Random.Shared.Next(FighterNames.Nicknames.Length)
            ];

        int Age = Random.Shared.Next(17, 40);
        
        int Striking = random.Next(1, 101);
        int Wrestling = random.Next(1, 101);
        int Grappling = random.Next(1, 101);
        int Cardio = random.Next(1, 101);

        Fighter fighter = new(FirstName, LastName, Nickname, Age, nationality, Striking, Wrestling, Grappling, Cardio);

        return fighter;
    }

    public Fighter GenerateWrestler()
    {
        {
        Nationality nationality = Enum.GetValues<Nationality>()[Random.Shared.Next(Enum.GetValues<Nationality>().Length)];

        string country = nationality.ToString();

        string[] firstNames = FighterNames.FirstNames[country];
        string[] lastNames = FighterNames.LastNames[country];

        string FirstName =
            firstNames[Random.Shared.Next(firstNames.Length)];

        string LastName =
            lastNames[Random.Shared.Next(lastNames.Length)];

        string Nickname =
            FighterNames.Nicknames[
                Random.Shared.Next(FighterNames.Nicknames.Length)
            ];

        int Age = Random.Shared.Next(17, 40);
        
        int Striking = random.Next(1, 51);
        int Wrestling = random.Next(60, 101);
        int Grappling = random.Next(40, 81);
        int Cardio = random.Next(50, 101);

        Fighter fighter = new(FirstName, LastName, Nickname, Age, nationality, Striking, Wrestling, Grappling, Cardio);

        return fighter;
    }
    }
}