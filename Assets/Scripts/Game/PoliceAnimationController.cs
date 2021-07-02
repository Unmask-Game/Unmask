using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class PoliceAnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    //[SerializeField] private AudioManager audioManager;

    private PoliceMovement _movement;

    private ItemController _items;

    //private AudioSource _footstepSound;
    //private AudioSource _footstepSound2;
    private bool _soundSwitchDone;

    private void Start()
    {
        _movement = GetComponent<PoliceMovement>();
        _items = GetComponent<ItemController>();
        //_footstepSound = audioManager.GetSound("Footstep");
        //_footstepSound2 = audioManager.GetSound("Footstep2");
    }

    private void Update()
    {
        if (_items.currentItem)
        {
            if (_items.currentItem.itemName == Item.ItemName.Baton)
            {
                animator.SetBool("walking_no_baton", false);
                animator.SetBool("walking", _movement.isWalking);
            }
            else
            {
                animator.SetBool("walking", false);
                animator.SetBool("walking_no_baton", _movement.isWalking);
            }
        }
        else
        {
            animator.SetBool("walking", _movement.isWalking);
            animator.SetBool("walking_no_baton", false);
        }
    }
}