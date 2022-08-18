using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crop : MonoBehaviour
{
    private CropConfig _config;

    private int _daysSincePlanted = 0;

    SpriteRenderer _sp;

    private void Awake()
    {
        
    }

    public void ProgressDay()
    {
        _daysSincePlanted++;

        Debug.Log("Crop has been planted for: " + _daysSincePlanted + " days");

        //TODO: Handle sprite switching logic
    }

    public bool ReadyToPick()
    {
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
