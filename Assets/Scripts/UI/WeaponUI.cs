using TMPro;
using UnityEngine;

public class WeaponUI : MonoBehaviour
{
    [SerializeField]
    private BattleManager _battleManager;

    [SerializeField]
    private TextMeshProUGUI _name, _type, _damage;

    private void OnEnable()
    {
        _battleManager.OnEndBattle += UpdateWeaponUI;
    }

    private void OnDisable()
    {
        _battleManager.OnEndBattle -= UpdateWeaponUI;
    }

    private void UpdateWeaponUI(WeaponSO weapon)
    {
        _name.text = weapon.name;
        _type.text = weapon.Type.ToString();
        _damage.text = weapon.Damage.ToString();
    }
}
