using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField]
    private PlayerSO _player;

    [SerializeField]
    private ClassSO _thief, _warrior, _barbarian;

    public void GenerateStats()
    {
        _player.CreateNewPlayer();
    }

    public void SelectStartClass(ClassSO characterClass)
    {
        _player.SetStartClass(characterClass);
    }
}
