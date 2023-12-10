using GameDevTV.Inventories;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FarmableLand : MonoBehaviour
{
    [SerializeField] Crop _cropPrefab;

    [Header("Graphics Settings")]
    [SerializeField] RuleTile _untilledTile;
    [SerializeField] RuleTile _tilledTile;

    
    Tilemap _tilemap; 

    bool _isOccupied = false;
    public bool IsOccupied { get => _isOccupied; set => _isOccupied = value; }

    Crop _childCrop;
    bool _isTilled = false;

    public bool IsTilled { get => _isTilled; set
        {
            _isTilled = value;

            Vector3Int tPos = _tilemap.WorldToCell(transform.position);
            TileBase tb = value ? _tilledTile : _untilledTile;
            _tilemap.SetTile(tPos, tb);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //Finds all tilemap objects in the scene
        Tilemap[] tilemaps = FindObjectsOfType<Tilemap>();
        foreach(Tilemap tm in tilemaps)
        {
            //Finds the correct tilemap 
            if(tm.gameObject.name == "Tilemap_Farmland")
            {
                _tilemap = tm;
                break;
            }
        }

        if(!_tilemap)
        {
            Debug.LogWarning("Tilemap_Farmland was not found");
        }

        IsTilled = false;
    }

    public void Plant(InventoryItem desiredCrop)
    {
        if (ReadyToPlant())
        {
            _childCrop = Instantiate(_cropPrefab);
            _childCrop.transform.parent = transform;
            _childCrop.transform.position = Vector2.zero;
            _childCrop.transform.localPosition = Vector2.zero;
            _childCrop.Plant(desiredCrop);
            _isOccupied = true;
        }
    }


    public void Harvest()
    {
        if (_childCrop && _childCrop.ReadyToPick)
        {
            Inventory.GetPlayerInventory().AddToFirstEmptySlot(_childCrop.Config.grownCropItem, 1);
            Destroy(_childCrop.gameObject);
            _isOccupied = false;
            _childCrop = null;
        }
    }

    public void ProgressDay()
    {
        if(_isOccupied && _childCrop)
        {
            _childCrop.ProgressDay();
        }
    }

    public bool ReadyToPlant()
    {
        return !_isOccupied && _isTilled;
    }

    public bool ReadyToHarvest()
    {
        //Makes sure we have a child crop and if it is ready to harvest
        return (_childCrop && _childCrop.ReadyToPick);
    }
}
