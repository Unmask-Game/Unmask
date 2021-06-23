using UnityEngine;

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
        //animator.SetLayerWeight(1,1);
        //animator.SetBool("baton_attack", _items.IsAttacking() == Item.ItemName.Baton);
        /*animator.SetBool("handcuffs_attack", _items.IsAttacking() == Item.ItemName.Handcuffs);
        animator.SetBool("lasso_attack", _items.IsAttacking() == Item.ItemName.Lasso);
        animator.SetBool("taser_attack", _items.IsAttacking() == Item.ItemName.Taser);
        */
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
    }
}