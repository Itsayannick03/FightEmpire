namespace FightEmpire;

public static class ActionRules
{
    public static List<(ActionType Action, int Weight)> GetActionWeights(FightPosition position, FightingStyle preferredStyle)
    {
        return (position, preferredStyle) switch
        {
            // STANDING
            (FightPosition.Standing, FightingStyle.Standup) => new()
            {
                (ActionType.Strike, 50),
                (ActionType.Kick, 40),
                (ActionType.TakedownAttempt, 10),
            },

            (FightPosition.Standing, FightingStyle.Wrestling) => new()
            {
                (ActionType.Strike, 25),
                (ActionType.Kick, 10),
                (ActionType.TakedownAttempt, 65),
            },

            (FightPosition.Standing, FightingStyle.Grappling) => new()
            {
                (ActionType.Strike, 20),
                (ActionType.Kick, 15),
                (ActionType.TakedownAttempt, 65),
            },


            // // CLINCH
            // (FightPosition.Clinch, FightingStyle.Striking) => new()
            // {
            //     (ActionType.Punch, 40),
            //     (ActionType.Takedown, 20),
            //     (ActionType.ClinchAttempt, 40)
            // },

            // (FightPosition.Clinch, FightingStyle.Wrestling) => new()
            // {
            //     (ActionType.Punch, 15),
            //     (ActionType.Takedown, 70),
            //     (ActionType.ClinchAttempt, 15)
            // },

            // (FightPosition.Clinch, FightingStyle.Grappling) => new()
            // {
            //     (ActionType.Punch, 10),
            //     (ActionType.Takedown, 75),
            //     (ActionType.ClinchAttempt, 15)
            // },


            // GROUND
            (FightPosition.Grounded, FightingStyle.Standup) => new()
            {
                (ActionType.GroundStrike, 60),
                (ActionType.SubmissionAttempt, 10),
                (ActionType.GetUpAttempt, 30)
            },

            (FightPosition.Grounded, FightingStyle.Wrestling) => new()
            {
                (ActionType.GroundStrike, 50),
                (ActionType.SubmissionAttempt, 20),
                (ActionType.GetUpAttempt, 30)
            },

            (FightPosition.Grounded, FightingStyle.Grappling) => new()
            {
                (ActionType.GroundStrike, 20),
                (ActionType.SubmissionAttempt, 65),
                (ActionType.GetUpAttempt, 15)
            },

            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public static T WeightedRandom<T>(List<(T Item, int Weight)> items)
    {
        int totalWeight = items.Sum(x => x.Weight);

        int roll = Random.Shared.Next(totalWeight);

        int currentWeight = 0;

        foreach (var item in items)
        {
            currentWeight += item.Weight;

            if (roll < currentWeight)
            {
                return item.Item;
            }
        }

        throw new InvalidOperationException("Could not select weighted item.");
    }

}