using FightEmpire.Core.Models;

namespace FightEmpire;

public class ActionSumary
{
    public Fighter? Winner;
    public Fighter? Looser;

    public Fighter Actor;
    public Fighter Defender;

    public ActionType Action;
    public ActionOutcome Outcome;

    public bool IsFinish;

    public bool Sucesfull;
    public int TimeTaken;


    public FightPosition Position;
}