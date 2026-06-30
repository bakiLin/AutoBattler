using System;
using System.Collections.Generic;
using System.Linq;
using Random = System.Random;

public class PlayerData
{
    public Stats Stats { get; private set; }
    public Dictionary<string, int> ClassLevels { get; private set; }
    public List<BonusBase> BonusList { get; private set; }

    private Random _random = new Random();

    public PlayerData(Stats stats, Dictionary<string, int> classDict, List<BonusBase> bonusList)
    {
        Stats = new Stats(stats.Strength, stats.Dexterity, stats.Endurance);
        ClassLevels = new Dictionary<string, int>(classDict);
        BonusList = new List<BonusBase>(bonusList);
    }

    public void GenerateStats()
    {
        Stats newStats;
        do newStats = new(_random.Next(1, 4), _random.Next(1, 4), _random.Next(1, 4));
        while (Stats.Equals(newStats));
        Stats = newStats;
    }

    public void IncreaseClassLevel(string id)
    {
        if (!ClassLevels.ContainsKey(id)) ClassLevels.Add(id, 0);
        ClassLevels[id]++;
    }

    public void GetBonus(ClassSO data)
    {
        if (!ClassLevels.TryGetValue(data.Id, out int currentLevel)) return;

        var bonus = Array.Find(data.Bonus, x => x.UnlockLevel == currentLevel);
        if (bonus != null) BonusList.Add(bonus.ClassBonus);

        var statBonus = Array.Find(data.StatBonus, x => x.UnlockLevel == currentLevel);
        if (statBonus != null) Stats += statBonus.Stats;
    }
}
