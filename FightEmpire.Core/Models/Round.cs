using FightEmpire.Core.Models;

namespace FightEmpire;
public class Round(Fighter fighter1, Fighter fighter2, int roundNumber)
{
    private Fighter? Winner = null;
    private Fighter? Looser = null;
    private RoundOutcome? Result = null;
    public bool hasBeenRun = false;
    public RoundSummary? Summary;
    private Fighter? _styleWinner;
    private bool IsFinish = false;
    Random random = new();

    public void Run()
    {
        
        FightingStyle roundStyle = this.DetermainFightingStyle();

        int fighther1Performance = GetPerformance(roundStyle, fighter1);
        int fighther2Performance = GetPerformance(roundStyle, fighter2);

        this.Result = this.DetermainRoundOutcome(roundStyle, fighther1Performance, fighther2Performance);
        
        this.DetermainRoundWinner(fighther1Performance, fighther2Performance);

        if(!IsFinish)
        {
            this.Winner!.WinRound();
            this.Looser!.LoseRound();
        }

        this.hasBeenRun = true;

        this.Summary = new()
        {
            RoundNumber = roundNumber,
            Winner = this.Winner!,
            Looser = this.Looser!,
            outcome = Result,
            style = roundStyle,
            isFinish = this.IsFinish,
            StyleWinner = this._styleWinner!
        };


    }

    private void DetermainRoundWinner(int fighther1Performance, int fighther2Performance)
    {
        if(fighther1Performance == fighther2Performance)
        {
            this.GetRandomWinner();
        }

        else if(fighther1Performance > fighther2Performance)
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

    private FightingStyle DetermainFightingStyle()
    {
        int fighter1StyleScore = fighter1.PreferedStat * random.Next(-50, 51);
        int fighter2StyleScore = fighter2.PreferedStat * random.Next(-50, 51);

        if(fighter1StyleScore == fighter2StyleScore)
        {
            if(random.Next(0, 101) > 50)
            {
                this._styleWinner = fighter1;
                return fighter1.PreferredStyle;

            }
            this._styleWinner = fighter2;
            return fighter2.PreferredStyle;
        }

        if(fighter1StyleScore > fighter2StyleScore)
        {
            this._styleWinner = fighter1;

            return fighter1.PreferredStyle;

        }

        this._styleWinner = fighter2;

        return fighter2.PreferredStyle;
    }

    private RoundOutcome DetermainRoundOutcome(FightingStyle fightingStyle, int fighther1Performance, int fighther2Performance)
    {
        if(!(Math.Abs(fighther1Performance - fighther2Performance) > 115))
            return RoundOutcome.NoFinish;
        
        this.IsFinish = true;
        switch (fightingStyle)
        {
            case FightingStyle.Standup:
                return RoundOutcome.Knockout;
            case FightingStyle.Grappling:
                return RoundOutcome.Submission;
            case FightingStyle.Wrestling:
                {
                    if(random.Next(0,101) > 50)
                        return RoundOutcome.TKO;
                    return RoundOutcome.Submission;
                }
            default:
                return RoundOutcome.Knockout;
        }



        
    }

    private int GetPerformance(FightingStyle fightingStyle, Fighter fighter)
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
}