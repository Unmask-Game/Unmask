using System;
using System.Collections;
using System.Collections.Generic;
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

    [SerializeField] private Animator playerAnimator;
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private Camera playerCam;

    private Item _itemToBePickedUp;
    [HideInInspector] public Item currentItem;

    // Two slots where 2 items of different types can be stored
    private Item _damageSlot;
    private Item _arrestSlot;

    private const float PickUpCooldown = 0.4f;
    private const float SwitchCooldown = 0.1f;
    private const float AttackCooldown = 1.2f;
    private const float CooldownAfterAttack = 0.9f;

    private float _attackCooldownExpiry;
    private float _switchCooldownExpiry;
    private float _pickUpCooldownExpiry;
    private float _cooldownNoticeExpiry;

    private PhotonView _view;

    // Text for Cooldown Notices and Info Bubbles
    public const string NpcCooldownText = "Stop hitting innocent bystanders!\n(Cooldown remaining: ";
    public const string StandardCooldownText = "Using the lasso results in\na cooldown. (Remaining: ";

    public const string ItemInfoText =
        "Oh no, there is someone \nstealing the masks of customers...\n-\nSearch for items to exhaust\n and arrest the thief!";

    public string CannotArrestText =
        "You cannot arrest the thief yet.\nExhaust him first using items\nthat deal damage!";

    private string _currentCooldownText;


    private void Start()
    {
        _view = GetComponent<PhotonView>();
        _itemSpawner = GameObject.FindWithTag("ItemSpawner").GetComponent<ItemSpawner>();
        
        // Show introduction when first starting the game 
        if (SettingsManager.Instance.GetShowItemInfo())
        {
            hud.OpenInfoBubble(ItemInfoText);
        }
        else
        {
            hud.CloseInfoBubble();
        }
    }
    
    private void Update()
    {
        // There can only be a cooldown applied if the player has an item (hit NPC with it e.g.)
        if (!currentItem) return;
        UpdateCooldownNotice();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        // Check if the player entered the collider of an item
        var item = other.GetComponent<Item>();
        if (!item) return;
        
        // When yes, the item can be picked up and it opens a panel so the player can react
        _itemToBePickedUp = item;
        hud.OpenMessagePanel();
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if player left an item collider
        var item = other.GetComponent<Item>();
        if (!item) return;
        
        // Item should not be able to be picked up anymore and the panel should disappear
        _itemToBePickedUp = null;
        hud.CloseMessagePanel();
    }

    public void PickUpItem(InputAction.CallbackContext context)
    {
        if (context.ReadValueAsButton())
        {
            // Player can only pick-up an item if the cooldown has expired
            if (Time.time > _pickUpCooldownExpiry)
            {
                // Variable is not null when the player entered some items collider range
                if (_itemToBePickedUp)
                {
                    // Check Type of Item (Either Arrest or Damage Type)
                    ref var slot = ref _itemToBePickedUp.itemType == Damage ? ref _damageSlot : ref _arrestSlot;
                    if (slot)
                    {
                        // Drop item if the slot is already full (only 1 item per type can be carried)
                        slot.OnDrop(_itemToBePickedUp);
                        var dropped = slot;
                        slot = _itemToBePickedUp;
                        slot.OnPickUp(itemPlace);
                        
                        // Stop showing that an item can be picked up
                        OnTriggerExit(slot.itemCollider);
                        SetCurrentItem(ref slot);
                        
                        // Reactivate the dropped item on the ground, so it could be picked up again
                        if (!dropped.gameObject.activeSelf)
                        {
                            dropped.gameObject.SetActive(true);
                        }
                    }
                    else
                    {
                        // When the slot is empty, do above minus the dropping part
                        slot = _itemToBePickedUp;
                        slot.OnPickUp(itemPlace);
                        OnTriggerExit(slot.itemCollider);
                        SetCurrentItem(ref slot);
                    }

                    // Disable introduction on how to play the game 
                    SettingsManager.Instance.SetShowItemInfo(false);
                    hud.CloseInfoBubble();
                    
                    // Apply cooldown for picking up an item
                    _pickUpCooldownExpiry = Time.time + PickUpCooldown;
                }
            }
        }
    }

    // Sync items for others as well when a player picks-up or switches his items
    [PunRPC]
    public void SetItemRemote(Item.ItemName itemName)
    {
        // Cycle through each item prefabs
        foreach (var item in _itemSpawner.itemPool)
        {
            // If the prefab matches the item to be displayed
            if (item.GetComponent<Item>().itemName == itemName)
            {
                // If the player already appears to have an item, destroy it
                var currentItem = GetCurrentItem();
                if (currentItem)
                {
                    Destroy(currentItem.gameObject);
                }
                
                // Instantiate the new item for others to see (same logic as in the Item.cs script OnPickUp)
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

    // Called by each item to play it's animation for others to see
    [PunRPC]
    public void PlayItemAnimationRemote()
    {
        var currentItem = GetCurrentItem();
        if (currentItem)
        {
            currentItem.PlayAnimation(playerAnimator, audioManager);
        }
    }

    // Used to see which item the player appears to have equipped in multiplayer
    private Item GetCurrentItem()
    {
        return currentItem = itemPlace.GetComponentInChildren<Item>();
    }

    // Called when the player attempts to switch items via mouse wheel or the keys 1 and 2
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
        // Player can only switch if the cooldown has expired
        if (Time.time > _switchCooldownExpiry)
        {
            // Used to see if player really switched (can't if he has no current item or only 1)
            var cooldownNeeded = false;
            
            // Switch current item to Damage Slot on 1 and Arrest Slot on 2
            if (switchDirection == 1)
            {
                cooldownNeeded = SetCurrentItem(ref _damageSlot);
            }
            else if (switchDirection == -1)
            {
                cooldownNeeded = SetCurrentItem(ref _arrestSlot);
            }

            // Apply cooldown for switching items if needed
            if (cooldownNeeded)
                _switchCooldownExpiry = Time.time + SwitchCooldown;
        }
    }

    // Called when player presses left mouse button or attack button
    public void AttackAction(InputAction.CallbackContext context)
    {
        if (!currentItem) return;
        // Player can only attack if the cooldown has expired and he presses the attack button
        if (Time.time >= _attackCooldownExpiry && context.ReadValueAsButton())
        {
            // Call item attack and give item the animator and audio manager to play sound/animation
            // Also transfer the PhotonView to sync item attack with others 
            StartCoroutine(currentItem.Attack(this, playerCam, playerAnimator, audioManager, _view));
            
            // Cooldowns, so you can't switch items while attacking etc. (seperated CooldownAfterAttack variable)
            _pickUpCooldownExpiry = _switchCooldownExpiry = Time.time + CooldownAfterAttack;
            _attackCooldownExpiry = Time.time + AttackCooldown;
        }
    }
    
    // Called when switching or picking-up items 
    private bool SetCurrentItem(ref Item slot)
    {
        // Return false if the slot is empty or the current item is already on the slot
        if (!slot || slot == currentItem) return false;

        if (currentItem)
        {
            // Set current item to not active
            currentItem.gameObject.SetActive(false);
            
            // Set the item to appear in the slot as new current item and activate it
            currentItem = slot;
            slot.gameObject.SetActive(true);
        }
        else
        {
            // If the player has not picked-up an item yet, set the current item to the slot
            currentItem = slot;
        }

        // There are 2 slots (one for the damage type items and one for arrest type)
        if (slot.itemType == Damage)
        {
            // Set currently displayed image of the slot to the new item's image
            // And set the slot to be selected for the player to see
            damageSlotImage.sprite = currentItem.sprite;
            damageSlotImage.gameObject.SetActive(true);
            
            // Small fade in on the selection panels when switching / picking-up and item 
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

        // Also set the same type of item (Baton, Taser etc.) as current item of this player for others see
        _view.RPC("SetItemRemote", RpcTarget.Others, slot.itemName);
        return true;
    }
    
    private void UpdateCooldownNotice()
    {
        // Show a cooldown notice if it is not expired yet
        if (Time.time < _cooldownNoticeExpiry)
        {
            // Show the cooldown and the remaining time that it will be applied
            hud.ShowCooldownNotice((int) Math.Round(_cooldownNoticeExpiry - Time.time), _currentCooldownText);
        }
        else
        {
            hud.CloseCooldownNotice();
        }
    }

    // Called when hitting mask thief with Lasso for example (To encourage team work)
    public void CooldownAllItems(float timeAttackCooldown, float timeOthersCooldown)
    {
        // Increase expiration time for attack, siwtch and pick-up cooldown
        _pickUpCooldownExpiry = _switchCooldownExpiry = Time.time + timeOthersCooldown;
        _attackCooldownExpiry = Time.time + timeAttackCooldown;
        AddAntiWeaponSpamNotice(timeAttackCooldown);
    }

    private void AddAntiWeaponSpamNotice(float addSeconds)
    {
        // Increase expiration time for showing a cooldown notice to the player
        _cooldownNoticeExpiry = Time.time + addSeconds;
        _currentCooldownText = StandardCooldownText;
        
        // Choose info color because the player did not do anything bad
        hud.cooldownNoticeColor = hud.infoColor;
        hud.ShowCooldownNotice((int) Math.Round(addSeconds), _currentCooldownText);
    }
    
    public void AddNpcHitNotice(float addSeconds)
    {
        // Increase expiration time for showing a cooldown notice to the player
        _cooldownNoticeExpiry = _attackCooldownExpiry = Time.time + addSeconds;
        _currentCooldownText = NpcCooldownText;
        
        // Choose alert color because the player hit an NPC (he should only hit the mask thief)
        hud.cooldownNoticeColor = hud.alertColor;
        hud.ShowCooldownNotice((int) Math.Round(addSeconds), _currentCooldownText);
    }

    // Show info bubble for a set amount of time with a Coroutine
    public void ShowInfoBubble(string text, float time)
    {
        IEnumerator<YieldInstruction> RunUpdateInfoBubble()
        {
            hud.OpenInfoBubble(text);
            yield return new WaitForSeconds(time);
            hud.CloseInfoBubble();
        }

        StartCoroutine(RunUpdateInfoBubble());
    }
}