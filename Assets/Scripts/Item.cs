using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public abstract class Item : MonoBehaviour
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
    public int damage;
    public float range;

    public GameObject onGroundModel;
    public GameObject equippedModel;
    
    private Rigidbody _itemBody;
    private BoxCollider _itemCollider;
    
    private void Awake()
    {
        var self = transform;
        positionOnMap = self.localPosition;
        originalRotation = self.rotation;
        originalScale = self.localScale;
        _itemBody = self.GetComponent<Rigidbody>();
        _itemCollider = _itemBody.GetComponent<BoxCollider>();
        
        onGroundModel.SetActive(true);
    }
    
    // TODO: Do Animation / Sound or something else
    public abstract IEnumerator Attack(Camera cam, Animator playerAnimator);

    // TODO: Do Animation / Sound or something else
    public void OnPickUp(GameObject equipPlace)
    {
        var self = transform;
        var parent = equipPlace.transform;
        
        self.parent = parent;
        self.position = parent.position;
        self.rotation = parent.rotation;
        _itemBody.isKinematic = true;
        _itemCollider.isTrigger = false;

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
        _itemBody.isKinematic = false;
        _itemCollider.isTrigger = true;
        
        onGroundModel.SetActive(true);
        equippedModel.SetActive(false);
    }
}