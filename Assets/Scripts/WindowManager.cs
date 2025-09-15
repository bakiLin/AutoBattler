using UnityEngine;

public class WindowManager : MonoBehaviour
{
    [SerializeField]
    private BattleManager _battleManager;

    [SerializeField]
    private GameObject _characterCreation, _levelUp;

    [SerializeField]
    private GameObject[] _battleUI;

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
        _characterCreation.SetActive(false);

        foreach (var item in _battleUI)
            item.SetActive(true);
    }

    private void LevelUpWindow()
    {
        foreach (var item in _battleUI)
            item.SetActive(false);

        _levelUp.SetActive(true);
    }
}
