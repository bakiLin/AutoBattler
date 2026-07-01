using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class PickClassButton : MonoBehaviour
{
    [SerializeField] private ClassSO _classSo;
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
        _button.onClick.AddListener(() => _playerManager.PickClass(_classSo));
    }

    private void OnDestroy()
    {
        _button.onClick.RemoveAllListeners();
    }
}
