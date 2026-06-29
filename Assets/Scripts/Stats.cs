using System;
using UnityEngine;

[Serializable]
public class Stats
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
}
