using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [System.Serializable]
    public class Sound
    {
        public string Name;

        public AudioClip Clip;

        public float Volume;

        public bool Loop;

        [HideInInspector] public AudioSource Source;
    }

    [SerializeField] private Sound[] sounds;

    public static AudioManager Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        foreach (var sound in sounds)
        {
            sound.Source = gameObject.AddComponent<AudioSource>();
            sound.Source.clip = sound.Clip;
            sound.Source.volume = sound.Volume;
            sound.Source.loop = sound.Loop;
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        Play("theme");
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, s => s.Name == name);
        if (s != null) s.Source.Play();
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, s => s.Name == name);
        if (s != null) s.Source.Stop();
    }
}
