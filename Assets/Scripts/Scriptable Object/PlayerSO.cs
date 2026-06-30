using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Player", fileName = "New Player")]
public class PlayerSO : ScriptableObject, ICharacter
{
    public Stats Stats { get => _data.Stats; }

    public List<BonusBase> BonusList { get => _data.BonusList; }

    public Dictionary<string, int> ClassDictionary { get => _data.ClassLevels; }

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

    public event Action<string> OnUpdateClass;

    [SerializeField] private ScriptableObjectHolder _holder;

    private PlayerData _data, _dataCopy;

    private void OnEnable() 
    {
        ResetCharacter();
    }

    public void ResetCharacter()
    {
        _health = 0;
        _weapon = null;

        _data = new PlayerData(new Stats(0, 0, 0), new Dictionary<string, int>(), new List<BonusBase>());
        _dataCopy = new PlayerData(new Stats(0, 0, 0), _data.ClassLevels, _data.BonusList);
    }

    public void GenerateStats()
    {
        _data.GenerateStats();
        OnUpdateStats?.Invoke();
    }

    public void SelectClass(ClassSO characterClass)
    {
        var stats = _dataCopy.Stats.Strength != 0 ? _dataCopy.Stats : _data.Stats;

        _data = new PlayerData(stats, 
            _dataCopy.ClassLevels, _dataCopy.BonusList);

        if (ClassDictionary.Count == 0) _weapon = characterClass.Weapon;

        _data.IncreaseClassLevel(characterClass.Id);
        _data.GetBonus(characterClass);

        _health = GetHealth();

        OnUpdateStats?.Invoke();
        OnUpdateClass?.Invoke(characterClass.Id);
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
        if (requiredLevel == CalculateLevel())
            return true;
        return false;
    }

    public int CalculateLevel()
    {
        int level = 0;
        foreach (var value in ClassDictionary.Values)
            level += value;
        return level;
    }

    public int GetActiveLevel() => _dataCopy.ClassLevels.Values.Sum();

    public int GetHealth()
    {
        int health = 0;

        foreach (var pair in ClassDictionary)
        {
            health += CalculateClassHealth(_holder.Thief, pair);
            health += CalculateClassHealth(_holder.Warrior, pair);
            health += CalculateClassHealth(_holder.Barbarian, pair);
        }

        return health;
    }

    private int CalculateClassHealth(ClassSO data, KeyValuePair<string, int> pair)
    {
        return pair.Key == data.Id ? data.Health * pair.Value : 0;
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
