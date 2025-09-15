using TMPro;
using UnityEngine;

public class PlayerBattleUI : MonoBehaviour
{
    [SerializeField]
    private BattleManager _battleManager;

    [SerializeField]
    private PlayerSO _player;

    [SerializeField] private TextMeshProUGUI _health;
    [SerializeField] private TextMeshProUGUI _damage;
    [SerializeField] private TextMeshProUGUI _strength;
    [SerializeField] private TextMeshProUGUI _dexterity;
    [SerializeField] private TextMeshProUGUI _endurance;

    private void OnEnable()
    {
        _battleManager.OnStartBattle += SetPlayerBattleStats;
    }

    private void OnDisable()
    {
        _battleManager.OnStartBattle -= SetPlayerBattleStats;
    }

    private void SetPlayerBattleStats()
    {
        _health.text = _player.Health.ToString();
        _damage.text = (_player.Weapon.Damage + _player.Stats.Strength).ToString();
        _strength.text = _player.Stats.Strength.ToString();
        _dexterity.text = _player.Stats.Dexterity.ToString();
        _endurance.text = _player.Stats.Endurance.ToString();
    }
}
