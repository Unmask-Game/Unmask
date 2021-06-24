using System.Collections;
using UnityEngine;

public class TaserScript : Item
{
    [SerializeField] private ParticleSystem particleEffect;
    
    public override IEnumerator Attack(Camera cam, Animator playerAnimator)
    {
        playerAnimator.SetLayerWeight(playerAnimator.GetLayerIndex("AttackLayer"), 1);
        playerAnimator.SetTrigger("taser_attack");
        particleEffect.Play();
        yield return new WaitForSeconds(0.2f);

        var ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, range))
        {
            var objectHit = hit.collider.gameObject;
            if (objectHit.CompareTag("TestVRPlayer"))
            {
                objectHit.GetComponent<TestVRPlayer>().TakeDamage(damage);
            }
        }
    }
}