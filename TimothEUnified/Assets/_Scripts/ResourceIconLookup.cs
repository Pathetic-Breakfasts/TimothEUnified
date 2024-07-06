using GameFramework.Inventories;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct ResourceIcon
{
    public ResourceType resourceType;
    public Sprite resourceIcon;
}

public class ResourceIconLookup : MonoBehaviour
{
    [SerializeField] List<ResourceIcon> resourceIcons;

    //////////////////////////////////////////////////
    public Sprite GetResourceSprite(ResourceType target)
    {
        foreach(ResourceIcon r in resourceIcons)
        {
            if(r.resourceType == target) return r.resourceIcon;
        }

        return null;
    }
}
