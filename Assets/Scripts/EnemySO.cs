using System;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Enemy", fileName = "EnemySO")]
public class EnemySO : ScriptableObject
{
    [field: SerializeField] public Enemy Enemy { get; private set; }
}

[Serializable]
public struct Enemy
{
    [field: SerializeField] public string Id { get; private set; }
    [field: SerializeField] public int Health { get; private set; }
    [field: SerializeField] public int Damage { get; private set; }
    [field: SerializeField] public WeaponSO Reward { get; private set; }
    [field: SerializeField] public BonusBase Bonus { get; private set; }
    [field: SerializeField] public Stats Stats { get; private set; }
}