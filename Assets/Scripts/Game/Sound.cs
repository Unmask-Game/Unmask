using System;
using UnityEngine;

[Serializable]
public class Sound
{
    // Source: https://www.youtube.com/watch?v=6OT43pvUyfY
    public string name;
    public AudioClip clip;
    [Range(0, 1)] public float volume;
    public float pitch;
    [HideInInspector] public AudioSource audioSource;
}