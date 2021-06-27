using System.Collections;
using UnityEngine;

public class TaserScript : Item
{
    [SerializeField] private ParticleSystem particleEffect;

    private void Start()
    {
        itemName = ItemName.Taser;
        itemType = ItemType.Damage;
        Damage = 10;
        Range = 1.4f;
    }

    public override IEnumerator Attack(ItemController itemController, Camera cam, Animator playerAnimator,
        AudioManager playerAudio)
    {
       // playerAnimator.SetLayerWeight(playerAnimator.GetLayerIndex("AttackLayer"), 1);
        playerAnimator.SetTrigger("taser_attack");
        playerAudio.Play("Taser");
        particleEffect.Play();

        yield return new WaitForSeconds(0f);

        InflictDamage(itemController, cam, Damage, Range, null);
    }
}