using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Inventories;
using static GameFramework.Inventories.Inventory;

[CreateAssetMenu(menuName ="Inventory/New Inventory Loadout")]
public class InventoryLoadout : ScriptableObject
{
    [SerializeField] InventorySlot[] items;

    public void SpawnItems(Inventory inventory)
    {
        for(int i =0; i < items.Length; ++i)
        {
            inventory.AddToFirstEmptySlot(items[i].item, items[i].number);
        }
    }
}
