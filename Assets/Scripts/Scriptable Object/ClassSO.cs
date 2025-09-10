using UnityEngine;
using Random = System.Random;

[CreateAssetMenu(menuName = "SO/Class", fileName = "New Class")]
public class ClassSO : ScriptableObject
{
    [SerializeField]
    private int _health;

    [SerializeField]
    private WeaponSO _weapon;

    private Random _random = new Random();

    public WeaponSO Weapon { get => _weapon; }

    public ClassStats GetStartStats()
    {
        int strength = _random.Next(1, 4);
        int dexterity = _random.Next(1, 4);
        int endurance = _random.Next(1, 4);

        return new ClassStats(_health, strength, dexterity, endurance);
    }
}
