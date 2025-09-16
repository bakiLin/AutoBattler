using UnityEngine;

public class WindowManager : MonoBehaviour
{
    [SerializeField]
    private BattleManager _battleManager;

    [SerializeField]
    private GameObject _battleButton, _characterCreation, _battleWindow, _levelUp;

    private void OnEnable()
    {
        _battleManager.OnStartBattle += SetBattleWindows;
        _battleManager.OnLevelUpChangeUI += LevelUpWindow;
    }

    private void OnDisable()
    {
        _battleManager.OnStartBattle -= SetBattleWindows;
        _battleManager.OnLevelUpChangeUI -= LevelUpWindow;
    }

    private void SetBattleWindows()
    {
        _battleButton.SetActive(false);
        _characterCreation.SetActive(false);
        _levelUp.SetActive(false);
        _battleWindow.SetActive(true);
    }

    private void LevelUpWindow()
    {
        _battleWindow.SetActive(false);
        _battleButton.SetActive(true);
        _levelUp.SetActive(true);
    }
}
