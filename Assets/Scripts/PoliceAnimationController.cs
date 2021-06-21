using UnityEngine;

public class PoliceAnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private PlayerMovement _movement;
    private ItemController _items;

    private void Start()
    {
        _movement = GetComponent<PlayerMovement>();
        _items = GetComponent<ItemController>();
    }

    private void Update()
    {
        animator.SetBool("attacking", _items.IsAttacking());
        animator.SetBool("walking", _movement.IsWalking());
    }
}