using FightEmpire.Core.Models;

namespace FightEmpire;

public class ExchangeSumary
{
    public FightPosition CurrentPosition;

    public List<ActionSumary> Actions = null!;

    public Fighter? Winner;
    public Fighter? Looser;

    public bool IsFinish;

    public ActionOutcome? FinishOutcome;

    public int TimeTaken;
}