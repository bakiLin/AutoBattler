using TMPro;
using UnityEngine;

public class BattleUI : MonoBehaviour
{
    [SerializeField]
    private BattleManager _battleManager;

    [SerializeField]
    private PlayerSO _player;

    [Header("PLAYER")]
    [SerializeField] private TextMeshProUGUI _playerHealth;
    [SerializeField] private TextMeshProUGUI _playerDamage;
    [SerializeField] private TextMeshProUGUI _playerStrength;
    [SerializeField] private TextMeshProUGUI _playerDexterity;
    [SerializeField] private TextMeshProUGUI _playerEndurance;
    [SerializeField] private TextMeshProUGUI[] _playerBonus;

    [Header("ENEMY")]
    [SerializeField] private TextMeshProUGUI _enemyName;
    [SerializeField] private TextMeshProUGUI _enemyHealth;
    [SerializeField] private TextMeshProUGUI _enemyDamage;
    [SerializeField] private TextMeshProUGUI _enemyStrength;
    [SerializeField] private TextMeshProUGUI _enemyDexterity;
    [SerializeField] private TextMeshProUGUI _enemyEndurance;
    [SerializeField] private TextMeshProUGUI _enemyBonus;

    private void OnEnable()
    {
        _player.OnUpdateHealth += UpdatePlayerStats;
        _battleManager.OnUpdateEnemyUI += UpdateEnemyStats;
    }

    private void OnDisable()
    {
        _player.OnUpdateHealth -= UpdatePlayerStats;
        _battleManager.OnUpdateEnemyUI -= UpdateEnemyStats;
    }

    private void UpdatePlayerStats()
    {
        _playerHealth.text = _player.Health.ToString();
        _playerDamage.text = (_player.Weapon.Damage + _player.Stats.Strength).ToString();
        _playerStrength.text = _player.Stats.Strength.ToString();
        _playerDexterity.text = _player.Stats.Dexterity.ToString();
        _playerEndurance.text = _player.Stats.Endurance.ToString();

        for (int i = 0; i < _player.BonusList.Count; i++)
            _playerBonus[i].text = _player.BonusList[i].name.ToString();
    }

    private void UpdateEnemyStats(EnemySO enemy)
    {
        _enemyName.text = enemy.name;
        _enemyHealth.text = enemy.Health.ToString();
        _enemyDamage.text = (enemy.Damage + enemy.Stats.Strength).ToString();
        _enemyStrength.text = enemy.Stats.Strength.ToString();
        _enemyDexterity.text = enemy.Stats.Dexterity.ToString();
        _enemyEndurance.text = enemy.Stats.Endurance.ToString();

        if (enemy.Bonus != null) _enemyBonus.text = enemy.Bonus.name.ToString();
        else _enemyBonus.text = "";
    }
}
