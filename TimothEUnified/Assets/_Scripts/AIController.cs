using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        EnemyPathfinder pathfinder = GetComponent<EnemyPathfinder>();

        PlayerInput input = FindObjectOfType<PlayerInput>();

        pathfinder.SetTargetTransform(input.transform, true, 2.5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
