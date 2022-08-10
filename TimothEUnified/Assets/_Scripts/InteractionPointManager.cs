using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionPointManager : MonoBehaviour
{
    [SerializeField] InteractionPoint[] _interactables;
    

    private void Awake()
    {
        
    }

    public InteractionPoint GetInteractionPoint(InteractDirection dir)
    {
        return _interactables[(int)dir];
    }
}
