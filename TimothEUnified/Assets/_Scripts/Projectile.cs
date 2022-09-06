using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Movement")]
    [Min(0.01f)][SerializeField] float _movementSpeed = 3.0f;



    [Header("Tag Settings")]
    [SerializeField] string[] _acceptedTags;




    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;
        pos += transform.forward * _movementSpeed;
        transform.position = pos;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        
    }
}
