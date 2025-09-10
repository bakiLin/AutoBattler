using UnityEngine;

[CreateAssetMenu(menuName = "SO/Enemy", fileName = "New Enemy")]
public class EnemySO : ScriptableObject
{
    [SerializeField]
    private int _health, _damage, _strength, _dexterity, _endurance;

    [SerializeField]
    private WeaponSO _reward;
}
