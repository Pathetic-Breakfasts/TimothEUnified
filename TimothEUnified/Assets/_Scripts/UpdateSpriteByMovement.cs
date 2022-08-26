using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class UpdateSpriteByMovement : MonoBehaviour
{
    Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        Vector2 rbVelocity = _rb.velocity;
        Vector2 direction = Vector2.zero;


        //Calculates which axis has the stronger pull
        float xDistToZero = rbVelocity.x < 0.0f ? Mathf.Abs(rbVelocity.x) : rbVelocity.y;
        float yDistToZero = rbVelocity.y < 0.0f ? Mathf.Abs(rbVelocity.y) : rbVelocity.y;

        //Is the character moving more in the X axis than the Y
        if(xDistToZero > yDistToZero)
        {
            //Moving Left or Right
            direction.x = rbVelocity.x < 0.0f ? -1.0f : 1.0f;
        }
        else
        {
            //Moving Up or Down
            direction.y = rbVelocity.y < 0.0f ? -1.0f : 1.0f;
        }



    }
}
