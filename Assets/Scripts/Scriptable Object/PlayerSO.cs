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
    private Stats _stats, _statsCopy;

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

    // Bonus
    [SerializeField] private BonusBase[] _availableBonus;

    private List<BonusBase> _bonusList, _bonusCopy;

    public List<BonusBase> BonusList { get => _bonusList; }

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
        _statsCopy = null;
        _weapon = null;
        _classDictionary = new Dictionary<string, ClassSO>();
        _dictionaryCopy = new Dictionary<string, ClassSO>();
        _bonusList = new List<BonusBase>();
        _bonusCopy = new List<BonusBase>();
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

        _bonusList.Clear();
        foreach (var item in _bonusCopy)
            _bonusList.Add(item);

        if (_statsCopy != null)
        {
            _stats = null;
            _stats = new Stats(_statsCopy.Strength, _statsCopy.Dexterity, _statsCopy.Endurance);
        }

        if (!_classDictionary.ContainsKey(characterClass.name))
            _classDictionary.Add(characterClass.name, new ClassSO(characterClass));
        _classDictionary[characterClass.name].Level += 1;

        switch (characterClass.Id)
        {
            case 0:
                CheckLevelToGetBonus(_classDictionary[characterClass.name].Level, new int[] { 1, 3 }, new int[] { 0, 5 });
                if (_classDictionary[characterClass.name].Level == 2) _stats.Dexterity += 1; 
                break;
            case 1:
                CheckLevelToGetBonus(_classDictionary[characterClass.name].Level, new int[] { 1, 2 }, new int[] { 1, 3 });
                if (_classDictionary[characterClass.name].Level == 3) _stats.Strength += 1;
                break;
            case 2:
                CheckLevelToGetBonus(_classDictionary[characterClass.name].Level, new int[] { 1, 2 }, new int[] { 2, 4 });
                if (_classDictionary[characterClass.name].Level == 3) _stats.Endurance += 1;
                break;
        }

        _health = GetHealth();
        if (_dictionaryCopy.Count == 0) _weapon = characterClass.Weapon;

        if (_stats != null) OnUpdateStats?.Invoke();
        OnUpdateClass?.Invoke(characterClass);
    }

    public int AttackBonus(BattleData battleData)
    {
        int damage = 0;
        foreach (var bonus in _bonusList)
        {
            if (bonus.BonusType == BonusType.Attack)
                damage += bonus.Bonus(battleData);
        }
        return damage;
    }

    public int DefenceBonus(BattleData battleData)
    {
        int damage = 0;
        foreach (var bonus in _bonusList)
        {
            if (bonus.BonusType == BonusType.Defence)
                damage += bonus.Bonus(battleData);
        }
        return damage;
    }

    private void CheckLevelToGetBonus(int classLevel, int[] level, int[] bonusId)
    {
        for (int i = 0; i < level.Length; i++)
            if (classLevel == level[i]) _bonusList.Add(Array.Find(_availableBonus, bonus => bonus.Id == bonusId[i]));
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

        _bonusCopy.Clear();
        foreach (var item in _bonusList)
            _bonusCopy.Add(item);

        _statsCopy = null;
        _statsCopy = new Stats(_stats.Strength, _stats.Dexterity, _stats.Endurance);
    }
}
