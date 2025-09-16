using UnityEngine;

public class WindowManager : MonoBehaviour
{
    [SerializeField]
    private BattleManager _battleManager;

    [Header("WINDOW")]
    [SerializeField] private GameObject _characterWindow;
    [SerializeField] private GameObject _battleWindow;

    [Header("BUTTON")]
    [SerializeField] private GameObject _generateButton;
    [SerializeField] private GameObject _battleButton;

    private void OnEnable()
    {
        _battleManager.OnStartBattle += BattleWindow;
        _battleManager.OnEndBattle += CharacterWindow;
    }

    private void OnDisable()
    {
        _battleManager.OnStartBattle -= BattleWindow;
        _battleManager.OnEndBattle -= CharacterWindow;
    }

    private void BattleWindow()
    {
        _characterWindow.SetActive(false);
        _generateButton.SetActive(false);
        _battleButton.SetActive(false);
        _battleWindow.SetActive(true);
    }

    private void CharacterWindow()
    {
        _battleWindow.SetActive(false);
        _characterWindow.SetActive(true);
        _battleButton.SetActive(true);
    }
}
