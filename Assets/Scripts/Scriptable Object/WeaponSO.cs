using System;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Weapon", fileName = "WeaponSO")]
public class WeaponSO : ScriptableObject
{
    [field: SerializeField] public Weapon Weapon { get; private set; }
}

[Serializable]
public class Weapon
{
    [field: SerializeField] public WeaponType Type { get; private set; }
    [field: SerializeField] public int Damage { get; private set; }
}

public enum WeaponType
{
    None,
    Slashing,
    Bludgeoning,
    Piercing
}
