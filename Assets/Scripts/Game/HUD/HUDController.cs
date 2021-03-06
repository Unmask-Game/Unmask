using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    [SerializeField] private GameObject messagePanel;
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private GameObject cooldownNotice;
    [SerializeField] private GameObject infoBubble;
    public Color infoColor;
    public Color alertColor;
    
    private List<GameObject> _slotSelectionList;
    private Image _cooldownNoticeBackground;
    public Color cooldownNoticeColor;
    private Text _cooldownNoticeText;
    private Text _infoBubbleText;

    private void Awake()
    {
        _cooldownNoticeText = cooldownNotice.GetComponentInChildren<Text>();
        _cooldownNoticeBackground = cooldownNotice.gameObject.GetComponent<Image>();
        _infoBubbleText = infoBubble.GetComponentInChildren<Text>();
        
        // Deselect / Disable all notices or selections in the beginning
        cooldownNotice.SetActive(false);
        _slotSelectionList = GetSlotSelections();
        messagePanel.SetActive(false);
        DeselectAllSlots();
    }
    
    public void OpenInfoBubble(string text)
    {
        infoBubble.SetActive(true);
        _infoBubbleText.text = text;
    }
    
    public void CloseInfoBubble()
    {
        infoBubble.SetActive(false);
    }
    
    public void OpenMessagePanel()
    {
        messagePanel.SetActive(true);
    }

    public void CloseMessagePanel()
    {
        messagePanel.SetActive(false);
    }

    public void ShowCooldownNotice(int displayTime, string text)
    {
        cooldownNotice.SetActive(true);
        _cooldownNoticeBackground.color = cooldownNoticeColor;
        _cooldownNoticeText.text = text + displayTime + "s)";
    }

    public void CloseCooldownNotice()
    {
        cooldownNotice.SetActive(false);
    }

    private void DeselectAllSlots()
    {
        foreach (var slot in GetSlotSelections())
        {
            slot.SetActive(false);
        }
    }

    public void SelectSlot(int index)
    {
        _slotSelectionList[index].SetActive(true);
    }

    public void DeselectSlot(int index)
    {
        _slotSelectionList[index].SetActive(false);
    }

    private List<GameObject> GetSlotSelections()
    {
        var slots = new List<GameObject>();
        foreach (Transform slot in inventoryPanel.GetComponentInChildren<Transform>())
        {
            if (slot.CompareTag("SlotSelection"))
                slots.Add(slot.gameObject);
        }

        return slots;
    }
}