using UnityEngine;

public class PoliceSoundController : MonoBehaviour
{
    [SerializeField] private PoliceMovement movement;
    [SerializeField] private AudioManager audioManager;
    private AudioSource _footstepSound;
    private AudioSource _footstepSound2;

    private void FixedUpdate()
    {
        if (!_footstepSound || !_footstepSound2)
        {
            _footstepSound = audioManager.GetSound("Footstep");
            _footstepSound2 = audioManager.GetSound("Footstep2");
        }
        // Play footstep sounds when walking
        else if (movement.isWalking && !_footstepSound.isPlaying && !_footstepSound2.isPlaying)
        {
            // Randomly use 1 of the 2 footstep sounds per step
            if (Random.Range(0f, 1f) > 0.5f)
            {
                _footstepSound.volume = Random.Range(0.02f, 0.03f); // Randomize volume and pitch for more realistic sound
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