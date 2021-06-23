using UnityEngine;

public class TestVRPlayer : MonoBehaviour
{
    public int resistancePoints;
    private Material material;
    
    // Start is called before the first frame update
    void Awake()
    {
        resistancePoints = 100;
        material = GetComponent<Renderer>().material;
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
        material.color = Random.ColorHSV(0f, 1f, 0.5f, 1f, 0f, 1f);
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
