using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

[CreateAssetMenu(menuName = "SO/Player", fileName = "New Player")]
public class PlayerSO : ScriptableObject
{
    // Health
    private int _health;

    public int Health { 
        get => _health; 
        set {
            if (value > 0) _health = value;
            else _health = 0;
            OnUpdateHealth?.Invoke();
        }
    }

    // Stats
    private Stats _stats;

    public Stats Stats { get => _stats; }

    // Class
    private Dictionary<string, ClassSO> _classDictionary;

    public Dictionary<string, ClassSO> ClassDictionary {  get => _classDictionary; }

    // Weapon
    private WeaponSO _weapon;

    public WeaponSO Weapon {
        get => _weapon; 
        set { 
            _weapon = value; 
            OnUpdateClass?.Invoke(null);
        }
    }

    // Events
    public event Action OnUpdateStats, OnUpdateHealth;

    public event Action<ClassSO> OnUpdateClass;

    // Other
    private Random _random = new Random();

    private Dictionary<string, ClassSO> _dictionaryCopy;

    private void OnEnable()
    {
        _health = 0;
        _stats = null;
        _classDictionary = new Dictionary<string, ClassSO>();
        _dictionaryCopy = new Dictionary<string, ClassSO>();
        _weapon = null;
    }

    public void GenerateStats()
    {
        Stats stats; 
        do stats = new Stats(_random.Next(1, 4), _random.Next(1, 4), _random.Next(1, 4));
        while (stats.IsEqual(_stats));
        _stats = stats;

        OnUpdateStats?.Invoke();
    }

    public void SelectClass(ClassSO characterClass)
    {
        _classDictionary.Clear();

        foreach (var pair in _dictionaryCopy)
        {
            _classDictionary.Add(pair.Key, pair.Value);
            _classDictionary[pair.Key] = new ClassSO(pair.Value);
        }

        if (!_classDictionary.ContainsKey(characterClass.name))
            _classDictionary.Add(characterClass.name, new ClassSO(characterClass));
        _classDictionary[characterClass.name].Level += 1;

        _health = GetHealth();
        if (_dictionaryCopy.Count == 0) _weapon = characterClass.Weapon;

        OnUpdateClass?.Invoke(characterClass);
    }

    public bool IsReadyToBattle(int requiredLevel)
    {
        if (_stats != null && requiredLevel == CalculateLevel())
            return true;
        return false;
    }

    private int CalculateLevel()
    {
        int level = 0;
        foreach (var value in _classDictionary.Values)
            level += value.Level;
        return level;
    }

    public int GetHealth()
    {
        int health = 0;
        foreach (var value in _classDictionary.Values)
            if (value.Level > 0) health += value.Health * value.Level;
        return health;
    }

    public void RestoreHealth()
    {
        _health = GetHealth();
        OnUpdateClass?.Invoke(null);
    }

    public void CopyDictionary()
    {
        _dictionaryCopy.Clear();

        foreach (var pair in _classDictionary)
        {
            _dictionaryCopy.Add(pair.Key, pair.Value);
            _dictionaryCopy[pair.Key] = new ClassSO(pair.Value);
        }
    }
}
