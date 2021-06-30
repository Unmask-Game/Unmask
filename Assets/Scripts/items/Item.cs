using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

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

    private Rigidbody _itemBody;
    private BoxCollider _itemCollider;
    private Animator _animator;
    protected const float WaitForAnimationTime = 0.3f;

    private void Awake()
    {
        var self = transform;
        positionOnMap = self.localPosition;
        originalRotation = self.rotation;
        originalScale = self.localScale;
        _itemBody = self.GetComponent<Rigidbody>();
        _itemCollider = _itemBody.GetComponent<BoxCollider>();
        _animator = self.GetComponent<Animator>();

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
            if (objectHit.CompareTag("TestVRPlayer"))
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
            if (objectHit.CompareTag("Player"))
            {
                objectHit.GetComponent<VRPlayerController>().TakeDamage(Damage);
                optionalSound?.Play();
            }
            else if (objectHit.CompareTag("NPC"))
            {
                itemController.AddNpcHitNotice(HitNpcCooldown);
                optionalSound?.Play();
            }
        }
    }

    public void OnPickUp(GameObject equipPlace)
    {
        var self = transform;
        var parent = equipPlace.transform;

        self.parent = parent;
        self.position = parent.position;
        self.rotation = parent.rotation;
        _itemBody.isKinematic = true;
        _itemCollider.isTrigger = false;

        _animator.enabled = false;
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

        _animator.enabled = true;
        onGroundModel.SetActive(true);
        equippedModel.SetActive(false);
    }
}