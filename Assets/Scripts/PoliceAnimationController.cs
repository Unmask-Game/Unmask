using UnityEngine;
using Random = UnityEngine.Random;

public class PoliceAnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private AudioManager audioManager;

    private PoliceMovement _movement;
    private ItemController _items;
    private AudioSource _footstepSound;
    private AudioSource _footstepSound2;
    private bool _soundSwitchDone;

    private void Start()
    {
        _movement = GetComponent<PoliceMovement>();
        _items = GetComponent<ItemController>();
        _footstepSound = audioManager.GetSound("Footstep");
        _footstepSound2 = audioManager.GetSound("Footstep2");
    }

    private void Update()
    {
        if (_items.currentItem)
        {
            if (_items.currentItem.itemName == Item.ItemName.Baton)
            {
                animator.SetBool("walking_no_baton", false);
                animator.SetBool("walking", _movement.IsWalking());
            }
            else
            {
                animator.SetBool("walking", false);
                animator.SetBool("walking_no_baton", _movement.IsWalking());
            }
        }
        else
        {
            animator.SetBool("walking", _movement.IsWalking());
        }

        if (_movement.IsWalking() && !_footstepSound.isPlaying && !_footstepSound2.isPlaying)
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