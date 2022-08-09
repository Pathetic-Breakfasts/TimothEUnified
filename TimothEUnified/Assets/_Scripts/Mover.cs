using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField] float _moveSpeed = 5.0f;

    public void Move(Vector2 movement)
    {
        if (movement == Vector2.zero) return;

        Vector2 originalPos = transform.position;
        originalPos += movement.normalized * _moveSpeed * Time.fixedDeltaTime;
        transform.position = originalPos;
    }
}
