using UnityEngine;
using Random = System.Random;

[CreateAssetMenu(menuName = "SO/Character", fileName = "New Character")]
public class CharacterSO : ScriptableObject
{
    [SerializeField]
    private int _health;

    [SerializeField]
    private WeaponSO _weapon;

    private Random _random = new Random();

    public WeaponSO Weapon { get => _weapon; }

    public CharacterStats GetStartStats()
    {
        int strength = _random.Next(1, 4);
        int dexterity = _random.Next(1, 4);
        int endurance = _random.Next(1, 4);

        return new CharacterStats(_health, strength, dexterity, endurance);
    }
}
