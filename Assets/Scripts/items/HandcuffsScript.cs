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

    public override IEnumerator Attack(ItemController itemController, Camera cam, Animator playerAnimator,
        AudioManager playerAudio)
    {
        yield return new WaitForSeconds(WaitForAnimationTime);

        TakeUnderArrest(cam);
    }

    public override void PlayAnimation(Animator playerAnimator, AudioManager playerAudio)
    {
        playerAnimator.SetTrigger("melee_attack");
        playerAudio.Play("Handcuffs");
    }
}