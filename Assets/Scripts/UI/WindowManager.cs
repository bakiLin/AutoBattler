using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class WindowManager : MonoBehaviour
{
    [SerializeField]
    private BattleManager _battleManager;

    [SerializeField]
    private PlayerSO _player;

    [SerializeField]
    private float _animationTime;

    [Header("WINDOW")]
    [SerializeField] private RectTransform _characterWindow;
    [SerializeField] private RectTransform _battleWindow;
    [SerializeField] private RectTransform _weaponWindow;
    [SerializeField] private RectTransform _status;

    [Header("BUTTON")]
    [SerializeField] private GameObject _generateButton;
    [SerializeField] private GameObject _takeWeaponButton;
    [SerializeField] private RectTransform _battleButton;
    [SerializeField] private RectTransform _replayButton;

    private void OnEnable()
    {
        _battleManager.OnStartBattle += BattleWindow;
        _battleManager.OnEndBattle += WeaponWindow;
        _battleManager.OnGameOver += GameOver;
    }

    private void OnDisable()
    {
        _battleManager.OnStartBattle -= BattleWindow;
        _battleManager.OnEndBattle -= WeaponWindow;
        _battleManager.OnGameOver -= GameOver;
    }

    private void Start()
    {
        Invoke(nameof(ShowStartUI), .2f);
    }

    private void ShowStartUI()
    {
        MoveUIDown(ref _characterWindow, 0f);
        MoveUIDown(ref _battleButton, -80f);
    }

    private void BattleWindow()
    {
        _characterWindow.DOAnchorPosY(1200f, _animationTime).SetEase(Ease.InBack);
        _battleButton.DOAnchorPosY(1200f, _animationTime)
            .SetEase(Ease.Linear)
            .OnComplete(() => 
            {
                _generateButton.SetActive(false);

                MoveUIDown(ref _battleWindow, 0f);
                MoveUIDown(ref _status, -90f);
            });
    }

    private void WeaponWindow(WeaponSO weapon)
    {
        _status.DOAnchorPosY(1200f, _animationTime).SetEase(Ease.InBack);
        _battleWindow.DOAnchorPosY(1200f, _animationTime).SetEase(Ease.InBack)
            .OnComplete(() =>
            {
                var button = _takeWeaponButton.GetComponent<Button>();
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() => {
                    AudioManager.Instance.Play("button");
                    _player.Weapon = weapon;
                    CharacterWindow();
                });

                MoveUIDown(ref _status, -90f);
                MoveUIDown(ref _weaponWindow, 0f);
            });
    }

    public void CharacterWindow()
    {
        _status.DOAnchorPosY(1200f, _animationTime).SetEase(Ease.InBack);
        _weaponWindow.DOAnchorPosY(1200f, _animationTime).SetEase(Ease.InBack)
            .OnComplete(() => ShowStartUI());
    }

    private void GameOver()
    {
        _status.DOAnchorPosY(1200f, _animationTime).SetEase(Ease.InBack);
        _battleWindow.DOAnchorPosY(1200f, _animationTime).SetEase(Ease.InBack)
            .OnComplete(() =>
            {
                var button = _replayButton.GetComponent<Button>();
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() => Replay());

                MoveUIDown(ref _replayButton, 0f);
                _status.DOAnchorPosY(-90f, _animationTime).SetEase(Ease.Linear);
            });
    }

    public void Replay()
    {
        AudioManager.Instance.Play("button");
        _status.DOAnchorPosY(1200f, _animationTime).SetEase(Ease.InBack);
        _replayButton.DOAnchorPosY(1200f, _animationTime)
            .SetEase(Ease.InBack)
            .OnComplete(() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex));
    }

    private void MoveUIDown(ref RectTransform rectTransform, float position)
    {
        var anchoredPosition = rectTransform.anchoredPosition;
        anchoredPosition.y = 1200f;
        rectTransform.anchoredPosition = anchoredPosition;

        rectTransform.gameObject.SetActive(true);
        rectTransform.DOAnchorPosY(position, _animationTime).SetEase(Ease.Linear);
    }
}
