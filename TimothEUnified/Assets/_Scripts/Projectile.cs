using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Movement")]
    [Min(0.01f)][SerializeField] float _movementSpeed = 3.0f;



    string[] _acceptedTags;


    float _damage;

    void Update()
    {
        transform.position += transform.right * Time.deltaTime * _movementSpeed;
    }

    public void SetTarget(Quaternion rotation, float damage, string[] acceptedTags)
    {
        transform.rotation = rotation;
        _acceptedTags = acceptedTags;

        _damage = damage;
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        bool _shouldBeDestroyed = false;

        foreach (string s in _acceptedTags)
        {
            if (col.CompareTag(s))
            {
                Health targetHealth = col.gameObject.GetComponent<Health>();

                if (targetHealth)
                {
                    targetHealth.TakeDamage(_damage);
                }

                _shouldBeDestroyed = true;

                continue;
            }
        }

        if (_shouldBeDestroyed)
        {
            Destroy(gameObject);
        }
    }
}
