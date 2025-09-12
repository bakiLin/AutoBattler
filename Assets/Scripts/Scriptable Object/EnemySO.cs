using UnityEngine;

[CreateAssetMenu(menuName = "SO/Enemy", fileName = "New Enemy")]
public class EnemySO : ScriptableObject
{
    [SerializeField]
    private WeaponSO _reward;

    [SerializeField]
    private int _health, _damage, _strength, _dexterity, _endurance;

    public int Health { 
        get => _health; 
        set {
            if (value > 0) _health = value;
            else _health = 0;
        }
    }

    public int Damage { get => _damage; }

    public int Strength { get => _strength; }

    public int Dexterity { get => _dexterity; }

    public int Endurance { get => _endurance; }

    public EnemySO(EnemySO enemy)
    {
        name = enemy.name;
        _health = enemy.Health;
        _damage = enemy.Damage;
        _strength = enemy.Strength;
        _dexterity = enemy.Dexterity;
        _endurance = enemy.Endurance;
    }
}
