using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class PoliceAnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private PoliceMovement _movement;
    private ItemController _items;

    private void Start()
    {
        _movement = GetComponent<PoliceMovement>();
        _items = GetComponent<ItemController>();
    }

    private void Update()
    {
        // If item in hand -> choose correct walking animation to be displayed
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