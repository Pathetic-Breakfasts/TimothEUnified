using GameDevTV.Inventories;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotbarManager : MonoBehaviour
{
    HotbarUI[] _hotbarSlots;


    private void Start()
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
        }
    }
}
