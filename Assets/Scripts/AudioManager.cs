using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    private void Start()
    {
        foreach (var sound in sounds)
        {
            ref var soundSource = ref sound.audioSource;
            soundSource = gameObject.AddComponent<AudioSource>();
            soundSource.clip = sound.clip;
            soundSource.volume = sound.volume;
            soundSource.pitch = sound.pitch;
        }
    }

    public void Play(string soundName)
    {
        var s = Array.Find(sounds, sound => sound.name == soundName);
        s?.audioSource.Play();
    }
}