using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    string[] _acceptedTags;

    float _movementSpeed = 1.0f;
    float _damage;

    void Update()
    {
        transform.position += transform.right * Time.deltaTime * _movementSpeed;
    }

    public void SetConfig(ProjectileConfig config)
    {
        _movementSpeed = config.movementSpeed;
        GetComponent<SpriteRenderer>().sprite = config.projectileSprite;
        GetComponent<DestroyAfterSeconds>().SetLifetime(config.lifetime);
    }

    public void SetDirection(Quaternion rotation, float damage, string[] acceptedTags)
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
                break;
            }
        }

        if (_shouldBeDestroyed)
        {
            Destroy(gameObject);
        }
    }
}
