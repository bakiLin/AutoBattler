using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

[CreateAssetMenu(menuName = "SO/Player", fileName = "New Player")]
public class PlayerSO : ScriptableObject
{
    public Stats Stats { get => _data.Stats; }

    public List<BonusBase> BonusList { get => _data.BonusList; }

    public Dictionary<string, ClassSO> ClassDictionary { get => _data.ClassDictionary; }

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
    [SerializeField] private ScriptableObjectHolder _holder;

    //private Random _random = new Random();

    private PlayerData _data, _dataCopy;

    private void OnEnable()
    {
        _health = 0;
        _weapon = null;

        _data = new PlayerData(null, new Dictionary<string, ClassSO>(), new List<BonusBase>());
        _dataCopy = new PlayerData(null, _data.ClassDictionary, _data.BonusList);
    }

    public void GenerateStats()
    {
        _data.GenerateStats();
        OnUpdateStats?.Invoke();
    }

    public void SelectClass(ClassSO characterClass)
    {
        _dataCopy.GetDataCopy(ref _data);
        _data.IncreaseClassLevel(characterClass);
        _data.GetBonus(characterClass, _holder);

        _health = GetHealth();
        if (ClassDictionary.Count == 1) _weapon = characterClass.Weapon;

        if (Stats != null) OnUpdateStats?.Invoke();
        OnUpdateClass?.Invoke(characterClass);
    }

    public int ActivateBonus(BattleData battleData, BonusType requiredBonusType)
    {
        int damage = 0;
        foreach (var bonus in BonusList)
            damage += bonus.BonusType == requiredBonusType ? bonus.Bonus(battleData) : 0;
        return damage;
    }

    public bool IsReadyToBattle(int requiredLevel)
    {
        if (Stats != null && requiredLevel == CalculateLevel())
            return true;
        return false;
    }

    public int CalculateLevel()
    {
        int level = 0;
        foreach (var value in ClassDictionary.Values)
            level += value.Level;
        return level;
    }

    public int GetActiveLevel() => _dataCopy.CalculateLevel();

    public int GetHealth()
    {
        int health = 0;
        foreach (var value in ClassDictionary.Values)
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
        _dataCopy = new PlayerData(Stats, ClassDictionary, BonusList);
    }
}
