using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    TrailRenderer _trail;
    SpriteRenderer _renderer;

    bool _attacking;

    [SerializeField] WeaponConfig _config;

    private void Awake()
    {
        _trail = GetComponentInChildren<TrailRenderer>();
        _renderer = GetComponent<SpriteRenderer>();
        _trail.gameObject.SetActive(false);
    }

    private void Update()
    {
    }

    public void StartSwing()
    {
        _trail.gameObject.SetActive(true);
        _attacking = true;
    }

    public void EndSwing()
    {
        _trail.gameObject.SetActive(false);
        _attacking = false;
    }

    public void EquipWeapon(WeaponConfig config)
    {
        _config = config;
        _renderer.sprite = _config._horizontalSprite;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_attacking) return;

        if (collision.CompareTag("Player")) return;

        Health targetHealth = collision.gameObject.GetComponent<Health>();

        if (targetHealth)
        {
            targetHealth.TakeDamage(_config._damage);
        }

        if (collision.CompareTag("Enemy"))
        {
            //Health targetHealth = collision.gameObject.GetComponent<Health>();

            if (targetHealth)
            {
                targetHealth.TakeDamage(_config._damage);
            }
        }
    }
}
