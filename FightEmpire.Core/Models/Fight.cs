using FightEmpire.Core.Models;

public class Fight(Fighter Fighter1, Fighter Fighter2)
{
    Random random = new Random();
    public Fighter run()
    {
        int fighther1Performance = getPerformance(Fighter1);
        int fighther2Performance = getPerformance(Fighter2);

        if(fighther1Performance > fighther2Performance)
        {
            Fighter1.Wins++;
            Fighter2.Losses++;

            return Fighter1;
        }

        Fighter2.Wins++;
        Fighter1.Losses++;

        return Fighter2;

    }

    private int getPerformance(Fighter fighter)
    {
        return fighter.Striking + fighter.Wrestling + fighter.Grappling + random.Next(1, 101);
    }
}