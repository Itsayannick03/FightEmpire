using FightEmpire.Core.Models;

namespace FightEmpire;

public class CombatAction(Fighter fighter1, Fighter fighter2, FightPosition startingPosition)
{
    public Fighter Actor { get; private set; } = null!;
    public Fighter Defender { get; private set; } = null!;

    public ActionType Action { get; private set; }

    public Fighter? Winner { get; private set; }
    public Fighter? Looser { get; private set; }


    public ActionOutcome Outcome { get; private set; }

    public bool Sucesfull { get; private set; }

    public bool IsFinish { get; private set; } = false;

    public int ActorPerformance { get; private set; }
    public int DefenderPerformance { get; private set; }

    public int TimeTaken;

    public FightPosition CurentPosition { get; private set; } = startingPosition;

    public ActionSumary Sumary = null!;

    public void Run()
    {
        DetermainInitiative();
        DetermainAction();
        DetermainSucess();
        DetermineOutcome();
        DetermineTimeTaken();

        Sumary = new()
        {
            Winner = this.Winner,
            Looser = this.Looser,
            Actor = this.Actor,
            Defender = this.Defender,
            Action = this.Action,
            Outcome = this.Outcome,
            IsFinish = this.IsFinish,
            Sucesfull = this.Sucesfull,
            Position = this.CurentPosition,
            TimeTaken = this.TimeTaken
        };
    }

    private void DetermainInitiative()
    {
        int fighter1Initiative = fighter1.Cardio + Random.Shared.Next(0, 51);
        int fighter2Initiative = fighter2.Cardio + Random.Shared.Next(0, 51);

        if(fighter1Initiative == fighter2Initiative)
        {
            GetRandomInitiative();
            return;
        }

        if(fighter1Initiative > fighter2Initiative)
        {
            Actor = fighter1;
            Defender = fighter2;
            return;
        }

        Actor = fighter2;
        Defender = fighter1;
    }

    private void GetRandomInitiative()
    {
        if(Random.Shared.Next(2) == 0)
        {
            Actor = fighter1;
            Defender = fighter2;
            return;
        }

        Actor = fighter2;
        Defender = fighter1;
    }

    private void DetermainAction()
    {
        var actions = ActionRules.GetActionWeights(CurentPosition, Actor.PreferredStyle);

        Action = ActionRules.WeightedRandom(actions);
    }

    private void DetermainSucess()
    {
        GetPerformance();

        if(ActorPerformance == DefenderPerformance)
        {
            GetRandomSucess();
            return;
        }

        if(ActorPerformance > DefenderPerformance)
        {
            Sucesfull = true;
            return;
        }

        Sucesfull = false;

    }

    private void GetPerformance()
    {
        int actorRelevantStat;
        int defenderRelevantStat;

        switch (Action)
        {
  
            case ActionType.Strike:
            {
                actorRelevantStat = Actor.Striking;
                defenderRelevantStat = Defender.Striking;
                break;
            }

            case ActionType.Kick:
            {
                actorRelevantStat = Actor.Striking;
                defenderRelevantStat = Defender.Striking;
                break;
            }

            case ActionType.TakedownAttempt:
            {   
                actorRelevantStat = Math.Max(Actor.Wrestling, Actor.Grappling);
                defenderRelevantStat = Defender.Wrestling;
                break;
            }

            case ActionType.GetUpAttempt:
            {
                actorRelevantStat = (Actor.Striking + Actor.Wrestling * 2) / 3;
                defenderRelevantStat = Math.Max(Defender.Wrestling, Defender.Grappling);
                break;
            }

            case ActionType.GroundStrike:
            {
                actorRelevantStat = (Actor.Striking + Actor.Wrestling * 2) / 3;
                defenderRelevantStat = Math.Max(Defender.Wrestling, Defender.Grappling);
                break;
            }

            case ActionType.SubmissionAttempt:
            {
                actorRelevantStat = Actor.Grappling;
                defenderRelevantStat = Defender.Grappling;
                break;
            }
            default:
                throw new ArgumentOutOfRangeException(nameof(Type));
        }

        ActorPerformance = actorRelevantStat + Random.Shared.Next(0,101);
        DefenderPerformance = defenderRelevantStat + Random.Shared.Next(0,101);
    }

        private void GetRandomSucess()
    {
        if(Random.Shared.Next(2) == 0)
        {
            Sucesfull = true;

            Winner = Actor;
            Looser = Defender;

            return;
        }

        Sucesfull = false;

        Winner = Defender;
        Looser = Actor;
    }


    private void DetermineOutcome()
    {
        if(!Sucesfull)
        {
            HandleDefense();
            return;
        }
        
        int difference = ActorPerformance - DefenderPerformance;

  

        if(HandleCrit(difference))
            return;
        

        HandleSucess();
        
        
    }

    private void HandleSucess()
    {
        Winner = Actor;
        Looser = Defender;

        switch (Action)
        {
            case ActionType.Strike:
            {
                Outcome = ActionOutcome.StrikeLanded;
                return;
            }

            case ActionType.Kick:
            {
                Outcome = ActionOutcome.KickLanded;
                return;
            }

            case ActionType.TakedownAttempt:
            {
                CurentPosition = FightPosition.Grounded;

                Outcome = ActionOutcome.Takedown;

                return;
            }

            case ActionType.GetUpAttempt:
            {
                CurentPosition = FightPosition.Standing;

                Outcome = ActionOutcome.GetUp;

                return;
            }

            case ActionType.GroundStrike:
            {
                Outcome = ActionOutcome.GroundStrikeLanded;
                return;
            }

            case ActionType.SubmissionAttempt:
            {
                Looser.LoseSubmission();

                if(Looser.SubmissionStamina <= 0)
                {
                    IsFinish = true;
                    Outcome = ActionOutcome.Submission;
                    return;
                }

                Outcome = ActionOutcome.SubmissionProgress;
                return;
            }

            default:
                throw new ArgumentOutOfRangeException(nameof(Type));
        }
    }

    private void HandleDefense()
    {
        Winner = Defender;
        Looser = Actor;

        switch (Action)
        {
            case ActionType.Strike:
            {
                Outcome = ActionOutcome.StrikeBlocked;
                return;
            }

            case ActionType.Kick:
            {
                Outcome = ActionOutcome.KickBlocked;
                return;
            }

            case ActionType.GroundStrike:
            {
                Outcome = ActionOutcome.GroundStrikeBlocked;
                return;
            }

            case ActionType.TakedownAttempt:
            {
                Outcome = ActionOutcome.TakedownBlocked;
                return;
            }

            case ActionType.GetUpAttempt:
            {
                Outcome = ActionOutcome.GetUpDenied;
                return;
            }

            case ActionType.SubmissionAttempt:
            {
                Outcome = ActionOutcome.SubmissionDefense;
                return;
            }
            
            default:
                throw new ArgumentOutOfRangeException(nameof(Type));

        }
    }

    private bool HandleCrit(int difference)
    {
        if (Action != ActionType.Strike &&
            Action != ActionType.Kick &&
            Action != ActionType.GroundStrike)
        {
            return false;
        }

        int finishChance = GetFinishChance(difference);

        if (Random.Shared.Next(0, 100) >= finishChance)
            return false;

        Winner = Actor;
        Looser = Defender;

        IsFinish = true;

        Outcome = DetermineCritFinishType();

        return true;
    }

    private ActionOutcome DetermineCritFinishType()
    {
        switch (Action)
        {
            case ActionType.Strike:
            case ActionType.Kick:
            {
                return ActionOutcome.Knockout;
            }
            case ActionType.GroundStrike:
            {
                return ActionOutcome.TKO;
            }
            
            default:
                throw new InvalidOperationException(
                $"{Action} cannot result in a critical finish."
            );
        }
    }

    private int GetFinishChance(int difference)
    {
        if (difference >= 100)
            return 5;

        if (difference >= 50)
            return 2;

        return 0;
    }

    private void DetermineTimeTaken()
    {
        TimeTaken = Outcome switch
        {
            ActionOutcome.StrikeLanded => Random.Shared.Next(2, 6),
            ActionOutcome.StrikeBlocked => Random.Shared.Next(2, 5),

            ActionOutcome.KickLanded => Random.Shared.Next(3, 7),
            ActionOutcome.KickBlocked => Random.Shared.Next(3, 6),

            ActionOutcome.Takedown => Random.Shared.Next(5, 13),
            ActionOutcome.TakedownBlocked => Random.Shared.Next(4, 10),

            ActionOutcome.GetUp => Random.Shared.Next(5, 11),
            ActionOutcome.GetUpDenied => Random.Shared.Next(4, 9),

            ActionOutcome.GroundStrikeLanded => Random.Shared.Next(2, 5),
            ActionOutcome.GroundStrikeBlocked => Random.Shared.Next(2, 5),

            ActionOutcome.SubmissionProgress => Random.Shared.Next(8, 21),
            ActionOutcome.SubmissionDefense => Random.Shared.Next(6, 16),
            ActionOutcome.Submission => Random.Shared.Next(5, 16),

            ActionOutcome.Knockout => Random.Shared.Next(2, 7),

            _ => Random.Shared.Next(2, 6)
        };
    }
}

