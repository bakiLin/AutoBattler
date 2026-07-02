using UnityEngine;
using VContainer;

public class MusicManager: MonoBehaviour
{
    [SerializeField] private SoundDataSO _musicData;
    private ISoundManager _soundManager;
    
    [Inject]
    private void Construct(ISoundManager soundManager)
    {
        _soundManager = soundManager;   
    }

    private void Start()
    {
        _soundManager.Get().Play(_musicData, destroyCancellationToken);
    }
}