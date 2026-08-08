using FightEmpire.Core.Models;

public class RoundSummary
{
    public int RoundNumber {get; set;}
    public Fighter Winner {get; set;}
    public Fighter Looser {get; set;}

    public Fighter StyleWinner {get; set;}
    public RoundOutcome? outcome {get; set;}
    public FightingStyle style {get; set;}
    public bool isFinish;
}