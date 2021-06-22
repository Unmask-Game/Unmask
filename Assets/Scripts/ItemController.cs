using UnityEngine;
using static Item.ItemType;

public class ItemController : MonoBehaviour
{
    [SerializeField] private GameObject itemPlace;
    [SerializeField] private HUDController hud;

    private Item _itemToBePickedUp;
    private Item _currentItem;
    private Item _damageSlot;
    private Item _arrestSlot;
    private KeyCode _pickUp;
    private KeyCode _attack;

    private const float PickUpCooldown = 0.4f;
    private const float SwitchCooldown = 0.1f;
    private const float AttackCooldown = 0.6f;

    private float _attackCooldownExpiry;
    private float _switchCooldownExpiry;
    private float _pickUpCooldownExpiry;

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

        if (!_currentItem) return;

        if (Time.time > _switchCooldownExpiry)
        {
            bool cooldownNeeded;
            // Switch items 
            if (Input.GetAxis("Mouse ScrollWheel") < 0f || Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                cooldownNeeded =
                    SetCurrentItem(ref _currentItem.itemType == Damage ? ref _arrestSlot : ref _damageSlot);
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
            {
                _switchCooldownExpiry = Time.time + SwitchCooldown;
            }
        }

        // Attack with item (maybe in FixedUpdate?)
        if (Time.time < _attackCooldownExpiry) return;
        if (!Input.GetKeyDown(_attack)) return;
        _currentItem.Attack();
        _attackCooldownExpiry = Time.time + AttackCooldown;
    }
    
    public Item.ItemName? IsAttacking()
    {
        if (Time.time <= 0) return null;
        if (Time.time <= _attackCooldownExpiry)
        {
            return _currentItem.itemName;
        }
        else
        {
            return null;
        }
    }

    private bool SetCurrentItem(ref Item slot)
    {
        if (!slot || slot == _currentItem) return false;

        if (_currentItem)
        {
            _currentItem.gameObject.SetActive(false);
            _currentItem = slot;
            slot.gameObject.SetActive(true);
        }
        else
        {
            _currentItem = slot;
        }

        hud.DeselectSlot(slot.itemType == Damage ? 1 : 0);
        hud.SelectSlot(slot.itemType == Damage ? 0 : 1);
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
}