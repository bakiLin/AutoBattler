using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class StartBattleButton : MonoBehaviour
{
    private BattleManager _battleManager;
    private ISoundManager _soundManager;
    private GameDatabaseSO _database;
    private Button _button;

    [Inject]
    private void Construct(BattleManager battleManager, ISoundManager soundManager, GameDatabaseSO database)
    {
        _button = GetComponent<Button>();
        _battleManager = battleManager;
        _soundManager = soundManager;
        _database = database;
    }

    private void Start()
    {
        _button.onClick.AddListener(OnClick);
    }

    private void OnClick() 
    {
        _soundManager.Get().Play(_database.ClickSound, destroyCancellationToken);
        _button.interactable = false;
        _battleManager.StartBattle(destroyCancellationToken).Forget(Debug.LogException);
    }

    private void OnDestroy()
    {
        _button.onClick.RemoveAllListeners();
    }
}
