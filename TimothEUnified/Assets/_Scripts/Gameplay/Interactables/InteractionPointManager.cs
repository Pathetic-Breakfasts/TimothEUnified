using System.Collections;
using System.Collections.Generic;
using TimothE.Utility;
using UnityEngine;

namespace TimothE.Gameplay.Interactables
{
    //////////////////////////////////////////////////
    public class InteractionPointManager : MonoBehaviour
    {
        [SerializeField] InteractionPoint[] _interactables;

        //////////////////////////////////////////////////
        public InteractionPoint GetInteractionPoint(InteractDirection dir)
        {
            return _interactables[(int)dir];
        }
    }
}
