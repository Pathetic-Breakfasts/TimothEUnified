using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CropType
{
    Carrot,
    Potatoes,
    Beetroots,
    Oat,
    Wheat,
    Tomatoes,
    Lettuce,
    Cabbage
}

public enum Seasons
{
    Spring,
    Summer,
    Autumn,
    Winter
}


[CreateAssetMenu(menuName ="Configs/Crop Config")]
public class CropConfig : ScriptableObject
{
    public CropType type;

    public Sprite[] growthSpriteArray;

    public int daysToGrow;
    [Range(0.0f,1.0f)]public float incorrectSeasonPentalty = 0.2f;

    public Seasons idealSeasonToGrow;

}
