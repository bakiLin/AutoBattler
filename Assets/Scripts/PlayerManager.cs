using MessagePipe;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

public class PlayerManager
{
    private PlayerData _data;
    private PlayerData _snapshot;
    private GameDatabaseSO _database;
    private IPublisher<UpdatePlayerInMenuMessage> _updatePlayerInMenu;

    private PlayerManager(GameDatabaseSO database, IPublisher<UpdatePlayerInMenuMessage> updatePlayerInMenu)
    {
        _database = database;
        _updatePlayerInMenu = updatePlayerInMenu;

        _data = new PlayerData(0, new Weapon(), new Stats(0, 0, 0), 
            new Dictionary<string, int>(), new List<BonusBase>());
        SaveSnapshot();
    }

    public PlayerData GetPlayerSnapshot() => _data.Clone();

    public void SaveSnapshot() => _snapshot = _data.Clone();

    public void GenerateStats()
    {
        Stats newStats;
        Random rand = new();

        do newStats = new Stats(rand.Next(1, 4), rand.Next(1, 4), rand.Next(1, 4));
        while (_data.Stats.Equals(newStats));

        _data.Stats = newStats;
        _data.Health = CalculateMaxHealth();

        NotifyUI();
    }

    public void PickClass(ClassSO so)
    {
        _data.Restore(_snapshot);
        _data.Classes[so.Id] = _data.Classes.GetValueOrDefault(so.Id, 0) + 1;

        ApplyClassBonuses(so);

        if (_data.Classes.Values.Sum() == 1)
            _data.Weapon = so.Weapon.Weapon;

        _data.Health = CalculateMaxHealth();
        NotifyUI();
    }

    private void ApplyClassBonuses(ClassSO data)
    {
        if (!_data.Classes.TryGetValue(data.Id, out int currentLevel)) return;

        var bonus = Array.Find(data.Bonus, x => x.UnlockLevel == currentLevel);
        if (bonus.ClassBonus != null) _data.Bonuses.Add(bonus.ClassBonus);

        var statBonus = Array.Find(data.StatBonus, x => x.UnlockLevel == currentLevel);
        if (!statBonus.Stats.IsZero()) _data.Stats += statBonus.Stats;
    }

    public void EquipWeapon(Weapon weapon)
    {
        _data.Weapon = weapon;
        SaveSnapshot();
        NotifyUI();
    }

    public bool IsReadyToBattle(int requiredLevel)
    {
        return !_data.Stats.IsZero() && _data.Classes.Values.Sum() == requiredLevel;
    }

    private int CalculateMaxHealth()
    {
        int totalHealth = _data.Stats.Endurance;
        foreach (var pair in _data.Classes)
        {
            var conf = _database.GetClassById(pair.Key);
            if (conf != null) totalHealth += conf.Health * pair.Value;
        }
        return totalHealth;
    }

    private void NotifyUI()
    {
        _updatePlayerInMenu.Publish(new UpdatePlayerInMenuMessage(_data));
    }
}
