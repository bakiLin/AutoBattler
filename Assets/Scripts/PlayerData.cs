using System;
using System.Collections.Generic;
using Random = System.Random;

public class PlayerData
{
    // Stats
    private Stats _stats;

    public Stats Stats { get => _stats; }

    // Class
    private Dictionary<string, ClassSO> _classDictionary;

    public Dictionary<string, ClassSO> ClassDictionary { get => _classDictionary; }

    // Bonus
    private List<BonusBase> _bonusList;

    public List<BonusBase> BonusList { get => _bonusList; }

    private Random _random = new Random();

    public PlayerData(Stats stats, Dictionary<string, ClassSO> classDictionary, List<BonusBase> bonusList)
    {
        if (stats != null)
            _stats = new Stats(stats.Strength, stats.Dexterity, stats.Endurance);

        _classDictionary = new Dictionary<string, ClassSO>();
        foreach (var pair in classDictionary)
            _classDictionary.Add(pair.Key, new ClassSO(pair.Value));

        _bonusList = new List<BonusBase>();
        foreach (var item in bonusList) _bonusList.Add(item);
    }

    public void GetDataCopy(ref PlayerData data)
    {
        if (_stats != null)
        {
            data._stats = null;
            data._stats = new Stats(_stats.Strength, _stats.Dexterity, _stats.Endurance);
        }

        data._classDictionary.Clear();
        foreach (var pair in _classDictionary)
            data._classDictionary.Add(pair.Key, new ClassSO(pair.Value));

        data._bonusList.Clear();
        foreach (var item in _bonusList)
            data._bonusList.Add(item);
    }

    public void GenerateStats()
    {
        Stats stats;
        do stats = new Stats(_random.Next(1, 4), _random.Next(1, 4), _random.Next(1, 4));
        while (stats.IsEqual(_stats));
        _stats = stats;
    }

    public int CalculateLevel()
    {
        int level = 0;
        foreach (var value in _classDictionary.Values)
            level += value.Level;
        return level;
    }

    public void IncreaseClassLevel(ClassSO characterClass)
    {
        if (!_classDictionary.ContainsKey(characterClass.name))
            _classDictionary.Add(characterClass.name, new ClassSO(characterClass));
        _classDictionary[characterClass.name].Level += 1;
    }

    public void GetBonus(ClassSO characterClass, ScriptableObjectHolder holder)
    {
        switch (characterClass.Id)
        {
            case 0:
                CheckLevelToGetBonus(_classDictionary[characterClass.name].Level, new int[] { 1, 3 }, new int[] { 0, 5 }, holder);
                if (_classDictionary[characterClass.name].Level == 2) _stats.Dexterity += 1;
                break;
            case 1:
                CheckLevelToGetBonus(_classDictionary[characterClass.name].Level, new int[] { 1, 2 }, new int[] { 1, 3 }, holder);
                if (_classDictionary[characterClass.name].Level == 3) _stats.Strength += 1;
                break;
            case 2:
                CheckLevelToGetBonus(_classDictionary[characterClass.name].Level, new int[] { 1, 2 }, new int[] { 2, 4 }, holder);
                if (_classDictionary[characterClass.name].Level == 3) _stats.Endurance += 1;
                break;
        }
    }

    private void CheckLevelToGetBonus(int classLevel, int[] level, int[] bonusId, ScriptableObjectHolder holder)
    {
        for (int i = 0; i < level.Length; i++)
        {
            if (classLevel == level[i])
                _bonusList.Add(Array.Find(holder.AvailableBonus, bonus => bonus.Id == bonusId[i]));
        }
    }
}
