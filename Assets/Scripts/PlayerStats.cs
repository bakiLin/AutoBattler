public class PlayerStats
{
    public int Strength, Dexterity, Endurance;

    public PlayerStats(int strength, int dexterity, int endurance)
    {
        Strength = strength;
        Dexterity = dexterity;
        Endurance = endurance;
    }

    public bool IsEqual(PlayerStats stats)
    {
        if (stats == null) return false;
        if (stats.Strength == Strength && stats.Dexterity == Dexterity
            && stats.Endurance == Endurance) return true;
        return false;
    }
}
