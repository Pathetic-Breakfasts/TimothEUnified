using GameDevTV.Inventories;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotbarManager : MonoBehaviour
{
    HotbarUI[] _hotbarSlots;

    int _currentSlot = 0;

    private void Awake()
    {
        _hotbarSlots = FindObjectsOfType<HotbarUI>();

        Inventory playerInventory = Inventory.GetPlayerInventory();
        playerInventory.inventoryUpdated += UpdateHotbar;
    }

    public void UpdateHotbar()
    {
        for(int i  = 0; i < _hotbarSlots.Length; i++)
        {
            _hotbarSlots[i].Setup();

            if(i == _currentSlot)
            {
                _hotbarSlots[i].SetSelected();
            }
            else
            {
                _hotbarSlots[i].SetUnselected();
            }
        }
    }

    public void ModifyIndex(int num)
    {
        _hotbarSlots[_currentSlot].SetUnselected();
        _currentSlot = (_currentSlot + num) % _hotbarSlots.Length;
        if(_currentSlot < 0) _currentSlot = _hotbarSlots.Length - 1;
        _hotbarSlots[_currentSlot].SetSelected();
    }

}
