using System;
using UnityEngine;

[Serializable]
public class ClassData
{
    [field: SerializeField] public string Id { get; private set; }
    [field: SerializeField] public int Health { get; private set; }
    [field: SerializeField] public WeaponSO Weapon { get; private set; }
    [field: SerializeField] public Bonus[] Bonus { get; private set; }
    public int Level { get; private set; }

    public ClassData(ClassData so)
    {
        Id = so.Id;
        Health = so.Health;
        Weapon = so.Weapon;
        Bonus = so.Bonus;
        Level = so.Level;
    }

    public void LevelUp() => Level++;
}

[Serializable]
public class Bonus
{
    [field: SerializeField] public int UnlockLevel { get; private set; }
    [field: SerializeField] public BonusBase ClassBonus { get; private set; }
}