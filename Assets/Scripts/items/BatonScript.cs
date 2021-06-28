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

    public override IEnumerator Attack(ItemController itemController, Camera cam, Animator playerAnimator,
        AudioManager playerAudio)
    {
        //playerAnimator.SetLayerWeight(playerAnimator.GetLayerIndex("AttackLayer"), 1);
        playerAnimator.SetTrigger("melee_attack");
        var sound = playerAudio.GetSound("Baton");
        sound.pitch = Random.Range(1f, 1.3f);
        sound.Play();

        yield return new WaitForSeconds(WaitForAnimationTime);

        var hitSound = playerAudio.GetSound("BatonHit");
        hitSound.pitch = Random.Range(0.75f, 1.1f);
        InflictDamage(itemController, cam, Damage, Range, hitSound);
    }
}