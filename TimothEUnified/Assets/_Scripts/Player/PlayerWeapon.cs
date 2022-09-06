using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    TrailRenderer _trail;
    SpriteRenderer _renderer;

    bool _attacking;

    [SerializeField] WeaponConfig _config;
    Collider2D _col;


    private void Awake()
    {
        _trail = GetComponentInChildren<TrailRenderer>();
        _renderer = GetComponent<SpriteRenderer>();
        _trail.gameObject.SetActive(false);
        _col = GetComponent<Collider2D>();
    }

    private void Update()
    {
    }

    public void StartSwing()
    {
        _trail.gameObject.SetActive(true);
        _attacking = true;
        _col.enabled = true;
    }

    public void EndSwing()
    {
        _trail.gameObject.SetActive(false);
        _attacking = false;
        _col.enabled = false;
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
