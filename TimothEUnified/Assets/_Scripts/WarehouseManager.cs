using GameFramework.Inventories;
using System.Collections.Generic;
using UnityEngine;
using TimothE.Gameplay.Interactables;
public class WarehouseManager : MonoBehaviour
{
    //////////////////////////////////////////////////
    public Dictionary<ResourceType, int> GetResourceMap()
    {
        Dictionary<ResourceType, int> map = new Dictionary<ResourceType, int>();

        foreach(Warehouse warehouse in FindObjectsOfType<Warehouse>())
        {
            Dictionary<ResourceType, int> warehouseMap = warehouse.ResourceMap;

            foreach(KeyValuePair<ResourceType, int> kvp in warehouseMap)
            {
                if(map.ContainsKey(kvp.Key))
                {
                    map[kvp.Key] += kvp.Value;
                }
                else
                {
                    map.Add(kvp.Key, kvp.Value);
                }
            }
        }

        return map;
    }
}
