using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class GenerateStatsButton : MonoBehaviour
{
    private PlayerManager _playerManager;
    private ISoundManager _soundManager;
    private GameDatabaseSO _database;
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
        _playerManager.GenerateStats();
    }

    private void OnDestroy()
    {
        _button.onClick.RemoveAllListeners();
    }
}
