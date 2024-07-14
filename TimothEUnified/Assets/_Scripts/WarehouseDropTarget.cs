using GameFramework.Core.UI.Dragging;
using GameFramework.Inventories;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TimothE.Gameplay.Interactables;
public class WarehouseDropTarget : MonoBehaviour, IDragDestination<InventoryItem>
{
    //////////////////////////////////////////////////
    public void AddItems(InventoryItem item, int number)
    {
        if(item != null)
        {
            Warehouse warehouse = FindObjectOfType<UIManager>().CurrentWarehouse;
            if(warehouse != null )
            {
                warehouse.InsertResource(item,number);
            }
        }
    }

    //////////////////////////////////////////////////
    public InventoryItem GetItem()
    {
        throw new System.NotImplementedException();
    }

    //////////////////////////////////////////////////
    public int GetNumber()
    {
        throw new System.NotImplementedException();
    }

    //////////////////////////////////////////////////
    public int MaxAcceptable(InventoryItem item)
    {
        if(item.itemType == ItemType.RESOURCE)
        {
            Warehouse warehouse = FindObjectOfType<UIManager>().CurrentWarehouse;
            if (warehouse != null)
            {
                return warehouse.GetRemainingCapacity();
            }
        }

        return 0;
    }

    //////////////////////////////////////////////////
    public void RemoveItems(int number)
    {
        throw new System.NotImplementedException();
    }
}
