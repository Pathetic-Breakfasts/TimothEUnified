using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Events;

public class Weapon : MonoBehaviour
{
    TrailRenderer _trail;
    SpriteRenderer _renderer;

    public bool Attacking { get => _attacking; }
    bool _attacking;

    [SerializeField] WeaponConfig _config;

    [SerializeField] string[] _acceptableTags;

    Collider2D _col;

    float _originalEulerZ;
    float _eulerZTargetAngle;
    [SerializeField] float _weaponSwingAmount = 90.0f;

    [Header("Events")]
    [SerializeField] UnityEvent _onStartSwing;
    [SerializeField] UnityEvent _onEndSwing;
    [SerializeField] UnityEvent _onHit;
    private void Awake()
    {
        _trail = GetComponentInChildren<TrailRenderer>();
        _renderer = GetComponent<SpriteRenderer>();
        _trail.gameObject.SetActive(false);
        _col = GetComponent<Collider2D>();
    }

    private void Start()
    {
        EquipWeapon(_config);
    }

    private void Update()
    {
        if (_attacking && !_config._isRanged)
        {
            Vector3 euler = transform.parent.localEulerAngles;
            euler.z = Mathf.LerpAngle(euler.z, _eulerZTargetAngle, 15.0f * Time.deltaTime) % 360.0f;
            transform.parent.localEulerAngles = euler;

            float diffToTarget = Mathf.Abs(euler.z - _eulerZTargetAngle) % 360.0f;

            if (diffToTarget < 3.0f) EndSwing();
        }
    }

    public void StartSwing(Transform target)
    {
        if (_attacking) return;

        if (target)
        {
            Vector2 dir = (Vector2)target.position - (Vector2)transform.parent.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            transform.parent.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        if (_config._isRanged)
        {
            Projectile projectile = Instantiate(_config._projectilePrefab);
            projectile.transform.position = transform.position;
            projectile.SetTarget(transform.parent.rotation, _config._damage, _acceptableTags);

            _attacking = false;
        }
        else
        {
            _trail.gameObject.SetActive(true);
            _attacking = true;
            _col.enabled = true;

            _onStartSwing.Invoke();

            _originalEulerZ = transform.parent.localEulerAngles.z % 360.0f;

            _eulerZTargetAngle = _originalEulerZ + (_weaponSwingAmount / 2.0f) % 360.0f;

            float angle = _originalEulerZ - (_weaponSwingAmount / 2.0f) % 360.0f;

            Vector3 eulers = transform.parent.localEulerAngles;
            eulers.z = angle;
            transform.parent.localEulerAngles = eulers;
        }
    }

    public void EndSwing()
    {
        Vector3 eulers = transform.parent.localEulerAngles;
        eulers.z = _originalEulerZ;
        transform.parent.localEulerAngles = eulers;

        _onEndSwing.Invoke();

        _trail.gameObject.SetActive(false);
        _attacking = false;
        _col.enabled = false;
    }

    public void EquipWeapon(WeaponConfig config)
    {
        _config = config;
        _renderer.sprite = _config._sprite;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_attacking) return;

        foreach (string s in _acceptableTags)
        {
            if (collision.CompareTag(s))
            {
                Health targetHealth = collision.GetComponent<Health>();

                if (targetHealth)
                {
                    targetHealth.TakeDamage(_config._damage);

                    _onHit.Invoke();
                }
            }
        }
    }
}
