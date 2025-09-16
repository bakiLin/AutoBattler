using TMPro;
using UnityEngine;

public class PlayerLevelUpUI : MonoBehaviour
{
    [SerializeField]
    private ClassHolder _classHolder;

    [SerializeField]
    private PlayerSO _player;

    [Header("STATS")]
    [SerializeField] private TextMeshProUGUI _health;
    [SerializeField] private TextMeshProUGUI _strength;
    [SerializeField] private TextMeshProUGUI _dexterity;
    [SerializeField] private TextMeshProUGUI _endurance;

    [Header("WEAPON")]
    [SerializeField] private TextMeshProUGUI _weaponName;
    [SerializeField] private TextMeshProUGUI _weaponType;
    [SerializeField] private TextMeshProUGUI _weaponDamage;

    [Header("CLASS")]
    [SerializeField] private TextMeshProUGUI _thief;
    [SerializeField] private TextMeshProUGUI _warrior;
    [SerializeField] private TextMeshProUGUI _barbarian;

    [Header("BUTTON")]
    [SerializeField] private GameObject _thiefButton;
    [SerializeField] private GameObject _warriorButton;
    [SerializeField] private GameObject _barbarianButton;

    private void OnEnable()
    {
        _player.OnChangeLevelUpUI += SetStats;
    }

    private void OnDisable()
    {
        _player.OnChangeLevelUpUI -= SetStats;

        _thiefButton.SetActive(true);
        _warriorButton.SetActive(true);
        _barbarianButton.SetActive(true);   
    }

    private void SetStats(ClassSO characterClass)
    {
        _health.text = $"{_player.Health} (+ {_player.Stats.Endurance})";
        _strength.text = _player.Stats.Strength.ToString();
        _dexterity.text = _player.Stats.Dexterity.ToString();
        _endurance.text = _player.Stats.Endurance.ToString();

        _weaponName.text = _player.Weapon.name;
        _weaponType.text = _player.Weapon.Type.ToString();
        _weaponDamage.text = $"{_player.Weapon.Damage} (+ {_player.Stats.Strength})";

        SetClassLevel(_classHolder.Thief.name, ref _thief);
        SetClassLevel(_classHolder.Warrior.name, ref _warrior);
        SetClassLevel(_classHolder.Barbarian.name, ref _barbarian);

        if (characterClass != null) SetButtons(characterClass);
    }

    private void SetClassLevel(string className, ref TextMeshProUGUI text)
    {
        if (_player.ClassDictionary.ContainsKey(className))
            text.text = _player.ClassDictionary[className].Level.ToString();
        else
            text.text = "0";
    }

    private void SetButtons(ClassSO characterClass)
    {
        _thiefButton.SetActive(true);
        _warriorButton.SetActive(true);
        _barbarianButton.SetActive(true);

        if (characterClass.name == _classHolder.Thief.name) _thiefButton.SetActive(false);
        else if (characterClass.name == _classHolder.Warrior.name) _warriorButton.SetActive(false);
        else if (characterClass.name == _classHolder.Barbarian.name) _barbarianButton.SetActive(false);
    }
}
