using TMPro;
using UnityEngine;

public class CharacterUI : MonoBehaviour
{
    [SerializeField]
    private ScriptableObjectHolder _scriptableObjectHolder;

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

    [Header("BONUS")]
    [SerializeField] private TextMeshProUGUI[] _bonus;

    private void OnEnable()
    {
        _player.OnUpdateClass += UpdateClassUI;
        _player.OnUpdateStats += UpdateStatsUI;
    }

    private void OnDisable()
    {
        _player.OnUpdateClass -= UpdateClassUI;
        _player.OnUpdateStats -= UpdateStatsUI;
    }

    private void UpdateClassUI(ClassSO characterClass)
    {
        _health.text = _player.GetHealth().ToString();

        var weapon = _player.Weapon;
        _weaponName.text = weapon.name;
        _weaponType.text = weapon.Type.ToString();
        _weaponDamage.text = weapon.Damage.ToString();

        _thief.text = SetClassLevel(_scriptableObjectHolder.Thief.name);
        _warrior.text = SetClassLevel(_scriptableObjectHolder.Warrior.name);
        _barbarian.text = SetClassLevel(_scriptableObjectHolder.Barbarian.name);

        ResetButtons();
        if (characterClass != null) SetButtons(characterClass.name);
        UpdateBonus();
        SetBonus();
    }

    private void UpdateStatsUI()
    {
        _strength.text = _player.Stats.Strength.ToString();
        _dexterity.text = _player.Stats.Dexterity.ToString();
        _endurance.text = _player.Stats.Endurance.ToString();

        UpdateBonus();
    }

    private void UpdateBonus()
    {
        if (_player.Stats != null && _player.ClassDictionary.Count > 0)
        {
            _health.text = $"{_player.GetHealth()} (+ {_player.Stats.Endurance})";
            _weaponDamage.text = $"{(_player.Weapon == null ? 0 : _player.Weapon.Damage)} (+ {_player.Stats.Strength})";
        }
    }

    private string SetClassLevel(string className)
    {
        if (_player.ClassDictionary.ContainsKey(className))
            return _player.ClassDictionary[className].Level.ToString();
        return "0";
    }

    public void ResetButtons()
    {
        if (_player.GetActiveLevel() < 3)
        {
            _thiefButton.SetActive(true);
            _warriorButton.SetActive(true);
            _barbarianButton.SetActive(true);
        }
        else
        {
            _thiefButton.SetActive(false);
            _warriorButton.SetActive(false);
            _barbarianButton.SetActive(false);
        }
    }

    private void SetButtons(string className)
    {
        if (className == _scriptableObjectHolder.Thief.name) _thiefButton.SetActive(false);
        else if (className == _scriptableObjectHolder.Warrior.name) _warriorButton.SetActive(false);
        else if (className == _scriptableObjectHolder.Barbarian.name) _barbarianButton.SetActive(false);
    }

    private void SetBonus()
    {
        for (int i = 0; i < _player.BonusList.Count; i++)
            _bonus[i].text = _player.BonusList[i].name.ToString();
    }
}
