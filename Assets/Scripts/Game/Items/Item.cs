using System.Collections;
using DefaultNamespace;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;
using static DefaultNamespace.Tags;

public abstract class Item : MonoBehaviour
{
    public enum ItemType
    {
        Damage,
        Arrest
    }

    public enum ItemName
    {
        Baton,
        Taser,
        Lasso,
        Handcuffs
    }

    [HideInInspector] public ItemName itemName;
    [HideInInspector] public ItemType itemType;
    // Used for the selection in the HUD to display current item (e.g.: Image of a Taser)
    public Sprite sprite;

    public Vector3 positionOnMap;
    public Quaternion originalRotation;
    public Vector3 originalScale;
    protected int Damage;
    protected float Range;

    // Models that will be switched on pick-up and drop
    public GameObject onGroundModel;
    public GameObject equippedModel;

    public Rigidbody itemBody;
    public BoxCollider itemCollider;
    public Animator animator;
    // Delay actual hit and animation, so hitting someone will be synced to animation
    protected const float WaitForAnimationTime = 0.3f;

    private void Awake()
    {
        var self = transform;
        positionOnMap = self.localPosition;
        originalRotation = self.rotation;
        originalScale = self.localScale;
        itemBody = self.GetComponent<Rigidbody>();
        itemCollider = itemBody.GetComponent<BoxCollider>();
        animator = self.GetComponent<Animator>();

        // When an item gets instantiated only show the on ground model
        onGroundModel.SetActive(true);
    }

    // Animator and audio manager as parameters to play sound/animation, also transfer the PhotonView to sync item attack with others
    public abstract IEnumerator Attack(ItemController itemController, Camera cam, Animator playerAnimator,
        AudioManager playerAudio, PhotonView view);

    // Called when player attacks with the handcuffs
    protected void TakeUnderArrest(ItemController itemController, Camera playerCam)
    {
        // Check if hit is in range via raycast from the current mouse position
        var ray = playerCam.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out var hit, Range))
        {
            var objectHit = hit.collider.gameObject;
            // If mask thief was hit
            if (objectHit.CompareTag(VrPlayerTag))
            {
                var vrPlayer = objectHit.GetComponent<VRPlayerController>();
                if (vrPlayer.isArrestable)
                {
                    vrPlayer.BeArrested();
                }
                else
                {
                    // Show info bubble that you have to deal damage first before you can arrest the mask thief
                    itemController.ShowInfoBubble(itemController.CannotArrestText, Constants.ShowItemInfoBubbleTime);
                }
            }
        }
    }

    // Called when player attacks with the taser or baton
    protected void InflictDamage(ItemController itemController, Camera playerCam, int damage, float range,
        AudioSource optionalSound)
    {
        // Check if hit is in range via raycast from the current mouse position
        var ray = playerCam.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out var hit, Range))
        {
            var objectHit = hit.collider.gameObject;
            // If mask thief was hit
            if (objectHit.CompareTag(VrPlayerTag))
            {
                objectHit.GetComponent<VRPlayerController>().TakeDamage(Damage, itemName);
                // When hitting someone also play sound
                optionalSound?.Play();
            }
            // If NPC was hit
            else if (objectHit.CompareTag(NpcTag))
            {
                // Cooldown player for hitting an NPC
                itemController.AddNpcHitNotice(Constants.HitNpcCooldown);
                optionalSound?.Play();
            }
        }
    }

    public abstract void PlayAnimation(Animator playerAnimator,
        AudioManager playerAudio);

    // The item will be transfered to the equip place
    public void OnPickUp(GameObject equipPlace)
    {
        // Add item as child to the equip place
        // And transform position, rotation according to it
        var self = transform;
        var parent = equipPlace.transform;
        self.parent = parent;
        self.position = parent.position;
        self.rotation = parent.rotation;
        
        // Disable colliders and own animation (item is normally rotating slowly on the ground)
        itemBody.isKinematic = true;
        ToggleCollider(false);
        animator.enabled = false;
        
        // Change models (the equipped model most likely appears to be smaller compared to on ground)
        onGroundModel.SetActive(false);
        equippedModel.SetActive(true);
    }

    // The item will be dropped on the ground by switching it's place with a new item
    // (You can only drop items by picking up a new one that fits the type (Damage/Arrest))
    public void OnDrop(Item otherItem)
    {
        // Remove item's parent
        // And swap item rotation and position with that of the other item's
        var self = transform;
        var newItem = otherItem.transform;
        self.parent = null;
        self.position = newItem.position;
        self.rotation = originalRotation;
        
        // Reset to original scale (on ground item model is usually bigger)
        self.localScale = originalScale;
        
        // Enable colliders and own animation
        itemBody.isKinematic = false;
        ToggleCollider(true);
        animator.enabled = true;
        
        // Change models
        onGroundModel.SetActive(true);
        equippedModel.SetActive(false);
    }

    // For enabling collider when item is on ground so it can be picked-up
    public void ToggleCollider(bool state)
    {
        if (state == false)
        {
            itemCollider.isTrigger = false;
            itemCollider.enabled = false;
        }
        else
        {
            itemCollider.enabled = true;
            itemCollider.isTrigger = true;
        }
    }
}