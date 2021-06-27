using System.Collections;
using UnityEngine;

public class LassoScript : Item
{
    private void Start()
    {
        itemName = ItemName.Lasso;
        itemType = ItemType.Arrest;
        Damage = 0;
        Range = 1.2f;
    }

    public override IEnumerator Attack(ItemController itemController, Camera cam, Animator playerAnimator, AudioManager playerAudio)
    {
        playerAnimator.SetLayerWeight(playerAnimator.GetLayerIndex("AttackLayer"), 1);
        //playerAnimator.SetTrigger("lasso_attack");
        playerAudio.Play("Lasso");
        
        yield return new WaitForSeconds(WaitForAnimationTime);

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