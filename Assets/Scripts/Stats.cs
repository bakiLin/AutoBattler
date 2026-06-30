using System;
using UnityEngine;

[Serializable]
public struct Stats
{
    [field: SerializeField] public int Strength { get; private set; }
    [field: SerializeField] public int Dexterity { get; private set; }
    [field: SerializeField] public int Endurance { get; private set; }

    public Stats(int strength, int dexterity, int endurance)
    {
        Strength = strength;
        Dexterity = dexterity;
        Endurance = endurance;
    }

    public static Stats operator +(Stats a, Stats b)
    {
        return new Stats(a.Strength + b.Strength, a.Dexterity + b.Dexterity, a.Endurance + b.Endurance);
    }
}
