using UnityEngine;
using Random = System.Random;

public class EnemyHolder : MonoBehaviour
{
    [SerializeField]
    private EnemySO[] _enemies;

    public EnemySO[] Enemies { get => _enemies; }

    private Random _random = new Random();

    public EnemySO GetRandom()
    {
        int index = _random.Next(0, _enemies.Length);
        return _enemies[index];
    }
}
