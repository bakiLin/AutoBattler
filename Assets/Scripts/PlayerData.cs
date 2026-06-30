using System;
using System.Collections.Generic;
using Random = System.Random;

public class PlayerData
{
    public Stats Stats { get; private set; }
    public Dictionary<string, int> Class { get; private set; }
    public List<BonusBase> Bonus { get; private set; }

    private Random _random = new Random();

    public PlayerData(Stats stats, Dictionary<string, int> classDict, List<BonusBase> bonusList)
    {
        Stats = new Stats(stats.Strength, stats.Dexterity, stats.Endurance);
        Class = new Dictionary<string, int>(classDict);
        Bonus = new List<BonusBase>(bonusList);
    }

    public void GenerateStats()
    {
        Stats newStats;
        do newStats = new(_random.Next(1, 4), _random.Next(1, 4), _random.Next(1, 4));
        while (Stats.Equals(newStats));
        Stats = newStats;
    }

    public void LevelUp(string id)
    {
        if (!Class.ContainsKey(id)) Class.Add(id, 0);
        Class[id]++;
    }

    public void GetBonus(ClassSO data)
    {
        if (!Class.TryGetValue(data.Id, out int currentLevel)) return;

        var bonus = Array.Find(data.Bonus, x => x.UnlockLevel == currentLevel);
        if (bonus != null) Bonus.Add(bonus.ClassBonus);

        var statBonus = Array.Find(data.StatBonus, x => x.UnlockLevel == currentLevel);
        if (statBonus != null) Stats += statBonus.Stats;
    }
}
