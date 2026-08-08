using FightEmpire.Core.Models;

namespace FightEmpire;
public class Exchange(Fighter fighter1, Fighter fighter2, int secondsLeft, int roundNumber)
{
    // Where we are
    public int ExchangeNumber { get; private set; }
    public int RoundNumber = roundNumber;

    // Result of the exchange
    public Fighter? Winner { get; private set; }
    public Fighter? Looser { get; private set; }

    // Who managed to impose their preferred style
    public Fighter? StyleWinner { get; private set; }

    public int Minute = secondsLeft / 60;
    public int Second = secondsLeft % 60;

    // What kind of exchange it became
    public FightingStyle Style { get; private set; }

    // Performance rolls
    public int Fighter1Performance { get; private set; }
    public int Fighter2Performance { get; private set; }

    // Did this exchange end the fight?
    public bool IsFinish { get; private set; }

    // If there was a finish, what kind?
    public RoundOutcome Outcome { get; private set; }

    Random random = new();

    public void Run()
    {
        Style = DetermineStyle();

        CalculatePerformances(Style);

        DetermineWinner();

        DetermineOutcome();
    }

    private FightingStyle DetermineStyle()
    {
        int fighter1StyleScore = fighter1.PreferedStat * random.Next(-50, 51);
        int fighter2StyleScore = fighter2.PreferedStat * random.Next(-50, 51);

        if(fighter1StyleScore == fighter2StyleScore)
        {
            if(random.Next(0, 101) > 50)
            {
                this.StyleWinner = fighter1;
                return fighter1.PreferredStyle;

            }
            this.StyleWinner = fighter2;
            return fighter2.PreferredStyle;
        }

        if(fighter1StyleScore > fighter2StyleScore)
        {
            this.StyleWinner = fighter1;

            return fighter1.PreferredStyle;

        }

        this.StyleWinner = fighter2;

        return fighter2.PreferredStyle;
    }

    private void CalculatePerformances(FightingStyle fightingStyle)
    {
        Fighter1Performance = CalculatePerformance(fightingStyle, fighter1);
        Fighter2Performance = CalculatePerformance(fightingStyle, fighter2);
    }

    private int CalculatePerformance(FightingStyle fightingStyle, Fighter fighter)
    {
        switch (fightingStyle)
        {
            case FightingStyle.Standup:
                return (fighter.Striking * 2) + fighter.Wrestling + fighter.Grappling + random.Next(-100, 101);
            case FightingStyle.Wrestling:
                return fighter.Striking  + (fighter.Wrestling * 2) + fighter.Grappling + random.Next(-100, 101);
            case FightingStyle.Grappling:
                return fighter.Striking  + fighter.Wrestling  + (fighter.Grappling * 2) + random.Next(-100, 101);    
            default:
                return fighter.Striking  + fighter.Wrestling  + fighter.Grappling + random.Next(-100, 101); 
        }
    }

    private void DetermineWinner()
    {
        if(Fighter1Performance == Fighter2Performance)
        {
            this.GetRandomWinner();
        }

        else if(Fighter1Performance > Fighter2Performance)
        {
            this.Winner = fighter1;
            this.Looser = fighter2;
        }

        else
        {
            this.Winner = fighter2;
            this.Looser = fighter1;
        }
    }

    private void GetRandomWinner()
    {
        if(random.Next(0, 101) > 50)
        {
            this.Winner = fighter1;
            this.Looser = fighter2;

            return;
        }
  
        this.Winner = fighter2;
        this.Looser = fighter1;
    }

    private void DetermineOutcome()
    {
        int difference = Math.Abs(Fighter1Performance - Fighter2Performance);

        bool isCrit = difference > 100;

        if(!isCrit)
        {
            Outcome = RoundOutcome.NoFinish;
            return;

        }
        
        HandleCrit();
        
    }

    private void HandleCrit()
    {
        int difference = Math.Abs(
            Fighter1Performance - Fighter2Performance
        );

        int finishChance = GetFinishChance(difference);

        if (random.Next(0, 100) >= finishChance)
        {
            Outcome = RoundOutcome.NoFinish;
            return;
        }

        IsFinish = true;
        Outcome = DetermineFinishType();
    }

    private int GetFinishChance(int difference)
    {
        if (difference >= 200)
            return 30;

        if (difference >= 150)
            return 15;

        if (difference >= 100)
            return 5;

        return 0;
    }

    private RoundOutcome DetermineFinishType()
    {
        switch (Style)
        {
            case FightingStyle.Standup:
                return RoundOutcome.Knockout;

            case FightingStyle.Grappling:
                return RoundOutcome.Submission;

            case FightingStyle.Wrestling:
                return random.Next(0, 100) < 50
                    ? RoundOutcome.TKO
                    : RoundOutcome.Submission;

            default:
                return RoundOutcome.Knockout;
        }
    }


}