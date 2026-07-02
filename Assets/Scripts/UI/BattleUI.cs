using Cysharp.Threading.Tasks;
using MessagePipe;
using System;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

[Serializable]
public struct BattleCharacterUI
{
    [field: SerializeField] public TextMeshProUGUI Name { get; private set; }
    [field: SerializeField] public TextMeshProUGUI Health { get; private set; }
    [field: SerializeField] public TextMeshProUGUI Damage { get; private set; }
    [field: SerializeField] public TextMeshProUGUI Strength { get; private set; }
    [field: SerializeField] public TextMeshProUGUI Dexterity { get; private set; }
    [field: SerializeField] public TextMeshProUGUI Endurance { get; private set; }
    [field: SerializeField] public VerticalLayoutGroup Bonuses { get; private set; }

    public void SetMainInfo(BattleCharacter character, TextMeshProUGUI bonusPrefab)
    {
        Name.text = character.Id;
        Health.text = character.Health.ToString();
        Damage.text = (character.Weapon.Damage + character.Stats.Strength).ToString();
        Strength.text = character.Stats.Strength.ToString();
        Dexterity.text = character.Stats.Dexterity.ToString();
        Endurance.text = character.Stats.Endurance.ToString();

        var container = Bonuses.transform;
        for (int i = container.childCount - 1; i >= 0; i--)
            GameObject.Destroy(container.GetChild(i).gameObject);

        foreach (var bonus in character.Bonuses)
        {
            var item = GameObject.Instantiate(bonusPrefab, container);
            item.text = bonus.name;
        }

        Bonuses.CalculateLayoutInputHorizontal();
        Bonuses.CalculateLayoutInputVertical();
        Bonuses.SetLayoutHorizontal();
        Bonuses.SetLayoutVertical();
    }

    public void SetHealth(int health)
    {
        Health.text = health.ToString();
    }
}

public class BattleUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _bonusPrefab;
    [SerializeField] private TextMeshProUGUI _status;
    [SerializeField] private BattleCharacterUI _player;
    [SerializeField] private BattleCharacterUI _enemy;

    private int _lastPlayerHealth = -1;
    private int _lastEnemyHealth = -1;

    [Inject]
    private void Construct(IAsyncSubscriber<StartBattleMessage> startBattleSub,
        ISubscriber<UpdateUIInBattleMessage> updateUIInBattleSub, 
        ISubscriber<SetBattleStatusMessage> setBattleStatus)
    {
        DisposableBag.Create(
            startBattleSub.Subscribe(SetupBattleUI),
            updateUIInBattleSub.Subscribe(UpdateBattleHealth),
            setBattleStatus.Subscribe(x => _status.text = x.Status)
        ).AddTo(destroyCancellationToken);
    }

    private UniTask SetupBattleUI(StartBattleMessage message, CancellationToken token)
    {
        if (token.IsCancellationRequested)
            return UniTask.CompletedTask;

        _player.SetMainInfo(message.Player, _bonusPrefab);
        _enemy.SetMainInfo(message.Enemy, _bonusPrefab);

        _lastPlayerHealth = message.Player.Health;
        _lastEnemyHealth = message.Enemy.Health;

        return UniTask.CompletedTask;
    }

    private void UpdateBattleHealth(UpdateUIInBattleMessage message)
    {
        if (_lastPlayerHealth != message.PlayerCurrentHealth)
        {
            _lastPlayerHealth = message.PlayerCurrentHealth;
            _player.SetHealth(message.PlayerCurrentHealth);
        }

        if (_lastEnemyHealth != message.EnemyCurrentHealth)
        {
            _lastEnemyHealth = message.EnemyCurrentHealth;
            _enemy.SetHealth(message.EnemyCurrentHealth);
        }
    }
}