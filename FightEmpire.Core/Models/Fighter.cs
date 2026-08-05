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

    public int Wins = 0;
    public int Losses = 0; 
    

    public override string ToString()
    {
        return $"""
        {FirstName} {Nickname} {LastName}

        Striking: {Striking}
        Wrestling: {Wrestling}
        Grappling: {Grappling}
        Cardio: {Cardio}
        """;
        
    }
}