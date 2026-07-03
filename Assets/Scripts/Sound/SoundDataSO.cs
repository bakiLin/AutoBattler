using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(menuName = "SO/Sound Data", fileName = "SoundDataSO")]
public class SoundDataSO : ScriptableObject
{
    [field: SerializeField] public AudioClip Clip { get; private set; }
    [field: SerializeField] public AudioMixerGroup Mixer { get; private set; }
    [field: SerializeField] public bool Loop { get; private set; }
    [field: SerializeField] public bool RandomPitch { get; private set; }
}
