using GameDevTV.Inventories;
using GameDevTV.UI.Inventories;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotbarUI : MonoBehaviour, IItemHolder
{
    [SerializeField] int index = 0;

    InventoryItemIcon _slotUI;
    Inventory _inventory;

    private void Awake()
    {
        _slotUI = GetComponentInChildren<InventoryItemIcon>();

        _inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
    }

    public void Setup()
    {
        Debug.Log("Setting Hotbar");
        if(!_inventory)
        {
            Debug.LogError("No Inventory!");
            return;
        }

        _slotUI.SetItem(_inventory.GetItemInSlot(index), _inventory.GetNumberInSlot(index));
    }

    void SetSelected()
    {

    }

    public InventoryItem GetItem()
    {
        return _inventory.GetItemInSlot(index);
    }
}
