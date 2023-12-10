using GameDevTV.Inventories;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Networking.PlayerConnection;
using UnityEngine;

public class Crop : MonoBehaviour
{
    private InventoryItem _config;

    public InventoryItem Config { get => _config; }

    private int _daysSincePlanted = 0;

    SpriteRenderer _sp;

    public bool ReadyToPick { get => _readyToPick; }
    bool _readyToPick = false;

    int _timeToGrowCrop;

    DayManager _dm;

    private void Awake()
    {
        _dm = FindObjectOfType<DayManager>();
        _sp = GetComponent<SpriteRenderer>();
    }

    public void ProgressDay()
    {
        _daysSincePlanted++;

        int noOfSprites = _config.growthSpriteArray.Length;
        int spriteDivider = _timeToGrowCrop / noOfSprites;

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

    public void Plant(InventoryItem config)
    {
        _config = config;

        //Calculates the amount of time it will take to grow the plant based on if we are in the correct season or not
        float timeToGrow = (_dm.CurrentSeason == _config.correctSeason) ? (float)_config.daysToGrow : 100.0f; //TODO: Make plants die in wrong season

        if(timeToGrow <= 0)
        {
            Debug.LogError("Plant:" + config.displayName + " days to grow set incorrectly");
        }

        _timeToGrowCrop = (int)timeToGrow;
        _sp.sprite = _config.growthSpriteArray[0];
    }
}
