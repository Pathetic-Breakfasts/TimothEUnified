using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateSpriteByMovement : MonoBehaviour
{
    EnemyPathfinder _pathfinder;
    Animator _animator;

    //////////////////////////////////////////////////
    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _pathfinder = GetComponent<EnemyPathfinder>();
    }

    //////////////////////////////////////////////////
    void Update()
    {
        Vector2 characterVelocity = _pathfinder.DirectionVector;
        Vector2 direction = Vector2.zero;


        //Calculates which axis has the stronger pull
        float xDistToZero = characterVelocity.x < 0.0f ? Mathf.Abs(characterVelocity.x) : characterVelocity.x;
        float yDistToZero = characterVelocity.y < 0.0f ? Mathf.Abs(characterVelocity.y) : characterVelocity.y;

        if (Mathf.Approximately(characterVelocity.x, 0.0f) && Mathf.Approximately(characterVelocity.y, 0.0f))
        {
            _animator.SetFloat("speed", 0.0f);
            return;
        }

        //Is the character moving more in the X axis than the Y
        if(xDistToZero > yDistToZero)
        {
            //Moving Left or Right
            direction.x = characterVelocity.x < 0.0f ? -1.0f : 1.0f;
        }
        else
        {
            //Moving Up or Down
            direction.y = characterVelocity.y < 0.0f ? -1.0f : 1.0f;
        }

        _animator.SetFloat("horizontal", direction.x);
        _animator.SetFloat("vertical", direction.y);
        _animator.SetFloat("speed", direction.magnitude);
    }
}
