using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField]
    private PlayerSO _player;

    [SerializeField]
    private ClassSO _thief, _warrior, _barbarian;

    public void SelectStartClass(ClassSO playerClass)
    {
        _player.Stats = playerClass.GetStartStats();
        _player.Weapon = playerClass.Weapon;

        //Debug.Log($"{_player.Stats.Health} {_player.Stats.Strength} {_player.Stats.Dexterity} {_player.Stats.Endurance}");
        //Debug.Log($"{_player.Weapon.name} {_player.Weapon.Type} {_player.Weapon.Damage}");
    }
}
