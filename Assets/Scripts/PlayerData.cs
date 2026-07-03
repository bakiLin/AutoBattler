using System.Collections.Generic;

public struct PlayerData
{
    public int Health { get; set; }
    public Weapon Weapon { get; set; }
    public Stats Stats { get; set; }
    public Dictionary<string, int> Classes { get; set; }
    public List<BonusBase> Bonuses { get; set; }

    public PlayerData(int health, Weapon weapon, 
        Stats stats, Dictionary<string, int> classes, List<BonusBase> bonuses)
    {
        Health = health;
        Weapon = weapon;
        Stats = stats;
        Classes = new Dictionary<string, int>(classes);
        Bonuses = new List<BonusBase>(bonuses);
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
}
