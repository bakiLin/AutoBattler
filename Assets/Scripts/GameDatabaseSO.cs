using System;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(menuName = "SO/Game Database", fileName = "GameDatabaseSO")]
public class GameDatabaseSO : ScriptableObject
{
    [field: SerializeField] public ClassSO[] Classes { get; private set; }
    [field: SerializeField] public EnemySO[] Enemies { get; private set; }
    [field: SerializeField] public SoundDataSO ClickSound { get; private set; }
    [field: SerializeField] public SoundDataSO AttackSound { get; private set; }
    [field: SerializeField] public SoundDataSO MissSound { get; private set; }
    [field: SerializeField] public float AnimationTime { get; private set; } = 0.4f;
    [field: SerializeField] public int TurnDelay { get; private set; } = 200;

    public ClassSO GetClassById(string id)
    {
        return Array.Find(Classes, x => x.Id == id);
    }

    public Enemy GetRandomEnemy()
    {
        return Enemies[Random.Range(0, Enemies.Length)].Enemy;
    }
}