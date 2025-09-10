using UnityEngine;

[CreateAssetMenu(menuName = "SO/Player", fileName = "New Player")]
public class PlayerSO : ScriptableObject
{
    private CharacterStats _stats;

    private WeaponSO _weapon;

    public CharacterStats Stats {
        get => _stats;
        set { 
            _stats = new CharacterStats(value.Health, value.Strength, value.Dexterity, value.Endurance); 
        }  
    }

    public WeaponSO Weapon { get => _weapon; set => _weapon = value; }
}
