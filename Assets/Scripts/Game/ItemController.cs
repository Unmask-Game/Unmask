using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static Item.ItemType;

public class ItemController : MonoBehaviour
{
    private ItemSpawner _itemSpawner;

    [SerializeField] private GameObject itemPlace;

    [SerializeField] private HUDController hud;
    [SerializeField] private Image damageSlotImage;
    [SerializeField] private Image arrestSlotImage;

    //[SerializeField] private PoliceMovement playerMovement;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private Camera playerCam;

    private Item _itemToBePickedUp;
    [HideInInspector] public Item currentItem;

    private Item _damageSlot;
    private Item _arrestSlot;

    private const float PickUpCooldown = 0.4f;
    private const float SwitchCooldown = 0.1f;
    private const float AttackCooldown = 1.2f;
    private const float CooldownAfterAttack = 0.9f;

    private float _attackCooldownExpiry;
    private float _switchCooldownExpiry;
    private float _pickUpCooldownExpiry;
    private float _cooldwonNoticeExpiry;

    private const string NpcCooldownText = "Stop hitting innocent bystanders!\n(Cooldown remaining: ";
    private const string StandardCooldownText = "Using the lasso results in\na cooldown. (Remaining: ";
    private string _currentCooldownText;

    private PhotonView _view;

    private void Start()
    {
        _view = GetComponent<PhotonView>();
        _itemSpawner = GameObject.FindWithTag("ItemSpawner").GetComponent<ItemSpawner>();
    }

    public void PickUpItem(InputAction.CallbackContext context)
    {
        if (context.ReadValueAsButton())
        {
            if (Time.time > _pickUpCooldownExpiry)
            {
                // Item pick-up
                if (_itemToBePickedUp)
                {
                    // Check Type of Item (Arrest/Damage Type)
                    ref var slot = ref _itemToBePickedUp.itemType == Damage ? ref _damageSlot : ref _arrestSlot;

                    if (slot)
                    {
                        slot.OnDrop(_itemToBePickedUp);
                        var dropped = slot;
                        slot = _itemToBePickedUp;
                        slot.OnPickUp(itemPlace);
                        OnTriggerExit(slot.itemCollider);
                        SetCurrentItem(ref slot);
                        if (!dropped.gameObject.activeSelf)
                        {
                            dropped.gameObject.SetActive(true);
                        }
                    }
                    else
                    {
                        slot = _itemToBePickedUp;
                        slot.OnPickUp(itemPlace);
                        OnTriggerExit(slot.itemCollider);
                        SetCurrentItem(ref slot);
                    }

                    _pickUpCooldownExpiry = Time.time + PickUpCooldown;
                }
            }
        }
    }

    [PunRPC]
    public void SetItemRemote(Item.ItemName itemName)
    {
        foreach (var item in _itemSpawner.itemPool)
        {
            if (item.GetComponent<Item>().itemName == itemName)
            {
                var currentItem = GetCurrentItem();
                if (currentItem)
                {
                    Destroy(currentItem.gameObject);
                }

                var itemObject = Instantiate(item);
                var spawnedItem = itemObject.GetComponent<Item>();
                var self = spawnedItem.transform;
                var parent = itemPlace.transform;

                self.parent = parent;
                self.position = parent.position;
                self.rotation = parent.rotation;

                spawnedItem.itemBody.isKinematic = true;
                spawnedItem.ToggleCollider(false);

                spawnedItem.animator.enabled = false;
                spawnedItem.onGroundModel.SetActive(false);
                spawnedItem.equippedModel.SetActive(true);
            }
        }
    }

    [PunRPC]
    public void PlayItemAnimationRemote()
    {
        var currentItem = GetCurrentItem();
        if (currentItem)
        {
            currentItem.PlayAnimation(playerAnimator, audioManager);
        }
    }

    private Item GetCurrentItem()
    {
        return currentItem = itemPlace.GetComponentInChildren<Item>();
    }

    public void SwitchAction(InputAction.CallbackContext context)
    {
        if (!currentItem) return;
        if (context.ReadValue<float>() > 0)
        {
            Switch(1);
        }
        else if (context.ReadValue<float>() < 0)
        {
            Switch(-1);
        }
    }

    private void Switch(int switchDirection)
    {
        if (Time.time > _switchCooldownExpiry)
        {
            var cooldownNeeded = false;
            if (switchDirection == 1)
            {
                cooldownNeeded = SetCurrentItem(ref _damageSlot);
            }
            else if (switchDirection == -1)
            {
                cooldownNeeded = SetCurrentItem(ref _arrestSlot);
            }

            if (cooldownNeeded)
                _switchCooldownExpiry = Time.time + SwitchCooldown;
        }
    }

    public void AttackAction(InputAction.CallbackContext context)
    {
        if (!currentItem) return;
        if (Time.time >= _attackCooldownExpiry && context.ReadValueAsButton())
        {
            // Slowdown if attacking ? 
            //playerMovement.SetTemporarySpeed(playerMovement.playerSpeed / 1.5f, AttackCooldown);
            //currentItem.PlayAnimation(playerAnimator, audioManager);
            //_view.RPC("PlayItemAnimationRemote", RpcTarget.Others);
            StartCoroutine(currentItem.Attack(this, playerCam, playerAnimator, audioManager, _view));
            // Cooldowns, so you can't switch items while attacking etc. (seperated CooldownAfterAttack variable)
            _pickUpCooldownExpiry = _switchCooldownExpiry = Time.time + CooldownAfterAttack;
            _attackCooldownExpiry = Time.time + AttackCooldown;
        }
    }

    private void Update()
    {
        if (!currentItem) return;
        UpdateCooldownNotice();
    }

    private bool SetCurrentItem(ref Item slot)
    {
        if (!slot || slot == currentItem) return false;

        if (currentItem)
        {
            currentItem.gameObject.SetActive(false);
            currentItem = slot;
            slot.gameObject.SetActive(true);
        }
        else
        {
            currentItem = slot;
        }

        if (slot.itemType == Damage)
        {
            damageSlotImage.sprite = currentItem.sprite;
            damageSlotImage.gameObject.SetActive(true);
            damageSlotImage.CrossFadeAlpha(1f, 0.2f, true);
            arrestSlotImage.CrossFadeAlpha(0.3f, 0.2f, true);
            hud.DeselectSlot(1);
            hud.SelectSlot(0);
        }
        else
        {
            arrestSlotImage.sprite = currentItem.sprite;
            arrestSlotImage.gameObject.SetActive(true);
            arrestSlotImage.CrossFadeAlpha(1f, 0.2f, true);
            damageSlotImage.CrossFadeAlpha(0.3f, 0.2f, true);
            hud.DeselectSlot(0);
            hud.SelectSlot(1);
        }

        _view.RPC("SetItemRemote", RpcTarget.Others, slot.itemName);
        return true;
    }

    private void OnTriggerEnter(Collider other)
    {
        var item = other.GetComponent<Item>();
        if (!item) return;
        _itemToBePickedUp = item;
        hud.OpenMessagePanel();
    }

    private void OnTriggerExit(Collider other)
    {
        var item = other.GetComponent<Item>();
        if (!item) return;
        _itemToBePickedUp = null;
        hud.CloseMessagePanel();
    }

    private void UpdateCooldownNotice()
    {
        if (Time.time < _cooldwonNoticeExpiry)
        {
            hud.ShowCooldownNotice((int) Math.Round(_cooldwonNoticeExpiry - Time.time), _currentCooldownText);
        }
        else
        {
            hud.CloseCooldownNotice();
        }
    }

    // OthersCooldown -> Cooldown for picking-up & switching items
    public void CooldownAllItems(float timeAttackCooldown, float timeOthersCooldown)
    {
        _pickUpCooldownExpiry = _switchCooldownExpiry = Time.time + timeOthersCooldown;
        _attackCooldownExpiry = Time.time + timeAttackCooldown;
        AddAntiWeaponSpamNotice(timeAttackCooldown);
    }

    private void AddAntiWeaponSpamNotice(float addSeconds)
    {
        _cooldwonNoticeExpiry = Time.time + addSeconds;
        _currentCooldownText = StandardCooldownText;
        hud.cooldownNoticeColor = hud.infoColor;
        hud.ShowCooldownNotice((int) Math.Round(addSeconds), _currentCooldownText);
    }

    public void AddNpcHitNotice(float addSeconds)
    {
        _cooldwonNoticeExpiry = _attackCooldownExpiry = Time.time + addSeconds;
        _currentCooldownText = NpcCooldownText;
        hud.cooldownNoticeColor = hud.alertColor;
        hud.ShowCooldownNotice((int) Math.Round(addSeconds), _currentCooldownText);
    }
}