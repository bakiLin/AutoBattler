using Cysharp.Threading.Tasks;
using MessagePipe;
using System;
using System.Threading;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

[Serializable]
public class BattleCharacterUI
{
    [field: SerializeField] public RectTransform Window { get; private set; }
    [field: SerializeField] public TextMeshProUGUI Name { get; private set; }
    [field: SerializeField] public TextMeshProUGUI Health { get; private set; }
    [field: SerializeField] public TextMeshProUGUI Damage { get; private set; }
    [field: SerializeField] public TextMeshProUGUI Strength { get; private set; }
    [field: SerializeField] public TextMeshProUGUI Dexterity { get; private set; }
    [field: SerializeField] public TextMeshProUGUI Endurance { get; private set; }
    [field: SerializeField] public VerticalLayoutGroup Bonuses { get; private set; }
    private Color _originalColor;

    public void SetMainInfo(BattleCharacter character, TextMeshProUGUI bonusPrefab)
    {
        _originalColor = Health.color;
        Health.DOKill();
        Health.color = _originalColor;

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

    public void SetHealth(int health, float duration)
    {
        Health.DOKill();
        Health.text = health.ToString();
        Health.color = Color.red;
        Health.DOColor(_originalColor, duration);
    }

    public void Shake(float duration)
    {
        Window.DOKill();
        Window.DOShakeAnchorPos(duration, 30f, 10, 90f);
    }

    public void Miss(float duration, float direction)
    {
        Window.DOKill();
        var seq = DOTween.Sequence();
        seq.Append(Window.DOAnchorPosX(Window.anchoredPosition.x + direction * 40f, duration * 0.5f).SetEase(Ease.OutSine));
        //seq.Join(Window.DORotate(new Vector3(0, 0, -direction * 10f), duration * 0.5f).SetEase(Ease.OutSine));
        seq.Append(Window.DOAnchorPosX(Window.anchoredPosition.x, duration * 0.5f).SetEase(Ease.InSine));
        //seq.Join(Window.DORotate(Vector3.zero, duration * 0.5f).SetEase(Ease.InSine));
    }
}

public class BattleUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _bonusPrefab;
    [SerializeField] private TextMeshProUGUI _status;
    [SerializeField] private BattleCharacterUI _player;
    [SerializeField] private BattleCharacterUI _enemy;

    private GameDatabaseSO _database;
    private int _lastPlayerHealth = -1;
    private int _lastEnemyHealth = -1;

    [Inject]
    private void Construct(IAsyncSubscriber<StartBattleMessage> startBattleSub,
        ISubscriber<UpdateUIInBattleMessage> updateUIInBattleSub, 
        ISubscriber<SetBattleStatusMessage> setBattleStatus,
        ISubscriber<CharacterMissedMessage> characterMissedSub,
        GameDatabaseSO database)
    {
        _database = database;
        DisposableBag.Create(
            startBattleSub.Subscribe(SetupBattleUI),
            updateUIInBattleSub.Subscribe(UpdateBattleHealth),
            setBattleStatus.Subscribe(x => _status.text = x.Status),
            characterMissedSub.Subscribe(OnCharacterMissed)
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
            _player.SetHealth(message.PlayerCurrentHealth, _database.AnimationTime);
            _player.Shake(_database.AnimationTime);
        }
        
        if (_lastEnemyHealth != message.EnemyCurrentHealth)
        {
            _lastEnemyHealth = message.EnemyCurrentHealth;
            _enemy.SetHealth(message.EnemyCurrentHealth, _database.AnimationTime);
            _enemy.Shake(_database.AnimationTime);
        }
    }

    private void OnCharacterMissed(CharacterMissedMessage message)
    {
        if (message.IsPlayerTarget)
            _player.Miss(_database.AnimationTime, -1f);
        else
            _enemy.Miss(_database.AnimationTime, 1f);
    }
}