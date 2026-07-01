using MessagePipe;
using System.Linq;
using System.Collections.Generic;

public class PlayerManager : ICharacter
{
    public Stats Stats => _data.Stats;
    public Weapon Weapon => _data.Weapon;

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
        _snapshot = _data.Clone();
    }

    public PlayerData GetPlayer()
    {
        return _data.Clone();
    }

    public void GenerateStats()
    {
        _data.GenerateStats();
        _data.SetHealth(CalculateMaxHealth());
        NotifyUI();
    }

    public void PickClass(ClassSO so)
    {
        _data.Restore(_snapshot);
        _data.LevelUp(so.Id);
        _data.ApplyBonus(so);
        _data.SetHealth(CalculateMaxHealth());

        if (_data.Classes.Values.Sum() == 1)
            _data.EquipWeapon(so.Weapon.Weapon);

        NotifyUI();
    }

    public bool IsReadyToBattle(int requiredLevel)
    {
        return !_data.Stats.IsZero() && _data.Classes.Values.Sum() == requiredLevel;
    }

    public void RestoreHealth()
    {
        _data.SetHealth(CalculateMaxHealth());
        NotifyUI();
    }

    public void SaveSnapshot()
    {
        _snapshot = _data.Clone();
    }

    public int CalculateBonusDamage(TurnData data, BonusType type)
    {
        return _data.CalculateBonusDamage(data, type);
    }

    public void TakeDamage(int damage)
    {
        _data.SetHealth(_data.Health - damage);
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
