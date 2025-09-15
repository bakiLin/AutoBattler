using System;
using System.Collections.Generic;
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
    private Dictionary<string, ClassSO> _classDictionary;

    public Dictionary<string, ClassSO> ClassDictionary {  get => _classDictionary; }
    // Weapon
    private WeaponSO _weapon;

    public WeaponSO Weapon { get => _weapon; }
    // Events
    public event Action OnGenerateStats, OnReady;

    public event Action<ClassSO> OnSelectStartClass, OnChangeLevelUpUI;
    // Variable
    private Dictionary<string, ClassSO> _dictionaryCopy;

    private void OnEnable()
    {
        _health = 0;
        _stats = null;
        _classDictionary = new Dictionary<string, ClassSO>();
        _dictionaryCopy = new Dictionary<string, ClassSO>();
        _weapon = null;
    }

    public void CreateNewPlayer()
    {
        PlayerStats stats; 
        do {
            stats = new PlayerStats(_random.Next(1, 4), _random.Next(1, 4), _random.Next(1, 4));
        } while (stats.IsEqual(_stats));
        _stats = stats;

        OnGenerateStats?.Invoke();
        CheckIfReady();
    }

    public void SetStartClass(ClassSO characterClass)
    {
        foreach (var value in _classDictionary.Values) value.Level = 0;

        if (!_classDictionary.ContainsKey(characterClass.name))
            _classDictionary.Add(characterClass.name, new ClassSO(characterClass));
        _classDictionary[characterClass.name].Level = 1;

        _health = characterClass.Health;
        _weapon = characterClass.Weapon;

        OnSelectStartClass?.Invoke(characterClass);
        CheckIfReady();
    }

    public void CheckIfReady()
    {
        if (_stats != null && _classDictionary != null)
            OnReady?.Invoke();
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
        OnChangeLevelUpUI?.Invoke(null);
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

    public void AddClass(ClassSO characterClass)
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
        OnChangeLevelUpUI?.Invoke(characterClass);
    }
}
