using UnityEngine;

public class CharacterButtonManager : MonoBehaviour
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
}
