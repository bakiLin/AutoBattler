using System.Collections.Generic;

public class PlayerDataCopy
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

    public PlayerDataCopy(Stats stats, Dictionary<string, ClassSO> classDictionary, List<BonusBase> bonusList)
    {
        if (stats != null)
            _stats = new Stats(stats.Strength, stats.Dexterity, stats.Endurance);

        _classDictionary = new Dictionary<string, ClassSO>();
        foreach (var pair in classDictionary)
            _classDictionary.Add(pair.Key, new ClassSO(pair.Value));

        _bonusList = new List<BonusBase>();
        foreach (var item in bonusList) _bonusList.Add(item);
    }

    public void GetPlayerDataCopy(ref Stats stats, ref Dictionary<string, ClassSO> classDictionary, ref List<BonusBase> bonusList)
    {
        if (_stats != null)
        {
            stats = null;
            stats = new Stats(_stats.Strength, _stats.Dexterity, _stats.Endurance);
        }

        classDictionary.Clear();
        foreach (var pair in _classDictionary)
            classDictionary.Add(pair.Key, new ClassSO(pair.Value));

        bonusList.Clear();
        foreach (var item in _bonusList)
            bonusList.Add(item);
    }

    public int CalculateLevel()
    {
        int level = 0;
        foreach (var value in _classDictionary.Values)
            level += value.Level;
        return level;
    }
}
