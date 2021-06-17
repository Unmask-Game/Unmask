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

    private void Start()
    {
        // make this changeable in settings
        _pickUp = KeyCode.E;
        _attack = KeyCode.Mouse0;
    }

    private void Update()
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
        }

        if (!_currentItem) return;

        // Switch items 
        if (Input.GetAxis("Mouse ScrollWheel") < 0f || Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            SetCurrentItem(ref _currentItem.itemType == Damage ? ref _arrestSlot : ref _damageSlot);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetCurrentItem(ref _damageSlot);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetCurrentItem(ref _arrestSlot);
        }
        
        // Attack with item (maybe in FixedUpdate?)
        if (Input.GetKeyDown(_attack))
        {
            _currentItem.Attack();
        }
    }

    private void SetCurrentItem(ref Item slot)
    {
        if (!slot) return;
        // vllt auch: || slot == _currentItem
        
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