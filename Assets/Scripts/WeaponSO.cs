using UnityEngine;

[CreateAssetMenu(menuName = "SO/Weapon", fileName = "New Weapon")]
public class WeaponSO : ScriptableObject
{
    [SerializeField]
    private WeaponType _type;

    [SerializeField]
    private int _damage;

    public WeaponType Type { get => _type; }

    public int Damage { get => _damage; }
}

public enum WeaponType
{
    Slashing,
    Bludgeoning,
    Piercing
}
