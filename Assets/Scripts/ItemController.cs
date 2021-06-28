using System;
using UnityEngine;
using UnityEngine.UI;
using static Item.ItemType;

public class ItemController : MonoBehaviour
{
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

    private KeyCode _pickUp;
    private KeyCode _attack;

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
    
    private void Start()
    {
        // make this changeable in settings
        _pickUp = KeyCode.E;
        _attack = KeyCode.Mouse0;
    }

    private void Update()
    {
        if (Time.time > _pickUpCooldownExpiry)
        {
            // Item pick-up
            if (_itemToBePickedUp && Input.GetKeyDown(_pickUp))
            {
                // Check Type of Item (Arrest/Damage Type)
                ref var slot = ref _itemToBePickedUp.itemType == Damage ? ref _damageSlot : ref _arrestSlot;

                if (slot)
                {
                    slot.OnDrop(_itemToBePickedUp);
                    var dropped = slot;
                    slot = _itemToBePickedUp;
                    slot.OnPickUp(itemPlace);
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
                    SetCurrentItem(ref slot);
                }

                _pickUpCooldownExpiry = Time.time + PickUpCooldown;
            }
        }

        if (!currentItem) return;

        // Switch items 
        if (Time.time > _switchCooldownExpiry)
        {
            bool cooldownNeeded;
            if (Input.GetAxis("Mouse ScrollWheel") < 0f || Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                cooldownNeeded =
                    SetCurrentItem(ref currentItem.itemType == Damage ? ref _arrestSlot : ref _damageSlot);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                cooldownNeeded = SetCurrentItem(ref _damageSlot);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                cooldownNeeded = SetCurrentItem(ref _arrestSlot);
            }
            else
            {
                cooldownNeeded = false;
            }

            if (cooldownNeeded)
                _switchCooldownExpiry = Time.time + SwitchCooldown;
        }

        UpdateCooldownNotice();

        if (Time.time < _attackCooldownExpiry) return;
        if (!Input.GetKeyDown(_attack)) return;
        // Slowdown if attacking ? 
        //playerMovement.SetTemporarySpeed(playerMovement.playerSpeed / 1.5f, AttackCooldown);
        StartCoroutine(currentItem.Attack(this, playerCam, playerAnimator, audioManager));
        // Cooldowns, so you can't switch items while attacking etc. (seperated CooldownAfterAttack variable)
        _pickUpCooldownExpiry = _switchCooldownExpiry = Time.time + CooldownAfterAttack;
        _attackCooldownExpiry = Time.time + AttackCooldown;
    }

    /*public Item.ItemName? IsAttacking()
    {
        if (Time.time <= 0) return null;
        if (Time.time <= _attackCooldownExpiry)
        {
            return currentItem.itemName;
        }

        return null;
    }*/

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
        _pickUpCooldownExpiry = _switchCooldownExpiry  = Time.time + timeOthersCooldown;
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