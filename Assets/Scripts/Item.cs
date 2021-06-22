using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public enum ItemType
    {
        Damage, Arrest 
    }
    
    public enum ItemName
    {
        Baton, Taser, Lasso, Handcuffs
    }

    public ItemName itemName;
    public ItemType itemType;
    public Sprite sprite;

    public Vector3 positionOnMap;
    public Quaternion originalRotation;
    public Vector3 originalScale;
    public Rigidbody itemBody;
    public BoxCollider itemCollider;

    public GameObject onGroundModel;
    public GameObject equippedModel;

    private void Awake()
    {
        var self = transform;
        positionOnMap = self.localPosition;
        originalRotation = self.rotation;
        originalScale = self.localScale;
        itemBody = self.GetComponent<Rigidbody>();
        itemCollider = itemBody.GetComponent<BoxCollider>();
        
        onGroundModel.SetActive(true);
    }
    
    // TODO: Do Animation / Sound or something else
    public void Attack()
    {
        // animator.doSomething();
        Debug.Log("Attack");
        //TODO: Colliders / Raycast and stuff
    }
    
    // TODO: Do Animation / Sound or something else
    public void OnPickUp(GameObject equipPlace)
    {
        var self = transform;
        var parent = equipPlace.transform;
        
        self.parent = parent;
        self.position = parent.position;
        self.rotation = parent.rotation;
        itemBody.isKinematic = true;
        itemCollider.isTrigger = false;
        
        onGroundModel.SetActive(false);
        equippedModel.SetActive(true);
    }

    public void OnDrop(Item otherItem)
    {
        
        var self = transform;
        var newItem = otherItem.transform;

        self.parent = null;
        self.position = newItem.position;
        self.rotation = originalRotation;
        self.localScale = originalScale;
        itemBody.isKinematic = false;
        itemCollider.isTrigger = true;
        
        onGroundModel.SetActive(true);
        equippedModel.SetActive(false);
    }
}