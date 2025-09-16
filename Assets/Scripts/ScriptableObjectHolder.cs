using UnityEngine;
using Random = System.Random;

public class ScriptableObjectHolder : MonoBehaviour
{
    [Header("CLASS")]
    [SerializeField] private ClassSO _thief;
    [SerializeField] private ClassSO _warrior;
    [SerializeField] private ClassSO _barbarian;

    [Header("ENEMY")]
    [SerializeField] private EnemySO[] _enemies;

    private Random _random = new Random();

    public ClassSO Thief { get => _thief; }

    public ClassSO Warrior { get => _warrior; }

    public ClassSO Barbarian { get => _barbarian; }

    public EnemySO GetRandom()
    {
        int index = _random.Next(0, _enemies.Length);
        return _enemies[index];
    }
}
