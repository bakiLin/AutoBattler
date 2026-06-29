using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    [SerializeField]
    private PlayerSO _player;

    [SerializeField]
    private Button[] _button;

    private void Start()
    {
        foreach (var button in _button)
        {
            button.onClick.AddListener(() => AudioManager.Instance.Play("button"));
        }
    }

    public void GenerateStats()
    {
        _player.GenerateStats();
    }

    public void SelectClass(ClassSO characterClass)
    {
        _player.SelectClass(characterClass);
    }
}
