using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

public class PlayerData
{
    public Stats Stats { get; private set; }
    public Dictionary<string, ClassData> ClassDict { get; private set; }
    public List<BonusBase> BonusList { get; private set; }

    private Random _random = new Random();

    public PlayerData(Stats stats, Dictionary<string, ClassData> classDict, List<BonusBase> bonusList)
    {
        Stats = new Stats(stats.Strength, stats.Dexterity, stats.Endurance);
        ClassDict = new Dictionary<string, ClassData>(classDict);
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
        return ClassDict.Values.Sum(value => value.Level);
    }

    public void IncreaseClassLevel(ClassData data)
    {
        if (!ClassDict.ContainsKey(data.Id))
            ClassDict.Add(data.Id, new ClassData(data));
        ClassDict[data.Id].LevelUp();
    }

    public void GetBonus(ClassData data)
    {
        Bonus bonus = Array.Find(data.Bonus, x => x.UnlockLevel == ClassDict[data.Id].Level);

        Debug.Log(data.Level);
        for (int i = 0; i < data.Bonus.Length; i++)
        {
            Debug.Log($"{data.Bonus[i].UnlockLevel} - {data.Bonus[i].ClassBonus}");
        }

        //Debug.Log($"{bonus} - {data.Bonus[0].}");

        if (bonus != null)
            BonusList.Add(bonus.ClassBonus);
    }
}
