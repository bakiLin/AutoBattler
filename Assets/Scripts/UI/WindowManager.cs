using UnityEngine;
using UnityEngine.UI;

public class WindowManager : MonoBehaviour
{
    [SerializeField]
    private BattleManager _battleManager;

    [SerializeField]
    private PlayerSO _player;

    [Header("WINDOW")]
    [SerializeField] private GameObject _characterWindow;
    [SerializeField] private GameObject _battleWindow;
    [SerializeField] private GameObject _weaponWindow;
    [SerializeField] private GameObject _gameOverWindow;

    [Header("BUTTON")]
    [SerializeField] private GameObject _generateButton;
    [SerializeField] private GameObject _takeWeaponButton;
    [SerializeField] private GameObject _battleButton;

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

    private void BattleWindow()
    {
        _characterWindow.SetActive(false);
        _generateButton.SetActive(false);
        _battleButton.SetActive(false);
        _battleWindow.SetActive(true);
    }

    private void WeaponWindow(WeaponSO weapon)
    {
        var button = _takeWeaponButton.GetComponent<Button>();
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => {
            _player.Weapon = weapon;
            CharacterWindow();
        });

        _battleWindow.SetActive(false);
        _weaponWindow.SetActive(true);
    }

    public void CharacterWindow()
    {
        _weaponWindow.SetActive(false);
        _characterWindow.SetActive(true);
        _battleButton.SetActive(true);
    }

    private void GameOver()
    {
        _characterWindow.SetActive(false);
        _generateButton.SetActive(false);
        _battleButton.SetActive(false);
        _battleWindow.SetActive(false);
        _gameOverWindow.SetActive(true);
    }
}
