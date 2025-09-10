using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField]
    private PlayerSO _player;

    [SerializeField]
    private CharacterSO _thief, _warrior, _barbarian;

    public void SelectStartClass(CharacterSO playerClass)
    {
        _player.Stats = playerClass.GetStartStats();
        _player.Weapon = playerClass.Weapon;

        //Debug.Log($"{_player.Weapon.name} {_player.Weapon.Damage}");
    }
}
