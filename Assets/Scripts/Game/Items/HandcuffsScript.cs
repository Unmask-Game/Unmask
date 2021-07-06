using System.Collections;
using Photon.Pun;
using UnityEngine;
using static DefaultNamespace.Constants;

public class HandcuffsScript : Item
{
    private void Start()
    {
        itemName = ItemName.Handcuffs;
        itemType = ItemType.Arrest;
        Damage = 0;
        Range = HandcuffsRange;
    }

    public override IEnumerator Attack(ItemController itemController, Camera cam, Animator playerAnimator,
        AudioManager playerAudio, PhotonView view)
    {
        PlayAnimation(playerAnimator, playerAudio);
        // Send RPC to others to also play the item's animation ("PlayAnimation") for them 
        view.RPC("PlayItemAnimationRemote", RpcTarget.Others);
        yield return new WaitForSeconds(WaitForAnimationTime);

        TakeUnderArrest(itemController, cam);
    }

    public override void PlayAnimation(Animator playerAnimator, AudioManager playerAudio)
    {
        playerAnimator.SetTrigger("melee_attack");
        playerAudio.Play("Handcuffs");
    }
}