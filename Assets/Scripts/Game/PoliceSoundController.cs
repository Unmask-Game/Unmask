using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceSoundController : MonoBehaviour
{
    [SerializeField] private PoliceMovement movement;
    [SerializeField] private AudioManager audioManager;
    private AudioSource _footstepSound;
    private AudioSource _footstepSound2;

    // Start is called before the first frame update
    void Start()
    {
        _footstepSound = audioManager.GetSound("Footstep");
        _footstepSound2 = audioManager.GetSound("Footstep2");
    }

    // Update is called once per frame
    void Update()
    {
        if (movement.IsWalking() && !_footstepSound.isPlaying && !_footstepSound2.isPlaying)
        {
            if (Random.Range(0f, 1f) > 0.5f)
            {
                _footstepSound.volume = Random.Range(0.02f, 0.03f);
                _footstepSound.pitch = Random.Range(0.9f, 1.1f);
                _footstepSound.Play();
            }
            else
            {
                _footstepSound2.volume = Random.Range(0.02f, 0.03f);
                _footstepSound2.pitch = Random.Range(0.9f, 1.1f);
                _footstepSound2.Play();
            }
        }
    }
}