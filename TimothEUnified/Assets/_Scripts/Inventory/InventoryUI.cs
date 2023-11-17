using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryUI : MonoBehaviour
{

    InventoryStore _inventoryStore;

    InventorySlotUI[] _inventorySlotUI;

    InventoryRecord _draggedRecord;

    InventorySlotUI _draggedUI;


    DraggedItemUI _draggedItemUI;
    private void Awake()
    {
        _inventoryStore = FindObjectOfType<InventoryStore>();
    }

    private void Start()
    {
        _inventoryStore = FindObjectOfType<InventoryStore>();
        _inventorySlotUI = FindObjectsOfType<InventorySlotUI>();
        _draggedItemUI = FindObjectOfType<DraggedItemUI>();
    }

    public void UpdateUI()
    {
        InventoryRecord[] inventoryRecords = _inventoryStore.GetInventoryRecords();

        if(inventoryRecords.Length != _inventorySlotUI.Length)
        {
            Debug.LogError("Mismatch between inventory record array length and inventory slot ui length");
        }

        //Cycle through our inventory screen
        for(int i = 0; i < _inventorySlotUI.Length; ++i)
        {
            _inventorySlotUI[i].SetItem(inventoryRecords[i].config, inventoryRecords[i].quantity);
        }

        //Cycle through our hotbar UI
    }

    private int FindSlotUIIndex(InventorySlotUI slotUI)
    {
        for(int i  = 0; i < _inventorySlotUI.Length; ++i)
        {
            if (_inventorySlotUI[i] == slotUI)
            {
                return i;
            }
        }

        return -1;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(eventData.selectedObject != null)
        {


        Debug.Log(eventData.selectedObject.name);
        InventorySlotUI slot = eventData.selectedObject.GetComponent<InventorySlotUI>();
        int index = FindSlotUIIndex(slot);

        if(index != -1)
        {
            InventoryRecord[] inventoryRecords = _inventoryStore.GetInventoryRecords();
            Debug.Log(inventoryRecords[index].config.name);
        }
        else
        {
            Debug.Log("Cannot find selected item");
        }
        }
        else
        {
            Debug.Log("No Object");
        }
    }

    public void SetDragged(InventorySlotUI slotUI)
    {
        _draggedUI = slotUI;
        if (slotUI == null)
        {
            _draggedItemUI.SetAttributes(null, 0);
            return;
        }

        int index = FindSlotUIIndex(slotUI);

        if (index != -1)
        {
            InventoryRecord[] inventoryRecords = _inventoryStore.GetInventoryRecords();
            _draggedItemUI.SetAttributes(inventoryRecords[index].config.icon, inventoryRecords[index].quantity);
            Debug.Log(inventoryRecords[index].config.name);
        }
        else
        {
            Debug.Log("Cannot find selected item");
        }
    }

    public void UpdateDragged(PointerEventData eventData)
    {
        if(_draggedUI == null)
        {
            return;
        }

        _draggedItemUI.UpdatePosition(eventData.position);
    }
}
