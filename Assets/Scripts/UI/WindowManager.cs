using Cysharp.Threading.Tasks;
using DG.Tweening;
using MessagePipe;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using VContainer;

public class WindowManager : MonoBehaviour
{
    [Header("WINDOWS")]
    [SerializeField] private RectTransform _character;
    [SerializeField] private RectTransform _battle;
    [SerializeField] private RectTransform _weaponEquipment;
    [SerializeField] private RectTransform _status;

    [Header("BUTTONS / ELEMENTS")]
    [SerializeField] private Button _generate;
    [SerializeField] private Button _takeWeapon;
    [SerializeField] private Button _rejectWeapon;
    [SerializeField] private Button _replay;
    [SerializeField] private RectTransform _startBattle;

    private ISoundManager _soundManager;
    private GameDatabaseSO _database;

    [Inject]
    private void Construct(IAsyncSubscriber<StartBattleMessage> startBattleSub,
        ISubscriber<BattleVictoryMessage> battleVictorySub,
        ISubscriber<GameOverMessage> gameOverSub,
        ISoundManager soundManager,
        GameDatabaseSO database)
    {
        _soundManager = soundManager;
        _database = database;

        DisposableBag.Create(
            startBattleSub.Subscribe(OnBattleStarted),
            battleVictorySub.Subscribe(x => OnBattleWon(x.OnClickAction, destroyCancellationToken).Forget(Debug.LogException)),
            gameOverSub.Subscribe(_ => OnGameOver(destroyCancellationToken).Forget(Debug.LogException))
        ).AddTo(destroyCancellationToken);
    }

    private void Start()
    {
        ShowMenuUI(destroyCancellationToken);
    }

    private void ShowMenuUI(CancellationToken token)
    {
        MoveUI(_character, 0f, _database.AnimationTime, token).Forget(Debug.LogException);
        MoveUI(_startBattle, -80f, _database.AnimationTime, token).Forget(Debug.LogException);
    }

    private async UniTask OnBattleStarted(StartBattleMessage message, CancellationToken token)
    {
        _generate.interactable = false;

        await UniTask.WhenAll(
            _character.DOAnchorPosY(Screen.height, _database.AnimationTime)
                .SetEase(Ease.InBack)
                .ToUniTask(cancellationToken: token),
            _startBattle.DOAnchorPosY(Screen.height, _database.AnimationTime)
                .SetEase(Ease.Linear)
                .ToUniTask(cancellationToken: token)
        );

        if (_generate.gameObject.activeSelf)
            _generate.gameObject.SetActive(false);

        await UniTask.WhenAll(
            MoveUI(_battle, 0f, _database.AnimationTime, token),
            MoveUI(_status, -90f, _database.AnimationTime, token)
        );
    
        _startBattle.GetComponent<Button>().interactable = true;
    }

    private async UniTask OnBattleWon(Action action, CancellationToken token = default)
    {
        await UniTask.WhenAll(
            _status.DOAnchorPosY(Screen.height, _database.AnimationTime)
                .SetEase(Ease.Linear)
                .ToUniTask(cancellationToken: token),
            _battle.DOAnchorPosY(Screen.height, _database.AnimationTime)
                .SetEase(Ease.InBack)
                .ToUniTask(cancellationToken: token)
        );

        _rejectWeapon.onClick.RemoveAllListeners();
        _rejectWeapon.onClick.AddListener(() =>
        {
            PlayClickSound();
            ReturnToCharacterWindow(destroyCancellationToken).Forget(Debug.LogException);
        });

        _takeWeapon.onClick.RemoveAllListeners();
        _takeWeapon.onClick.AddListener(() =>
        {
            PlayClickSound();
            action.Invoke();
            ReturnToCharacterWindow(destroyCancellationToken).Forget(Debug.LogException);
        });

        await UniTask.WhenAll(
            MoveUI(_status, -90f, _database.AnimationTime, token),
            MoveUI(_weaponEquipment, 0f, _database.AnimationTime, token)
        );
    }

    private async UniTask ReturnToCharacterWindow(CancellationToken token)
    {
        await UniTask.WhenAll(
            _status.DOAnchorPosY(Screen.height, _database.AnimationTime)
                .SetEase(Ease.Linear)
                .ToUniTask(cancellationToken: token),
            _weaponEquipment.DOAnchorPosY(Screen.height, _database.AnimationTime)
                .SetEase(Ease.InBack)
                .ToUniTask(cancellationToken: token)
        );

        ShowMenuUI(token);
    }

    private async UniTask OnGameOver(CancellationToken token)
    {
        await UniTask.WhenAll(
            _status.DOAnchorPosY(Screen.height, _database.AnimationTime)
                .SetEase(Ease.Linear)
                .ToUniTask(cancellationToken: token),
            _battle.DOAnchorPosY(Screen.height, _database.AnimationTime)
                .SetEase(Ease.InBack)
                .ToUniTask(cancellationToken: token)
        );

        _replay.onClick.RemoveAllListeners();
        _replay.onClick.AddListener(() =>
        {
            PlayClickSound();
            OnReplayClicked(destroyCancellationToken).Forget(Debug.LogException);
        });

        await UniTask.WhenAll(
            MoveUI((RectTransform)_replay.transform, 0f, _database.AnimationTime, token),
            _status.DOAnchorPosY(-90f, _database.AnimationTime)
                .SetEase(Ease.Linear)
                .ToUniTask(cancellationToken: token)
        );
    }

    private async UniTask OnReplayClicked(CancellationToken token)
    {
        var rect = (RectTransform)_replay.transform;

        await UniTask.WhenAll(
            _status.DOAnchorPosY(Screen.height, _database.AnimationTime)
                .SetEase(Ease.Linear)
                .ToUniTask(cancellationToken: token),
            rect.DOAnchorPosY(Screen.height, _database.AnimationTime)
                .SetEase(Ease.Linear)
                .ToUniTask(cancellationToken: token)
        );

        if (token.IsCancellationRequested) return;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void PlayClickSound()
    {
        _soundManager.Get().Play(_database.ClickSound, destroyCancellationToken);
    }

    private async UniTask MoveUI(RectTransform rectTransform, float targetPositionY, float duration,
        CancellationToken token = default)
    {
        if (rectTransform == null) return;

        var anchoredPosition = rectTransform.anchoredPosition;
        anchoredPosition.y = Screen.height;
        rectTransform.anchoredPosition = anchoredPosition;

        rectTransform.gameObject.SetActive(true);

        await rectTransform.DOAnchorPosY(targetPositionY, duration)
            .SetEase(Ease.Linear)
            .ToUniTask(cancellationToken: token);
    }
}
