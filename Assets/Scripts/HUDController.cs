using System.Collections.Generic;
using UnityEngine;

public class HUDController : MonoBehaviour
{
    [SerializeField] private GameObject messagePanel;
    [SerializeField] private GameObject inventoryPanel;
    private List<GameObject> _slotSelectionList;
    
    private void Awake()
    {
        _slotSelectionList = GetSlotSelections();
        messagePanel.SetActive(false);
        DeselectAllSlots();
    }

    public void OpenMessagePanel()
    {
        messagePanel.SetActive(true);
    }

    public void CloseMessagePanel()
    {
        messagePanel.SetActive(false);
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
