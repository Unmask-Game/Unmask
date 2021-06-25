using System.Collections;
using UnityEngine;

public class BatonScript : Item
{
    private void Start()
    {
        itemName = ItemName.Baton;
        itemType = ItemType.Damage;
        Damage = 20;
        Range = 0.7f;
    }

    public override IEnumerator Attack(Camera cam, Animator playerAnimator, AudioManager playerAudio)
    {
        playerAnimator.SetLayerWeight(playerAnimator.GetLayerIndex("AttackLayer"), 1);
        playerAnimator.SetTrigger("melee_attack");
        playerAudio.Play("Baton");
        yield return new WaitForSeconds(0.2f);

        var ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hit, Range))
        {
            var objectHit = hit.collider.gameObject;
            if (objectHit.CompareTag("TestVRPlayer"))
            {
                objectHit.GetComponent<TestVRPlayer>().TakeDamage(Damage);
            }
        }
    }
}