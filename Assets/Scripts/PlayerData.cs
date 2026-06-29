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
        (int, int, int) stats;
        do stats = new(_random.Next(1, 4), _random.Next(1, 4), _random.Next(1, 4));
        while (stats.Item1 == Stats.Strength && stats.Item2 == Stats.Dexterity && stats.Item3 == Stats.Endurance);
        Stats = new Stats(stats.Item1, stats.Item2, stats.Item3);
    }

    public int CalculateLevel()
    {
        return ClassLevels.Values.Sum();
    }

    public void IncreaseClassLevel(ClassData data)
    {
        if (!ClassLevels.ContainsKey(data.Id))
            ClassLevels.Add(data.Id, 0);
        ClassLevels[data.Id]++;
    }

    public void GetBonus(ClassData data)
    {
        Bonus bonus = Array.Find(data.Bonus, x => x.UnlockLevel == ClassLevels[data.Id]);
        if (bonus != null)
            BonusList.Add(bonus.ClassBonus);
    }
}
