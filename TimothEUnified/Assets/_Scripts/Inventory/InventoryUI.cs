using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{

    InventoryStore _inventoryStore;

    InventorySlotUI[] _inventorySlotUI;

    private void Awake()
    {
        _inventoryStore = FindObjectOfType<InventoryStore>();
    }

    private void Start()
    {
        _inventoryStore = FindObjectOfType<InventoryStore>();
        _inventorySlotUI = FindObjectsOfType<InventorySlotUI>();
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
}
