using System;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] Sounds;

    private void Start()
    {
        foreach (var sound in Sounds)
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
        var s = Array.Find(Sounds, sound => sound.name == soundName);
        s?.audioSource.Play();
    }
    
    public AudioSource GetSound(string soundName)
    {
        var s = Array.Find(Sounds, sound => sound.name == soundName);
        return s.audioSource;
    }
}