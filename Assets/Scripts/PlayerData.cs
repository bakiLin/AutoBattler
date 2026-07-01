using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

public struct PlayerData : ICharacter
{
    public int Health { get; private set; }
    public Weapon Weapon { get; private set; }
    public Stats Stats { get; private set; }
    public Dictionary<string, int> Classes { get; private set; }
    public List<BonusBase> Bonuses { get; private set; }

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
        return new PlayerData(Health, Weapon, Stats, 
            new Dictionary<string, int>(Classes), new List<BonusBase>(Bonuses));
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
        Weapon = weapon;
    }

    public void GenerateStats()
    {
        Stats newStats;
        Random rand = new();
        do newStats = new(rand.Next(1, 4), rand.Next(1, 4), rand.Next(1, 4));
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
        if (bonus.ClassBonus != null) Bonuses.Add(bonus.ClassBonus);

        var statBonus = Array.Find(data.StatBonus, x => x.UnlockLevel == currentLevel);
        if (!statBonus.Stats.IsZero()) Stats += statBonus.Stats;
    }

    public int CalculateBonusDamage(TurnData data, BonusType type)
    {
        return Bonuses.Where(x => x.Type == type).Sum(x => x.Use(data));
    }
}
