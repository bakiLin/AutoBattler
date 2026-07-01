using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class GenerateStatsButton : MonoBehaviour
{
    private PlayerManager _playerManager;
    private Button _button;

    [Inject]
    private void Construct(PlayerManager playerManager)
    {
        _button = GetComponent<Button>();
        _playerManager = playerManager;
    }

    private void Start()
    {
        _button.onClick.AddListener(() => _playerManager.GenerateStats());
    }

    private void OnDestroy()
    {
        _button.onClick.RemoveAllListeners();
    }
}
