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
    [SerializeField] private float _animationTime = 0.4f;

    [Inject]
    private void Construct(IAsyncSubscriber<StartBattleMessage> startBattleSub,
        ISubscriber<BattleVictoryMessage> battleVictorySub, ISubscriber<GameOverMessage> gameOverSub)
    {
        DisposableBag.Create(
            startBattleSub.Subscribe(OnBattleStarted),
            battleVictorySub.Subscribe(x => OnBattleWon(x.OnClickAction).Forget()),
            gameOverSub.Subscribe(_ => OnGameOver().Forget())
        ).AddTo(destroyCancellationToken);
    }

    private void Start()
    {
        ShowMenuUI();
    }

    private void ShowMenuUI()
    {
        MoveUIDown(_character, 0f).Forget();
        MoveUIDown(_startBattle, -80f).Forget();
    }

    private async UniTask OnBattleStarted(StartBattleMessage message, CancellationToken token)
    {
        _generate.interactable = false;
        var startBattle = _startBattle.GetComponent<Button>();
        startBattle.interactable = false;

        await UniTask.WhenAll(
            _character.DOAnchorPosY(Screen.height, _animationTime).SetEase(Ease.InBack).ToUniTask(),
            _startBattle.DOAnchorPosY(Screen.height, _animationTime).SetEase(Ease.Linear).ToUniTask()
        );

        if (_generate.gameObject.activeSelf)
            _generate.gameObject.SetActive(false);
        startBattle.interactable = true;

        await UniTask.WhenAll(
            MoveUIDown(_battle, 0f),
            MoveUIDown(_status, -90f)
        );
    }

    private async UniTask OnBattleWon(Action action, CancellationToken token = default)
    {
        await UniTask.WhenAll(
            _status.DOAnchorPosY(Screen.height, _animationTime).SetEase(Ease.InBack).ToUniTask(cancellationToken: token),
            _battle.DOAnchorPosY(Screen.height, _animationTime).SetEase(Ease.InBack).ToUniTask(cancellationToken: token)
        );

        _rejectWeapon.onClick.RemoveAllListeners();
        _rejectWeapon.onClick.AddListener(() => ReturnToCharacterWindow().Forget());

        _takeWeapon.onClick.RemoveAllListeners();
        _takeWeapon.onClick.AddListener(() => {
            action.Invoke();
            ReturnToCharacterWindow().Forget();
        });

        await UniTask.WhenAll(
            MoveUIDown(_status, -90f),
            MoveUIDown(_weaponEquipment, 0f)
        );
    }

    private async UniTask ReturnToCharacterWindow()
    {
        await UniTask.WhenAll(
            _status.DOAnchorPosY(Screen.height, _animationTime).SetEase(Ease.InBack).ToUniTask(),
            _weaponEquipment.DOAnchorPosY(Screen.height, _animationTime).SetEase(Ease.InBack).ToUniTask()
        );
        ShowMenuUI();
    }

    private async UniTask OnGameOver()
    {
        await UniTask.WhenAll(
            _status.DOAnchorPosY(Screen.height, _animationTime).SetEase(Ease.InBack).ToUniTask(),
            _battle.DOAnchorPosY(Screen.height, _animationTime).SetEase(Ease.InBack).ToUniTask()
        );

        _replay.onClick.RemoveAllListeners();
        _replay.onClick.AddListener(() => OnReplayClicked().Forget());

        await UniTask.WhenAll(
            MoveUIDown((RectTransform)_replay.transform, 0f),
            _status.DOAnchorPosY(-90f, _animationTime).SetEase(Ease.Linear).ToUniTask()
        );
    }

    private async UniTask OnReplayClicked()
    {
        var rect = (RectTransform)_replay.transform;
        await UniTask.WhenAll(
            _status.DOAnchorPosY((Screen.height), _animationTime).SetEase(Ease.InBack).ToUniTask(),
            rect.DOAnchorPosY(1200f, _animationTime).SetEase(Ease.InBack).ToUniTask()
        );
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private async UniTask MoveUIDown(RectTransform rectTransform, float targetPositionY, 
        CancellationToken token = default)
    {
        if (rectTransform == null) return;

        var anchoredPosition = rectTransform.anchoredPosition;
        anchoredPosition.y = Screen.height;
        rectTransform.anchoredPosition = anchoredPosition;

        rectTransform.gameObject.SetActive(true);
        await rectTransform.DOAnchorPosY(targetPositionY, _animationTime)
            .SetEase(Ease.Linear).ToUniTask(cancellationToken: token);
    }
}
