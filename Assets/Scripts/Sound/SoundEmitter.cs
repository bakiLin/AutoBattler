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

    public IObjectPool<SoundEmitter> Pool { set => _pool = value; }

    private void Awake()
    {
        _source = GetComponent<AudioSource>();
    }

    public async UniTask Play(SoundDataSO data, CancellationToken token)
    {
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
                await UniTask.Delay((int)(_source.clip.length * 1000), cancellationToken: token,
                    cancelImmediately: true);
                _source.Stop();
                _pool.Release(this);
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
            _source.Stop();
            _pool.Release(this);
        }
    }
}
