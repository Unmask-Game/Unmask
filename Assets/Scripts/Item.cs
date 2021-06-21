using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public enum ItemType
    {
        Damage, Arrest 
    }
    
    public string itemName;
    public ItemType itemType;
    public string description;
    public Sprite image;
    public Animator animator;
    public Vector3 positionOnMap;
    public Quaternion originalRotation;
    public Vector3 originalScale;
    public Rigidbody itemBody;
    public BoxCollider itemCollider;
    public Vector3 equippedSize;

    private void Awake()
    {
        var self = transform;
        positionOnMap = self.localPosition;
        originalRotation = self.rotation;
        originalScale = self.localScale;
        itemBody = gameObject.GetComponent<Rigidbody>();
        itemCollider = itemBody.GetComponent<BoxCollider>();
        
    }
    
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
        
        //TODO: Change model when picked-up to another model while its equipped
        
        self.parent = parent;
        self.position = parent.position;
        self.rotation = parent.rotation;
        itemBody.isKinematic = true;
        itemCollider.isTrigger = false;
    }

    // Instantiate new GameObject on that position???
    public void OnDrop(Item otherItem)
    {
        var self = transform;
        var newItem = otherItem.transform;

        //TODO: Change model when dropped 
        
        self.parent = null;
        self.position = newItem.position;
        self.rotation = originalRotation;
        self.localScale = originalScale;
        itemBody.isKinematic = false;
        itemCollider.isTrigger = true;
    }
}