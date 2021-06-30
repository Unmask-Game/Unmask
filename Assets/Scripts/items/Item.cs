using System.Collections;
using DefaultNamespace;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;
using static DefaultNamespace.Constants;

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

    protected const float HitNpcCooldown = 15f;

    [HideInInspector] public ItemName itemName;
    [HideInInspector] public ItemType itemType;
    public Sprite sprite;

    public Vector3 positionOnMap;
    public Quaternion originalRotation;
    public Vector3 originalScale;
    protected int Damage;
    protected float Range;

    public GameObject onGroundModel;
    public GameObject equippedModel;

    public Rigidbody itemBody;
    public BoxCollider itemCollider;
    public Animator animator;
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

        onGroundModel.SetActive(true);
    }

    public abstract IEnumerator Attack(ItemController itemController, Camera cam, Animator playerAnimator,
        AudioManager playerAudio);

    protected void TakeUnderArrest(Camera playerCam)
    {
        var ray = playerCam.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out var hit, Range))
        {
            var objectHit = hit.collider.gameObject;
            if (objectHit.CompareTag(VrPlayerTag))
            {
                objectHit.GetComponent<VRPlayerController>().BeArrested();
            }
        }
    }

    protected void InflictDamage(ItemController itemController, Camera playerCam, int damage, float range,
        AudioSource optionalSound)
    {
        var ray = playerCam.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out var hit, Range))
        {
            var objectHit = hit.collider.gameObject;
            if (objectHit.CompareTag(VrPlayerTag))
            {
                objectHit.GetComponent<VRPlayerController>().TakeDamage(Damage);
                optionalSound?.Play();
            }
            else if (objectHit.CompareTag(NpcTag))
            {
                itemController.AddNpcHitNotice(HitNpcCooldown);
                optionalSound?.Play();
            }
        }
    }
    
    public abstract void PlayAnimation(Animator playerAnimator,
        AudioManager playerAudio);

    public void OnPickUp(GameObject equipPlace)
    {
        var self = transform;
        var parent = equipPlace.transform;

        self.parent = parent;
        self.position = parent.position;
        self.rotation = parent.rotation;
        itemBody.isKinematic = true;
        itemCollider.isTrigger = false;

        animator.enabled = false;
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

        animator.enabled = true;
        onGroundModel.SetActive(true);
        equippedModel.SetActive(false);
    }
}