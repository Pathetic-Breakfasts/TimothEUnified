using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmableLand : MonoBehaviour
{

    public bool IsOccupied { get => _occupied; set => _occupied = value; }
    bool _occupied = false;

    public bool IsTilled { get => _tilled; set
        {
            _tilled = value;
            GetComponent<SpriteRenderer>().color = _tilled ? Color.green : Color.red;
        }
    }
    bool _tilled = false;

    // Start is called before the first frame update
    void Start()
    {
        IsTilled = false;   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
