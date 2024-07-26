using GameFramework.Inventories;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
struct ResourceCost
{
    public ResourceType item;
    public int quantity;
}

[CreateAssetMenu(menuName ="Assets/Create Structure Config")]
public class StructureConfig : ScriptableObject
{
    //UI
    public string buildingName;
    public string description;

    public Vector2 structureSize;

    public Sprite icon;

    //Gameplay
    public RuleTile tilemapTile;

    //Resource Costs
    List<ResourceCost> structureResourceCost;
    int structureGoldCost;
}
