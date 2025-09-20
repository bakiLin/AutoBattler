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

    // Bonus
    private List<BonusBase> _bonusList;

    public List<BonusBase> BonusList { get => _bonusList; }

    // Events
    public event Action OnUpdateStats, OnUpdateHealth;

    public event Action<ClassSO> OnUpdateClass;

    // Other
    [SerializeField] private ScriptableObjectHolder _holder;

    private Random _random = new Random();

    private PlayerDataCopy _playerDataCopy;

    private void OnEnable()
    {
        _health = 0;
        _stats = null;
        _weapon = null;
        _classDictionary = new Dictionary<string, ClassSO>();
        _bonusList = new List<BonusBase>();
        _playerDataCopy = new PlayerDataCopy(null, _classDictionary, _bonusList);
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
        _playerDataCopy.GetPlayerDataCopy(ref _stats, ref _classDictionary, ref _bonusList);

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
        if (_playerDataCopy.ClassDictionary.Count == 0) _weapon = characterClass.Weapon;

        if (_stats != null) OnUpdateStats?.Invoke();
        OnUpdateClass?.Invoke(characterClass);
    }

    public int ActivateBonus(BattleData battleData, BonusType requiredBonusType)
    {
        int damage = 0;
        foreach (var bonus in _bonusList)
            damage += bonus.BonusType == requiredBonusType ? bonus.Bonus(battleData) : 0;
        return damage;
    }

    private void CheckLevelToGetBonus(int classLevel, int[] level, int[] bonusId)
    {
        for (int i = 0; i < level.Length; i++)
        {
            if (classLevel == level[i]) 
                _bonusList.Add(Array.Find(_holder.AvailableBonus, bonus => bonus.Id == bonusId[i]));
        }
    }

    public bool IsReadyToBattle(int requiredLevel)
    {
        if (_stats != null && requiredLevel == CalculateLevel())
            return true;
        return false;
    }

    public int CalculateLevel()
    {
        int level = 0;
        foreach (var value in _classDictionary.Values)
            level += value.Level;
        return level;
    }

    public int GetActiveLevel() => _playerDataCopy.CalculateLevel();

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

    public void CopyPlayerData()
    {
        _playerDataCopy = new PlayerDataCopy(_stats, _classDictionary, _bonusList);
    }
}
