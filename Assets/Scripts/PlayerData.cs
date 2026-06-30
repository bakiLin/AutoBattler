using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class PlayerData
{
    public int Health { get; private set; }
    public Weapon Weapon { get; private set; }
    public Stats Stats { get; private set; }
    public Dictionary<string, int> Classes { get; private set; }
    public List<BonusBase> Bonuses { get; private set; }

    private Random _random = new Random();

    public PlayerData(int health, Weapon weapon, 
        Stats stats, Dictionary<string, int> classDict, List<BonusBase> bonusList)
    {
        Health = health;
        Weapon = weapon;
        Stats = new Stats(stats.Strength, stats.Dexterity, stats.Endurance);
        Classes = new Dictionary<string, int>(classDict);
        Bonuses = new List<BonusBase>(bonusList);
    }

    public PlayerData Clone()
    {
        return new PlayerData(Health, Weapon, Stats, Classes, Bonuses);
    }

    public void Restore(PlayerData snapshot)
    {
        Health = snapshot.Health;
        Weapon = snapshot.Weapon;

        if (!snapshot.Stats.IsZero())
        {
            Stats = new Stats(snapshot.Stats.Strength, snapshot.Stats.Dexterity, 
                snapshot.Stats.Endurance);
        }

        Classes.Clear();
        foreach (var pair in snapshot.Classes) Classes.Add(pair.Key, pair.Value);

        Bonuses.Clear();
        Bonuses.AddRange(snapshot.Bonuses);
    }

    public void SetHealth(int value)
    {
        Health = Mathf.Max(0, value);
    }

    public void EquipWeapon(Weapon weapon)
    {
        if (weapon != null) Weapon = weapon;
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
        if (!Classes.ContainsKey(id)) Classes.Add(id, 0);
        Classes[id]++;
    }

    public void ApplyBonus(ClassSO data)
    {
        if (!Classes.TryGetValue(data.Id, out int currentLevel)) return;

        var bonus = Array.Find(data.Bonus, x => x.UnlockLevel == currentLevel);
        if (bonus != null) Bonuses.Add(bonus.ClassBonus);

        var statBonus = Array.Find(data.StatBonus, x => x.UnlockLevel == currentLevel);
        if (statBonus != null) Stats += statBonus.Stats;
    }
}
