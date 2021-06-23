using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestVRPlayer : MonoBehaviour
{
    public int resistancePoints;
    
    // Start is called before the first frame update
    void Awake()
    {
        resistancePoints = 100;
    }

    private void OnTriggerEnter(Collider other)
    {
        /*if (!other.gameObject.CompareTag("Weapon")) return;
        resistancePoints -= other.gameObject.GetComponentInParent<ItemController>().currentItem.damage;
        Debug.Log("RP: "+resistancePoints);*/
    }

    public void TakeDamage(int damage)
    {
        resistancePoints -= damage;
        Debug.Log("Damn, I got hit for -" + damage+" .... Current RP: "+resistancePoints);
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
