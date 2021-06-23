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
        animator.SetBool("walking", _movement.IsWalking());
    }
}