using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    [SerializeField]
    private PlayerSO _player;

    public void GenerateStats()
    {
        _player.GenerateStats();
    }

    public void SelectClass(ClassSO characterClass)
    {
        _player.SelectClass(characterClass);
    }

    public void Replay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
