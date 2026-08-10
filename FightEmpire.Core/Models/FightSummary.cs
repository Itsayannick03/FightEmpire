using FightEmpire.Core.Models;

namespace FightEmpire;

public class FightSummary
{
    public Fighter Winner {get; set;} = null!;
    public Fighter Looser {get; set;} = null!;

    public RoundOutcome? Result {get; set;}

    public int FinishRound;

}