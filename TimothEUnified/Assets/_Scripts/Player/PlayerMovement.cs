using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] float _moveSpeed = 5.0f;

    Rigidbody2D _rb;
    Animator _animator;

    Vector2 _movement;


    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _movement.x = Input.GetAxisRaw("Horizontal");
        _movement.y = Input.GetAxisRaw("Vertical");

        _animator.SetFloat("Horizontal", _movement.x);
        _animator.SetFloat("Vertical", _movement.y);
        _animator.SetFloat("Speed", _movement.sqrMagnitude);
    }

    private void FixedUpdate()
    {
        if (_movement == Vector2.zero) return;

        Vector2 originalPos = transform.position;
        originalPos += _movement.normalized * _moveSpeed * Time.fixedDeltaTime;
        transform.position = originalPos;

        _animator.SetFloat("LastHorizontal", _movement.x);
        _animator.SetFloat("LastVertical", _movement.y);
    }
}
