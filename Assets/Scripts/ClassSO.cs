using System;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Class", fileName = "ClassSO")]
public class ClassSO : ScriptableObject
{
    [field: SerializeField] public string Id { get; private set; }
    [field: SerializeField] public int Health { get; private set; }
    [field: SerializeField] public WeaponSO Weapon { get; private set; }
    [field: SerializeField] public Bonus[] Bonus { get; private set; }
    [field: SerializeField] public StatBonus[] StatBonus { get; private set; }
}

[Serializable]
public class Bonus
{
    [field: SerializeField] public int UnlockLevel { get; private set; }
    [field: SerializeField] public BonusBase ClassBonus { get; private set; }
}

[Serializable]
public class StatBonus
{
    [field: SerializeField] public int UnlockLevel { get; private set; }
    [field: SerializeField] public Stats Stats { get; private set; }
}