using UnityEngine;

[CreateAssetMenu(menuName = "SO/Class", fileName = "New Class")]
public class ClassSO : ScriptableObject
{
    [SerializeField]
    private int _health;

    [SerializeField]
    private WeaponSO _weapon;

    public int Health { get => _health; }

    public WeaponSO Weapon { get => _weapon; }
}
