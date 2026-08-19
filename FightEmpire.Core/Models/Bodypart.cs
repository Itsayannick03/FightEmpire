namespace FightEmpire;

public class Bodypart
{
    BodypartType BodypartType {get;}
    int MaxHitpoints {get;}
    public int Hitpoints {get; private set;}

    public bool IsDamaged => Hitpoints < MaxHitpoints;
    public bool IsCritical => Hitpoints < MaxHitpoints * 0.25;

    public Bodypart(BodypartType bodypartType)
    {
        BodypartType = bodypartType;
        MaxHitpoints = 100;
    }

    public void TakeDamage(int damage)
    {
        Hitpoints -= damage;
    }

    public void Heal()
    {
        Hitpoints = MaxHitpoints;
    }

}