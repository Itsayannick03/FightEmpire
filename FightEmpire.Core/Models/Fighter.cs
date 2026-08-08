namespace FightEmpire.Core.Models;

public class Fighter
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Nickname { get; set; } = string.Empty;
    public int Age { get; set; }

    public int Striking { get; set; }
    public int Wrestling { get; set; }
    public int Grappling { get; set; }
    public int Cardio { get; set; }

    public List<Fight> FightHistory {get; private set;} = new();

    public int StatsTotal => Striking + Wrestling + Grappling;

    public FightingStyle PreferredStyle => getPreferedStyle();
    public int PreferedStat => getPreferedStyleStat(PreferredStyle);

    public int Wins { get; set; } = 0 ;
    public int Losses { get; set; } = 0; 

    public int Points { get; set; } = 0;

    public int ExchangeWins = 0;


    

    public override string ToString()
    {
        return $"""
        {FirstName} "{Nickname}" {LastName}
        {Wins}-{Losses}

        Striking: {Striking}
        Wrestling: {Wrestling}
        Grappling: {Grappling}
        Cardio: {Cardio}

        Fight History:
        {GetFightHistory()}
        """;
        
    }

    private string GetFightHistory()
    {
        if (FightHistory.Count == 0)
            return "No fights yet.";

        string history = "";

        int fightNumber = 1;
        foreach (Fight fight in FightHistory)
        {
            history += $"{fightNumber}.\t";
            bool won = fight.Winner == this;

            Fighter opponent = won
                ? fight.Looser!
                : fight.Winner!;

            string result = won ? "W" : "L";

            history +=
                $"{result}  " +
                $"{opponent.FirstName} {opponent.LastName}  " +
                $"{fight.Result}  " +
                $"R{fight.FinishRound}\n";
            fightNumber++;
        }

        return history;
    }

    private FightingStyle getPreferedStyle()
    {
        int highest = Math.Max(Striking, Math.Max(Wrestling, Grappling));

        List<FightingStyle> bestStyles = new();
        
        if(Striking == highest)
            bestStyles.Add(FightingStyle.Standup);

        if(Wrestling == highest)
            bestStyles.Add(FightingStyle.Wrestling);

        if(Grappling == highest)
            bestStyles.Add(FightingStyle.Grappling);
        
        return bestStyles[Random.Shared.Next(bestStyles.Count)];

        
    }

    private int getPreferedStyleStat(FightingStyle fightingStyle)
    {
        switch (fightingStyle)
        {
            case FightingStyle.Standup:
                return this.Striking;
            case FightingStyle.Wrestling:
                return this.Wrestling;
            case FightingStyle.Grappling:
                return this.Grappling;
            default:
                return this.Striking;
        }
    }

    public void UpdateFightHistory(Fight fight)
    {
        FightHistory.Add(fight);
    }

    public void WinRound()
    {
        this.Points += 10;
    }

    public void LoseRound()
    {
        this.Points += 9;
    }

    public void ResetPoints()
    {
        this.Points = 0;
    }

    public void ResetExchangeWins()
    {
        this.ExchangeWins = 0;
    }
}