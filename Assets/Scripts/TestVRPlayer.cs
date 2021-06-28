using System;
using UnityEngine;
using Random = UnityEngine.Random;
public class TestVRPlayer : MonoBehaviour
{
    public int resistancePoints;
    private Material material;

    // Start is called before the first frame update
    private void Awake()
    {
        resistancePoints = 100;
        material = GetComponent<Renderer>().material;
    }

    private void Update()
    {
    }

    public void TakeDamage(int damage)
    {
        resistancePoints -= damage;
        Debug.Log("Damn, I got hit for -" + damage + " .... Current RP: " + resistancePoints);
        material.color = Random.ColorHSV(0f, 1f, 0.5f, 1f, 0.5f, 1f);
    }

    public void BeArrested()
    {
        if (resistancePoints > 0) return;
        Debug.Log("Damn, I've been arrested");
        Destroy(gameObject);
    }

    // called when player is hit by lasso
    public void BeSlowedDown(float time)
    {
        Debug.Log("Damn, I've been slowed down");
    }
}