using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TimothE.Gameplay.Interactables
{
    //////////////////////////////////////////////////
    public class Bed : MonoBehaviour, IInteractable
    {
        public bool CanSleep { get => _bCanSleep; set => _bCanSleep = value; }
        bool _bCanSleep = false;
    
        TimeManager _dayManager;
    
        //////////////////////////////////////////////////
        private void Start()
        {
            _dayManager = FindObjectOfType<TimeManager>();
    
            //HACK allows us to sleep until we have a RaidManager in place
            _bCanSleep = true;
        }
    
        //////////////////////////////////////////////////
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
    
        //////////////////////////////////////////////////
        public void OnUse(PlayerController controller)
        {
            //TODO: Implement sleep UI and regaining character energy
            throw new System.NotImplementedException();
        }
    }
}
