using System;
using UnityEngine;
using Random = System.Random;

[CreateAssetMenu(menuName = "SO/Player", fileName = "New Player")]
public class PlayerSO : ScriptableObject
{
    private Random _random = new Random();
    // Health
    private int _health;

    public int Health { 
        get => _health; 
        set {
            if (value > 0) _health = value;
            else _health = 0;
        }
    }
    // Stats
    private PlayerStats _stats;

    public PlayerStats Stats { get => _stats; }

    // Class
    private ClassSO _class;

    public ClassSO Class {  get => _class; }

    // Weapon
    private WeaponSO _weapon;

    public WeaponSO Weapon { get => _weapon; }
    // Events
    public event Action<PlayerStats> OnGenerateStats;

    public event Action<ClassSO> OnSelectStartClass;

    public event Action OnReady;

    private void OnEnable()
    {
        _health = 0;
        _stats = null;
        _class = null;
        _weapon = null;
    }

    public void CreateNewPlayer()
    {
        PlayerStats stats; 
        do {
            stats = new PlayerStats(_random.Next(1, 4), _random.Next(1, 4), _random.Next(1, 4));
        } while (stats.IsEqual(_stats));
        _stats = stats;

        OnGenerateStats?.Invoke(_stats);
        CheckIfReady();
    }

    public void SetStartClass(ClassSO characterClass)
    {
        _class = characterClass;
        _health = characterClass.Health;
        _weapon = characterClass.Weapon;

        OnSelectStartClass?.Invoke(_class);
        CheckIfReady();
    }

    public void CheckIfReady()
    {
        if (_stats != null && _class != null)
            OnReady?.Invoke();
    }
}
