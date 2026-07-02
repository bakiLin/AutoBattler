using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class PickClassButton : MonoBehaviour
{
    [SerializeField] private ClassSO _classSo;
    private GameDatabaseSO _database;
    private PlayerManager _playerManager;
    private ISoundManager _soundManager;
    private Button _button;

    [Inject]
    private void Construct(PlayerManager playerManager, ISoundManager soundManager, GameDatabaseSO database)
    {
        _button = GetComponent<Button>();
        _playerManager = playerManager;
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
        _playerManager.PickClass(_classSo);
    }

    private void OnDestroy()
    {
        _button.onClick.RemoveAllListeners();
    }
}
