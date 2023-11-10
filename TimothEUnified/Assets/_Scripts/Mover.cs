using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField] float _moveSpeed = 5.0f;

    //Movement Speed Ratio controls how much the relative move speed affects the character. A movement speed of 5.0 with a Movement Speed Ratio of 0.5 will result in a move speed 2.5
    public float MovementSpeedRatio { get => _movementSpeedRatio; set=> _movementSpeedRatio = value; }
    float _movementSpeedRatio = 1.0f;

    public void Move(Vector2 movement)
    {
        if (movement == Vector2.zero) return;

        Vector2 originalPos = transform.position;
        originalPos += movement.normalized * (_moveSpeed * _movementSpeedRatio) * Time.fixedDeltaTime;
        transform.position = originalPos;
    }
}
