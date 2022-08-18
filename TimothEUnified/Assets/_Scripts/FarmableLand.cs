using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmableLand : MonoBehaviour
{
    [SerializeField] Crop _cropPrefab;

    public bool IsOccupied { get => _isOccupied; set => _isOccupied = value; }
    bool _isOccupied = false;

    public bool IsTilled { get => _isTilled; set
        {
            _isTilled = value;
            GetComponent<SpriteRenderer>().color = _isTilled ? Color.green : Color.red;
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

    public bool ReadyToPlant()
    {
        return !_isOccupied && _isTilled;
    }
}
