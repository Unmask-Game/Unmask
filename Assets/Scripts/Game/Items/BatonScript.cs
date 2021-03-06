using System.Collections;
using Photon.Pun;
using UnityEngine;
using static DefaultNamespace.Constants;

public class BatonScript : Item
{

    private void Start()
    {
        itemName = ItemName.Baton;
        itemType = ItemType.Damage;
        Damage = BatonDamage;
        Range = BatonRange;
    }

    public override IEnumerator Attack(ItemController itemController, Camera cam, Animator playerAnimator,
        AudioManager playerAudio, PhotonView view)
    {
        PlayAnimation(playerAnimator, playerAudio);
        // Send RPC to others to also play the item's animation ("PlayAnimation") for them
        view.RPC("PlayItemAnimationRemote", RpcTarget.Others);
        yield return new WaitForSeconds(WaitForAnimationTime);

        var hitSound = playerAudio.GetSound("BatonHit");
        hitSound.pitch = Random.Range(0.75f, 1.1f);
        InflictDamage(itemController, cam, Damage, Range, hitSound);
    }

    public override void PlayAnimation(Animator playerAnimator, AudioManager playerAudio)
    {
        playerAnimator.SetTrigger("melee_attack");
        var sound = playerAudio.GetSound("Baton");
        sound.pitch = Random.Range(1f, 1.3f);
        sound.Play();
    }
}