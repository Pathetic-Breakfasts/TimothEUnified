using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FarmableLand : MonoBehaviour
{
    [SerializeField] Crop _cropPrefab;
    [SerializeField] Sprite _tilledSprite;
    [SerializeField] Sprite _untilledSprite;

    [SerializeField] RuleTile _untilledTile;
    [SerializeField] RuleTile _tilledTile;


    Tilemap _tilemap; 
    public bool IsOccupied { get => _isOccupied; set => _isOccupied = value; }
    bool _isOccupied = false;

    public bool IsTilled { get => _isTilled; set
        {
            _isTilled = value;

            if(_tilemap == null)
            {
                Tilemap[] tms = FindObjectsOfType<Tilemap>();

                foreach (Tilemap tm in tms)
                {
                    if (tm.gameObject.name == "Tilemap_Decoration")
                    {
                        _tilemap = tm;
                    }
                }

                if (_tilemap == null)
                {
                    Debug.Log("Tilemap_Decoration was not found");
                }
            }

            Vector3Int tPos = _tilemap.WorldToCell(transform.position);
            TileBase tb = value ? _tilledTile : _untilledTile;
            _tilemap.SetTile(tPos, tb);
            //GetComponent<SpriteRenderer>().sprite = _isTilled ? _tilledSprite : _untilledSprite;
        }
    }
    bool _isTilled = false;

    // Start is called before the first frame update
    void Start()
    {

        Tilemap[] tms = FindObjectsOfType<Tilemap>();

        foreach(Tilemap tm in tms)
        {
            if(tm.gameObject.name == "Tilemap_Decoration")
            {
                _tilemap = tm;
            }
        }

        if(_tilemap == null)
        {
            Debug.Log("Tilemap_Decoration was not found");
        }
        IsTilled = false;
    }

    public void Plant(CropConfig desiredCrop)
    {
        if (ReadyToPlant())
        {
            Crop cropObject = Instantiate(_cropPrefab);
            cropObject.transform.parent = transform;
            cropObject.transform.position = Vector2.zero;
            cropObject.transform.localPosition = Vector2.zero;
            cropObject.Plant(desiredCrop);
            _isOccupied = true;
        }
    }

    public void Harvest()
    {

        Crop co = GetComponentInChildren<Crop>();
        if (co)
        {
            if (co.ReadyToPick())
            {
                //TODO: Actually pickup the crop (requires inventory)
                Debug.Log("Gained 1 " + co.Config.type);

                _isOccupied = false;
                Destroy(co.gameObject);

            }
        }
    }

    public bool ReadyToPlant()
    {
        return !_isOccupied && _isTilled;
    }

    public bool ReadyToHarvest()
    {
        Crop co = GetComponentInChildren<Crop>();
        if (co)
        {
            if (co.ReadyToPick()) return true;
        }
        return false;
    }
}
