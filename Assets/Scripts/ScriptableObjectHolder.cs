using UnityEngine;
using Random = System.Random;

[CreateAssetMenu(menuName = "SO/SO Holder", fileName = "SO Holder")]
public class ScriptableObjectHolder : ScriptableObject
{
    [Header("CLASS")]
    [SerializeField] private ClassSO _thief;
    [SerializeField] private ClassSO _warrior;
    [SerializeField] private ClassSO _barbarian;

    [Header("ENEMY")]
    [SerializeField] private EnemySO[] _enemies;

    [Header("BONUS")]
    [SerializeField] private BonusBase[] _availableBonus;

    public ClassSO Thief { get => _thief; }

    public ClassSO Warrior { get => _warrior; }

    public ClassSO Barbarian { get => _barbarian; }

    public BonusBase[] AvailableBonus { get => _availableBonus; }

    private Random _random = new Random();

    public EnemySO GetRandom()
    {
        int index = _random.Next(0, _enemies.Length);
        return _enemies[index];
    }
}
