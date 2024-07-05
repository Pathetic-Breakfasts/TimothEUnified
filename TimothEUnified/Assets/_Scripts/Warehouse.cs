using GameDevTV.Inventories;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class Warehouse : MonoBehaviour, IInteractable
{
    public Dictionary<ResourceType, int> ResourceMap { get => _resourceMap; }
    Dictionary<ResourceType, int> _resourceMap;

    [SerializeField] int _maximumCapactity = 1000;

    [SerializeField] bool _debugWarehouse = false;

    //////////////////////////////////////////////////
    private void Awake()
    {
        _resourceMap = new Dictionary<ResourceType, int>();
    }

    //////////////////////////////////////////////////
    public int InsertResource(InventoryItem item, int number)
    {
        if (item == null || item.itemType != ItemType.RESOURCE)
        {
            string reason = item ? item.displayName + ": ItemType is incorrect: " + item.itemType.ToString() : "Item is null";
            DebugLog(gameObject.name + ": Failed to insert item! Reason: " + reason);

            return -1;
        }

        int putIn = CalculatePutIn(number * item.resourceSize);
        if(putIn == 0)
        {
            Debug.Log(gameObject.name + ": No space present in this warehosue.");
            return -1;
        }

        if (_resourceMap.ContainsKey(item.resourceType))
        {
            DebugLog(gameObject.name + ": Resource is already present in this warehouse. Adding: " + putIn);
            _resourceMap[item.resourceType] += putIn;
        }
        else
        {
            DebugLog(gameObject.name + ": Resource is not present in this warehouse. Adding: " + putIn);
            _resourceMap.Add(item.resourceType, putIn);
        }

        FindObjectOfType<WarehouseUI>().Redraw(this);

        return putIn;
    }

    //////////////////////////////////////////////////
    private int CalculatePutIn(int num)
    {
        int space = _maximumCapactity - GetCurrentResourceTotal();
        //No space remaining
        if(space == 0)
        {
            return 0;
        }

        //The amount of space we have exceeds the amount of resources we are trying to put in
        if(num < space)
        {
            return num;
        }

        //The amount of resouces we are putting in exceeds the amount of space we have
        //We want to put 500 in
        //We have space for 300 units
        //Then we want to take num - space to give us 200 remainder
        //We then return num - remainder (500 - 200) = 300 we have put 300 items in
        int remainder = num - space;
        return num - remainder;
    }


    //////////////////////////////////////////////////
    public int GetCurrentResourceTotal()
    {
        int runningTotal = 0;
        foreach(KeyValuePair<ResourceType, int> record in _resourceMap)
        {
            runningTotal += record.Value;
        }
        return runningTotal;
    }

    //////////////////////////////////////////////////
    public int GetRemainingCapacity()
    {
        return _maximumCapactity - GetCurrentResourceTotal();
    }

    //TODO: Figure out how removing items across multiple warehouses will work
    //////////////////////////////////////////////////
    public void RemoveResource(ResourceType type, int num)
    {
        
    }

    //////////////////////////////////////////////////
    public void OnUse(PlayerController controller)
    {
        controller.SetWarehouseUIVisibility(true, this);
    }


    //////////////////////////////////////////////////
    private void DebugLog(string message)
    {
        if (_debugWarehouse)
        {
            Debug.Log(message);
        }
    }
}
