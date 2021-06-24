using System.Collections;
using UnityEngine;

public class HandcuffsScript : Item
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override IEnumerator Attack(Camera cam, Animator playerAnimator)
    {
        playerAnimator.SetLayerWeight(playerAnimator.GetLayerIndex("AttackLayer"), 1);
        playerAnimator.SetTrigger("melee_attack");
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


        /*equipPlaceCollider.isTrigger = true;
        yield return new WaitForSeconds(0.2f);
        equipPlaceCollider.isTrigger = false;
        */
        //playerAnimator.SetLayerWeight(playerAnimator.GetLayerIndex("AttackLayer"),0);
        // animator.doSomething();
        //Debug.Log("Attack");
        //TODO: Colliders / Raycast and stuff
    }
}
