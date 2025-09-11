using TMPro;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField]
    private PlayerSO _player;

    [Header("STATS")]
    [SerializeField] private TextMeshProUGUI _strength;
    [SerializeField] private TextMeshProUGUI _dexterity;
    [SerializeField] private TextMeshProUGUI _endurance;

    [Header("CLASS")]
    [SerializeField] private TextMeshProUGUI _classType;
    [SerializeField] private TextMeshProUGUI _classHealth;

    [Header("WEAPON")]
    [SerializeField] private TextMeshProUGUI _weaponName;
    [SerializeField] private TextMeshProUGUI _weaponType;
    [SerializeField] private TextMeshProUGUI _weaponDamage;

    private void OnEnable()
    {
        _player.OnGenerateStats += ChangeStatsUI;
        _player.OnSelectStartClass += SelectStartClassUI;
    }

    private void OnDisable()
    {
        _player.OnGenerateStats -= ChangeStatsUI;
        _player.OnSelectStartClass -= SelectStartClassUI;
    }

    private void ChangeStatsUI(PlayerStats stats)
    {
        _strength.text = stats.Strength.ToString();
        _dexterity.text = stats.Dexterity.ToString();
        _endurance.text = stats.Endurance.ToString();

        SumUpStats();
    }

    private void SelectStartClassUI(ClassSO characterClass)
    {
        _classType.text = characterClass.name;
        _classHealth.text = characterClass.Health.ToString();

        _weaponName.text = characterClass.Weapon.name;
        _weaponType.text = characterClass.Weapon.Type.ToString();
        _weaponDamage.text = characterClass.Weapon.Damage.ToString();

        SumUpStats();
    }

    private void SumUpStats()
    {
        if (_player.Stats != null && _player.Class != null)
        {
            _classHealth.text = $"{_player.Class.Health} (+ {_player.Stats.Endurance})";
            _weaponDamage.text = $"{_player.Weapon.Damage} (+ {_player.Stats.Strength})";
        }
    }
}
