using GameDevTV.Inventories;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crop : MonoBehaviour
{
    private InventoryItem _config;

    public InventoryItem Config { get => _config; }

    private int _daysSincePlanted = 0;

    SpriteRenderer _sp;

    bool _readyToPick = false;

    int _timeToGrowCrop;

    DayManager _dm;

    private void Awake()
    {
        _dm = FindObjectOfType<DayManager>();
    }

    public void ProgressDay()
    {
        _daysSincePlanted++;

        Debug.Log("Crop has been planted for: " + _daysSincePlanted + " days");

        int noOfSprites = _config.growthSpriteArray.Length; //4
        int spriteDivider = _timeToGrowCrop / noOfSprites; //3

        int spriteElement = (_daysSincePlanted / spriteDivider);
        if(spriteElement >= noOfSprites)
        {
            spriteElement = noOfSprites - 1;
        }

        _sp.sprite = _config.growthSpriteArray[spriteElement];

        if(_daysSincePlanted > _timeToGrowCrop)
        {
            _readyToPick = true;
        }
    }

    public bool ReadyToPick()
    {
        return _readyToPick;
    }

    public void Plant(InventoryItem config)
    {
        _config = config;

        //Calculates the amount of time it will take to grow the plant based on if we are in the correct season or not
        float timeToGrow = (_dm.CurrentSeason == _config.idealSeasonToGrow) ? (float)_config.daysToGrow : (float)_config.daysToGrow * (1.0f + _config.incorrectSeasonPentalty);

        _timeToGrowCrop = (int)timeToGrow;

        _sp = GetComponent<SpriteRenderer>();
        _sp.sprite = _config.growthSpriteArray[0];
    }
}
