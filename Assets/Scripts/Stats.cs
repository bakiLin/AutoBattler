using System;

[Serializable]
public class Stats
{
    public int Strength, Dexterity, Endurance;

    public Stats(int strength, int dexterity, int endurance)
    {
        Strength = strength;
        Dexterity = dexterity;
        Endurance = endurance;
    }

    public bool IsEqual(Stats stats)
    {
        if (stats == null) return false;
        if (stats.Strength == Strength && stats.Dexterity == Dexterity
            && stats.Endurance == Endurance) return true;
        return false;
    }
}
