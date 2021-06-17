using System.Collections.Generic;
using UnityEngine;

public class HUDController : MonoBehaviour
{
    [SerializeField] private GameObject messagePanel;
    [SerializeField] private GameObject inventoryPanel;
    private List<GameObject> _slotList;
    
    private void Awake()
    {
        _slotList = GetSlots();
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
        foreach (Transform slot in inventoryPanel.GetComponentInChildren<Transform>())
        {
            if (slot.CompareTag("Slot"))
                slot.gameObject.SetActive(false);
        }
    }
    public void SelectSlot(int index)
    {
        _slotList[index].SetActive(true);
    }
    
    public void DeselectSlot(int index)
    {
        _slotList[index].SetActive(false);
    }

    private List<GameObject> GetSlots()
    {
        var slots = new List<GameObject>();
        foreach (Transform slot in inventoryPanel.GetComponentInChildren<Transform>())
        {
            if (slot.CompareTag("Slot"))
                slots.Add(slot.gameObject);
        }

        return slots;
    }
}
