using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public Sound[] Sounds;

    [SerializeField]
    private AudioMixer audioMixer;

    private void Start()
    {
        foreach (var sound in Sounds)
        {
            ref var soundSource = ref sound.audioSource;
            soundSource = gameObject.AddComponent<AudioSource>();
            soundSource.clip = sound.clip;
            soundSource.volume = sound.volume;
            soundSource.pitch = sound.pitch;
            soundSource.outputAudioMixerGroup = audioMixer.outputAudioMixerGroup;
        }

        // Apply sound volume from settings to master audio mixer
        float volume = SettingsManager.Instance.GetVolume();
        float volumeDd = (float)Math.Log10(Math.Max(0.0001, volume)) * 20;
        audioMixer.SetFloat("volume", volumeDd);
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