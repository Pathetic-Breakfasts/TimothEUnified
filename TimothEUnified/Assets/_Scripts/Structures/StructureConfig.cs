using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Assets/Create Structure Config")]
public class StructureConfig : ScriptableObject
{
    public string buildingName;
    public string description;

    public Vector2 structureSize;

    public Sprite icon;

    public RuleTile tilemapTile;

    //TODO: Resource Costs

}
