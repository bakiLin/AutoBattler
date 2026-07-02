using System.Collections.Generic;
using MessagePipe;
using TMPro;
using UnityEngine;
using VContainer;
using System;
using Cysharp.Threading.Tasks;

[Serializable]
public struct ClassUI
{
    public ClassSO Class;
    public TextMeshProUGUI LevelText;
    public GameObject LevelUpButton;
}

public class PlayerMenuUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _health;
    [SerializeField] private TextMeshProUGUI _strength;
    [SerializeField] private TextMeshProUGUI _dexterity;
    [SerializeField] private TextMeshProUGUI _endurance;

    [SerializeField] private TextMeshProUGUI _weaponName;
    [SerializeField] private TextMeshProUGUI _weaponType;
    [SerializeField] private TextMeshProUGUI _weaponDamage;

    [SerializeField] private ClassUI[] _classes;
    [SerializeField] private TextMeshProUGUI[] _bonuses;

    private int _lastHealth = -1;
    private int _lastStrength = -1;
    private int _lastDexterity = -1;
    private int _lastEndurance = -1;
    private string _lastWeaponId;
    private string[] _lastBonuses;
    private Dictionary<string, int> _lastClasses = new();

    [Inject]
    private void Construct(ISubscriber<UpdatePlayerInMenuMessage> updatePlayerInMenu,
        ISubscriber<BattleVictoryMessage> battleVictory)
    {
        _lastBonuses = new string[_bonuses.Length];
        Array.Fill(_lastBonuses, string.Empty);

        DisposableBag.Create(
            updatePlayerInMenu.Subscribe(UpdateUI),
            battleVictory.Subscribe(x => SetClassButtons(x.BattleNumber))
        ).AddTo(destroyCancellationToken);
    }

    private void SetClassButtons(int battleNumber)
    {
        if (battleNumber >= 4 && _classes[0].LevelUpButton.activeSelf)
        {
            foreach (var item in _classes)
                item.LevelUpButton.SetActive(false);
        }
    }

    private void UpdateUI(UpdatePlayerInMenuMessage message)
    {
        var data = message.PlayerData;
        var stats = data.Stats;
        var weapon = data.Weapon;

        if (_lastHealth != data.Health)
            _health.text = (_lastHealth = data.Health).ToString();

        if (_lastStrength != stats.Strength)
            _strength.text = (_lastStrength = stats.Strength).ToString();

        if (_lastDexterity != stats.Dexterity)
            _dexterity.text = (_lastDexterity = stats.Dexterity).ToString();

        if (_lastEndurance != stats.Endurance)
            _endurance.text = (_lastEndurance = stats.Endurance).ToString();

        if (_lastWeaponId != weapon.Id)
        {
            _lastWeaponId = weapon.Id;
            _weaponName.text = weapon.Id;
            _weaponType.text = weapon.Type.ToString();
            _weaponDamage.text = weapon.Damage.ToString();
        }

        UpdateClasses(data.Classes);
        UpdateBonuses(data.Bonuses);
    }

    private void UpdateClasses(Dictionary<string, int> classes)
    {
        if (classes == null) return;

        foreach (var item in _classes)
        {
            string id = item.Class.Id;
            int level = classes.GetValueOrDefault(id);

            if (_lastClasses.GetValueOrDefault(id, -1) != level)
                item.LevelText.text = (_lastClasses[id] = level).ToString();
        }
    }

    private void UpdateBonuses(List<BonusBase> bonuses)
    {
        for (int i = 0; i < _bonuses.Length; i++)
        {
            string newText = (bonuses != null && i < bonuses.Count) ? bonuses[i].name : string.Empty;

            if (_lastBonuses[i] != newText)
            {
                _lastBonuses[i] = newText;
                _bonuses[i].text = newText;
            }
        }
    }
}
