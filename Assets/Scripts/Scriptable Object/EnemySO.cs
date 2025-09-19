using UnityEngine;

[CreateAssetMenu(menuName = "SO/Enemy", fileName = "New Enemy")]
public class EnemySO : ScriptableObject
{
    // Health
    [SerializeField] private int _health; 

    public int Health { 
        get => _health; 
        set {
            if (value > 0) _health = value;
            else _health = 0;
        }
    }

    // Damage
    [SerializeField] private int _damage;

    public int Damage { get => _damage; }
    
    // Stats
    [SerializeField] private Stats _stats;

    public Stats Stats { get => _stats; }

    // Reward
    [SerializeField] private WeaponSO _reward;

    public WeaponSO Reward { get => _reward; }

    // Bonus
    [SerializeField] private BonusBase _bonus;

    public BonusBase Bonus { get => _bonus; }

    public EnemySO(EnemySO enemy)
    {
        name = enemy.name;
        _health = enemy.Health;
        _damage = enemy.Damage;
        _stats = new Stats(enemy.Stats.Strength, enemy.Stats.Dexterity, enemy.Stats.Endurance);
        _reward = enemy.Reward;
        _bonus = enemy.Bonus;
    }
}
