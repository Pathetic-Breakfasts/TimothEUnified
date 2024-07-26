using GameFramework.Inventories;
using System.Collections.Generic;
using UnityEngine;
using TimothE.Gameplay.Interactables;
public class WarehouseManager : MonoBehaviour
{
    //////////////////////////////////////////////////
    public Dictionary<ResourceType, int> ResourceMap { get => _resources; }
    Dictionary<ResourceType, int> _resources;

    //////////////////////////////////////////////////
    private void Awake()
    {
        _resources = new Dictionary<ResourceType, int>();
    }

    //TODO: Figure out how to take objects out of their source warehouse
    //If warehouses even care about what objects are inside of them? 

    //////////////////////////////////////////////////
    public void AddResource(ResourceType resourceType, int amount)
    {
        if(_resources.ContainsKey(resourceType))
        {
            _resources[resourceType] += amount;
        }
        else
        {
            _resources.Add(resourceType, amount);
        }
    }

    //////////////////////////////////////////////////
    public void RemoveResource(ResourceType resourceType, int amount) 
    {
        if( _resources.ContainsKey(resourceType))
        {
            if (amount <= _resources[resourceType]) 
            {
                _resources[resourceType] -= amount;

                if (_resources[resourceType] <= 0)
                {
                    _resources.Remove(resourceType);
                }
            }
        }
    }

    //////////////////////////////////////////////////
    public bool HasResource(ResourceType resourceType, int amount)
    {
        if (_resources.ContainsKey(resourceType))
        {
            if (amount <= _resources[resourceType])
            {
                return true;
            }
        }

        return false;
    }
}
