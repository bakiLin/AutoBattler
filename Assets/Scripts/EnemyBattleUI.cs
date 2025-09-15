using TMPro;
using UnityEngine;

public class EnemyBattleUI : MonoBehaviour
{
    [SerializeField]
    private BattleManager _battleManager;

    [SerializeField] private TextMeshProUGUI _enemy;
    [SerializeField] private TextMeshProUGUI _health;
    [SerializeField] private TextMeshProUGUI _damage;
    [SerializeField] private TextMeshProUGUI _strength;
    [SerializeField] private TextMeshProUGUI _dexterity;
    [SerializeField] private TextMeshProUGUI _endurance;

    private void OnEnable()
    {
        _battleManager.OnChangeEnemyUI += SetEnemyStats;
    }

    private void OnDisable()
    {
        _battleManager.OnChangeEnemyUI -= SetEnemyStats;
    }

    private void SetEnemyStats(EnemySO enemy)
    {
        _enemy.text = enemy.name;
        _health.text = enemy.Health.ToString();
        _damage.text = (enemy.Damage + enemy.Strength).ToString();
        _strength.text = enemy.Strength.ToString();
        _dexterity.text = enemy.Dexterity.ToString();
        _endurance.text = enemy.Endurance.ToString();
    }
}
