using System;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(menuName = "SO/Game Database", fileName = "GameDatabaseSO")]
public class GameDatabaseSO : ScriptableObject
{
    [field: SerializeField] public ClassSO[] Classes { get; private set; }
    [field: SerializeField] public EnemySO[] Enemies { get; private set; }

    public ClassSO GetClassById(string id)
    {
        return Array.Find(Classes, x => x.Id == id);
    }

    public EnemySO GetRandomEnemy()
    {
        return Enemies[Random.Range(0, Enemies.Length)];
    }
}