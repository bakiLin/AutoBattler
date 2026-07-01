using System;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Weapon", fileName = "WeaponSO")]
public class WeaponSO : ScriptableObject
{
    [field: SerializeField] public Weapon Weapon { get; private set; }
}

[Serializable]
public struct Weapon
{
    [field: SerializeField] public string Id { get; private set; }
    [field: SerializeField] public WeaponType Type { get; private set; }
    [field: SerializeField] public int Damage { get; private set; }

    public Weapon(int damage)
    {
        Id = string.Empty;
        Type = WeaponType.None;
        Damage = damage;
    }
}

public enum WeaponType
{
    None,
    Slashing,
    Bludgeoning,
    Piercing
}
