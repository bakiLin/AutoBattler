using UnityEngine;

public class WindowManager : MonoBehaviour
{
    [SerializeField]
    private BattleManager _battleManager;

    [SerializeField]
    private GameObject[] _turnOff, _turnOn;

    private void OnEnable()
    {
        _battleManager.OnStartBattle += SetBattleWindows;
    }

    private void OnDisable()
    {
        _battleManager.OnStartBattle -= SetBattleWindows;
    }

    private void SetBattleWindows()
    {
        foreach (var item in _turnOff)
            item.SetActive(false);

        foreach (var item in _turnOn) 
            item.SetActive(true);
    }
}
