using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class StartBattleButton : MonoBehaviour
{
    private BattleManager _battleManager;
    private Button _button;

    [Inject]
    private void Construct(BattleManager battleManager)
    {
        _button = GetComponent<Button>();
        _battleManager = battleManager;
    }

    private void Start()
    {
        _button.onClick.AddListener(() => _battleManager.StartBattle().Forget());
    }

    private void OnDestroy()
    {
        _button.onClick.RemoveAllListeners();
    }
}
