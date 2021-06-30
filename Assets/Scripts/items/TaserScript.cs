using System.Collections;
using Photon.Pun;
using UnityEngine;

public class TaserScript : Item
{
    private ParticleSystem _particleEffect;

    private void Start()
    {
        itemName = ItemName.Taser;
        itemType = ItemType.Damage;
        Damage = 10;
        Range = 1.4f;
        _particleEffect = GetComponentInChildren<ParticleSystem>();
    }

    public override IEnumerator Attack(ItemController itemController, Camera cam, Animator playerAnimator,
        AudioManager playerAudio,PhotonView view)
    {
        PlayAnimation(playerAnimator, playerAudio);
        view.RPC("PlayItemAnimationRemote", RpcTarget.Others);
        yield return new WaitForSeconds(0f);
        InflictDamage(itemController, cam, Damage, Range, null);
    }
    
    public override void PlayAnimation(Animator playerAnimator, AudioManager playerAudio)
    {
        playerAnimator.SetTrigger("taser_attack");
        playerAudio.Play("Taser");
        _particleEffect.Play();
    }
}