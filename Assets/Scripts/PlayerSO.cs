using UnityEngine;

[CreateAssetMenu(menuName = "SO/Player", fileName = "New Player")]
public class PlayerSO : ScriptableObject
{
    private ClassStats _stats;

    private WeaponSO _weapon;

    public ClassStats Stats {
        get => _stats;
        set => _stats = new ClassStats(value.Health, value.Strength, value.Dexterity, value.Endurance); 
    }

    public WeaponSO Weapon { get => _weapon; set => _weapon = value; }
}
