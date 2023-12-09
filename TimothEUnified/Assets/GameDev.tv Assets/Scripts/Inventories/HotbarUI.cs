using GameDevTV.Inventories;
using GameDevTV.UI.Inventories;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotbarUI : MonoBehaviour, IItemHolder
{
    [SerializeField] int index = 0;

    [SerializeField] GameObject _selectedIconObject = null;

    InventoryItemIcon _slotUI;
    Inventory _inventory;

    private void Awake()
    {
        _slotUI = GetComponentInChildren<InventoryItemIcon>();

        _inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();

        if (_selectedIconObject)
        {
            _selectedIconObject.SetActive(false);
        }
    }

    public void Setup()
    {
        if(!_inventory)
        {
            Debug.LogError("No Inventory!");
            return;
        }

        _slotUI.SetItem(_inventory.GetItemInSlot(index), _inventory.GetNumberInSlot(index));
    }

    public void SetSelected()
    {
        _selectedIconObject?.SetActive(true);
    }

    public void SetUnselected()
    {
        _selectedIconObject?.SetActive(false);
    }

    public InventoryItem GetItem()
    {
        return _inventory.GetItemInSlot(index);
    }
}
