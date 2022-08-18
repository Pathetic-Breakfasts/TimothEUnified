using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmableLand : MonoBehaviour
{
    [SerializeField] Crop _cropPrefab;
    [SerializeField] Sprite _tilledSprite;
    [SerializeField] Sprite _untilledSprite;

    public bool IsOccupied { get => _isOccupied; set => _isOccupied = value; }
    bool _isOccupied = false;

    public bool IsTilled { get => _isTilled; set
        {
            _isTilled = value;
            GetComponent<SpriteRenderer>().sprite = _isTilled ? _tilledSprite : _untilledSprite;
        }
    }
    bool _isTilled = false;

    // Start is called before the first frame update
    void Start()
    {
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
