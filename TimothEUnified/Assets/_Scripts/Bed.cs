using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bed : MonoBehaviour
{



    public bool CanSleep { get => _bCanSleep; set => _bCanSleep = value; }
    bool _bCanSleep = false;

    DayManager _dayManager;

    private void Start()
    {
        _dayManager = FindObjectOfType<DayManager>();

        //HACK allows us to sleep until we have a RaidManager in place
        _bCanSleep = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag(GameTagManager._playerTag))
        {
            collision.gameObject.GetComponent<PlayerController>().CurrentBed = this;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag(GameTagManager._playerTag))
        {
            collision.gameObject.GetComponent<PlayerController>().CurrentBed = null;
        }
    }

    public void Sleep(int desiredSleep)
    {
        if(_bCanSleep)
        {
            _dayManager.SleepSkipTime(desiredSleep);
        }
        else
        {
            //TODO: UI PROMPT: "You cannot sleep right now"
        }
    }
}
