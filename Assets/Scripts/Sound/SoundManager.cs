using UnityEngine;
using UnityEngine.Pool;

public class SoundManager : MonoBehaviour, ISoundManager
{
    [SerializeField] private bool _collectionCheck = true;
    [SerializeField] private int _defaultCapacity = 100;
    [SerializeField] private int _maxSize = 20;
    [SerializeField] private SoundEmitter _emitterPrefab;
    private IObjectPool<SoundEmitter> _pool;

    private void Awake()
    {
        _pool = new ObjectPool<SoundEmitter>(CreateSoundEmitter, OnGetFromPool, OnReleaseToPool,
            OnDestroyPooledObject, _collectionCheck, _defaultCapacity, _maxSize);
    }

    private SoundEmitter CreateSoundEmitter()
    {
        SoundEmitter emitter = Instantiate(_emitterPrefab, transform);
        emitter.Pool = _pool;
        return emitter;
    }
    
    private void OnGetFromPool(SoundEmitter emitter)
    {
        emitter.gameObject.SetActive(true);
    }

    private void OnReleaseToPool(SoundEmitter emitter)
    {
        emitter.gameObject.SetActive(false);
    }

    private void OnDestroyPooledObject(SoundEmitter emitter)
    {
        Destroy(emitter.gameObject);
    }

    public SoundEmitter Get()
    {
        return _pool.Get();
    }
}
