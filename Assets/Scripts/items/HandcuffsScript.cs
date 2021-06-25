using System.Collections;
using UnityEngine;

public class HandcuffsScript : Item
{
    private void Start()
    {
        itemName = ItemName.Handcuffs;
        itemType = ItemType.Arrest;
        Damage = 0;
        Range = 0.2f;
    }

    public override IEnumerator Attack(Camera cam, Animator playerAnimator, AudioManager playerAudio)
    {
        playerAnimator.SetLayerWeight(playerAnimator.GetLayerIndex("AttackLayer"), 1);
        playerAnimator.SetTrigger("melee_attack");
        playerAudio.Play("Handcuffs");
        yield return new WaitForSeconds(0.2f);

        var ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hit, Range))
        {
            var objectHit = hit.collider.gameObject;
            if (objectHit.CompareTag("TestVRPlayer"))
            {
                objectHit.GetComponent<TestVRPlayer>().BeArrested();
            }
        }
    }
}
