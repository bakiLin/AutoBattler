using UnityEngine;

[CreateAssetMenu(menuName = "SO/Class", fileName = "New Class")]
public class ClassSO : ScriptableObject
{
    // ID
    [SerializeField]
    private int _id;

    public int Id { get => _id; }

    // Health
    [SerializeField]
    private int _health;

    public int Health { get => _health; }

    // Weapon
    [SerializeField]
    private WeaponSO _weapon;

    public WeaponSO Weapon { get => _weapon; }
    
    // Level
    private int _level = 0;

    public int Level { get => _level; set => _level = value; }

    public ClassSO(ClassSO characterClass)
    {
        name = characterClass.name;
        _health = characterClass.Health;
        _weapon = characterClass.Weapon;    
        _level = characterClass._level;
    }
}
