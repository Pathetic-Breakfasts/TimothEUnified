using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class UpdateSpriteByMovement : MonoBehaviour
{
    Rigidbody2D _rb;
    Animator _animator;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponentInChildren<Animator>();
    }


    void Update()
    {
        Vector2 rbVelocity = _rb.velocity;
        Vector2 direction = Vector2.zero;


        //Calculates which axis has the stronger pull
        float xDistToZero = rbVelocity.x < 0.0f ? Mathf.Abs(rbVelocity.x) : rbVelocity.x;
        float yDistToZero = rbVelocity.y < 0.0f ? Mathf.Abs(rbVelocity.y) : rbVelocity.y;

        if (Mathf.Approximately(rbVelocity.x, 0.0f) && Mathf.Approximately(rbVelocity.y, 0.0f))
        {
            _animator.SetFloat("speed", 0.0f);
            return;
        }

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

        _animator.SetFloat("horizontal", direction.x);
        _animator.SetFloat("vertical", direction.y);
        _animator.SetFloat("speed", direction.magnitude);

    }
}
