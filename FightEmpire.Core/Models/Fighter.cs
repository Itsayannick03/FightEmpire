namespace FightEmpire.Core.Models;

public class Fighter
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Nickname { get; set; } = string.Empty;
    public int Age { get; set; }

    public Nationality Nationality { get; set; }


    public int Striking { get; set; }
    public int Wrestling { get; set; }
    public int Grappling { get; set; }
    public int Cardio { get; set; }

    public List<FightSummary> FightHistory {get; private set;} = new();

    public int StatsTotal => Striking + Wrestling + Grappling;

    public FightingStyle PreferredStyle => getPreferedStyle();
    public int PreferedStat => getPreferedStyleStat(PreferredStyle);

    private int _submissionStaminaMax;
    public int SubmissionStamina { get; private set; }

    public int Wins { get; set; } = 0 ;
    public int Losses { get; set; } = 0; 
    public int Draws { get; set; } = 0; 


    public int Points { get; set; } = 0;

    public int RankingPoints { get; set; } = 1000;

    public int RoundScore = 0;


    public Fighter( string firstName, string lastName, string nickname, int age, Nationality nationality, int striking, int wrestling, int grappling, int cardio)
{
    FirstName = firstName;
    LastName = lastName;
    Nickname = nickname;
    Age = age;

    Nationality = nationality;

    Striking = striking;
    Wrestling = wrestling;
    Grappling = grappling;
    Cardio = cardio;

    _submissionStaminaMax = Grappling;

    SubmissionStamina = _submissionStaminaMax;
}


    

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

        foreach (FightSummary fight in FightHistory)
        {
            string result;
            Fighter opponent;

            if (fight.Result == FightResult.Draw)
            {
                result = "D";

                opponent = fight.Fighter1 == this
                    ? fight.Fighter2
                    : fight.Fighter1;
            }
            else
            {
                bool won = fight.Winner == this;

                result = won ? "W" : "L";

                opponent = won
                    ? fight.Loser!
                    : fight.Winner!;
            }

            string method;

            if (fight.IsFinish)
            {
                method =
                    $"{fight.FinishOutcome} " +
                    $"R{fight.FinishRound} " +
                    $"{fight.FinishMinute}:{fight.FinishSecond:00}";
            }
            else
            {
                method = fight.Result.ToString();
            }

            history +=
                $"{fightNumber}.\t" +
                $"{result}  " +
                $"{opponent.FirstName} {opponent.LastName}  " +
                $"{method}\n";

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

    public void WinRound()
    {
        this.Points += 10;
    }

    public void LoseRound()
    {
        this.Points += 9;
    }

    public void LoseRound10_8()
    {
        this.Points += 8;
    }

    public void LoseSubmission()
    {
        SubmissionStamina--;
    }
    
    public void ResetPoints()
    {
        this.Points = 0;
    }

    public void ResetScore()
    {
        this.RoundScore = 0;
    }

    public void RoundReset()
    {
        RoundScore = 0;

        // TODO: make less op
        SubmissionStamina = _submissionStaminaMax;

    }

    public void Reset()
    {
        ResetPoints();
        ResetScore();

        SubmissionStamina = _submissionStaminaMax;
    }
}