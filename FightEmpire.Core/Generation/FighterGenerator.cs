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
            Fighter fighter = new Fighter
            {
                
                FirstName = FighterNames.FirstNames[random.Next(FighterNames.FirstNames.Length)],
                LastName = FighterNames.LastNames[random.Next(FighterNames.LastNames.Length)],
                Nickname = FighterNames.Nicknames[random.Next(FighterNames.Nicknames.Length)],
                Age = random.Next(17, 40),
                Striking = random.Next(1, 101),
                Wrestling = random.Next(1, 101),
                Grappling = random.Next(1, 101),
                Cardio = random.Next(1, 101)
            };

            fighters.Add(fighter);
        }

        return fighters;
        
    }
}