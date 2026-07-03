using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

[RequireComponent(typeof(AudioSource))]
public class SoundEmitter : MonoBehaviour
{
    private AudioSource _source;
    private IObjectPool<SoundEmitter> _pool;
    private bool _released;

    public IObjectPool<SoundEmitter> Pool { set => _pool = value; }

    private void Awake()
    {
        _source = GetComponent<AudioSource>();
    }

    public async UniTask Play(SoundDataSO data, CancellationToken token)
    {
        _released = false;

        if (data == null || data.Clip == null)
        {
            ReleaseToPool();
            return;
        }

        _source.clip = data.Clip;
        _source.outputAudioMixerGroup = data.Mixer;
        _source.loop = data.Loop;
        _source.pitch = data.RandomPitch ? Random.Range(0.85f, 1.15f) : 1f;

        await PlayAsync(token);
    }

    private async UniTask PlayAsync(CancellationToken token)
    {
        try
        {
            _source.Play();

            if (!_source.loop)
            {
                await UniTask.Delay((int)(_source.clip.length * 1000), 
                    cancellationToken: token, cancelImmediately: true);
            }
            else
            {
                while (!token.IsCancellationRequested)
                {
                    await UniTask.Yield(token, cancelImmediately: true);
                }
            }
        }
        catch (OperationCanceledException)
        {
        }
        finally
        {
            ReleaseToPool();
        }
    }

    private void ReleaseToPool()
    {
        if (_released) return;

        _released = true;
        _source.Stop();
        _source.clip = null;
        _pool?.Release(this);
    }
}
