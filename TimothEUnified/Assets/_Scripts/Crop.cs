using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crop : MonoBehaviour
{
    private CropConfig _config;

    public CropConfig Config { get => _config; }

    private int _daysSincePlanted = 0;

    SpriteRenderer _sp;

    bool _readyToPick = false;

    private void Awake()
    {
        
    }

    public void ProgressDay()
    {
        _daysSincePlanted++;

        Debug.Log("Crop has been planted for: " + _daysSincePlanted + " days");

        int noOfSprites = _config.growthSpriteArray.Length; //4
        int spriteDivider = _config.daysToGrow / noOfSprites; //3

        int spriteElement = (_daysSincePlanted / spriteDivider);
        if(spriteElement >= noOfSprites)
        {
            spriteElement = noOfSprites - 1;
        }

        _sp.sprite = _config.growthSpriteArray[spriteElement];

        if(_daysSincePlanted > _config.daysToGrow)
        {
            _readyToPick = true;
        }
    }

    public bool ReadyToPick()
    {
        return _readyToPick;
        if(_daysSincePlanted >= _config.daysToGrow)
        {
            return true;
        }
        return false;
    }

    public void Plant(CropConfig config)
    {
        _config = config;


        _sp = GetComponent<SpriteRenderer>();
        _sp.sprite = _config.growthSpriteArray[0];
    }
}
