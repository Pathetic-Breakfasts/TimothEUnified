using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Weapon : MonoBehaviour
{
    TrailRenderer _trail;
    SpriteRenderer _renderer;

    public bool Attacking { get => _attacking; }
    bool _attacking;

    [SerializeField] WeaponConfig _config;

    [SerializeField] string[] _acceptableTags;

    Collider2D _col;

    Cinemachine.CinemachineImpulseSource _impulseSource;

    float _originalEulerZ;
    float _eulerZTargetAngle;
    [SerializeField] float _weaponSwingAmount = 90.0f;

    private void Awake()
    {
        _trail = GetComponentInChildren<TrailRenderer>();
        _renderer = GetComponent<SpriteRenderer>();
        _trail.gameObject.SetActive(false);
        _col = GetComponent<Collider2D>();
        _impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    private void Update()
    {
        if (_attacking)
        {            
            Vector3 euler = transform.parent.localEulerAngles;
            euler.z = Mathf.LerpAngle(euler.z, _eulerZTargetAngle, 15.0f * Time.deltaTime) % 360.0f;
            transform.parent.localEulerAngles = euler;

            float diffToTarget = Mathf.Abs(euler.z - _eulerZTargetAngle) % 360.0f;

            Debug.Log("Z Angle: " + euler.z);

            if(diffToTarget < 3.0f)
            {
                Debug.Log("Swing Finished");
                EndSwing();
            }

        }
    }

    public void StartSwing()
    {
        if (_attacking) return;

        _trail.gameObject.SetActive(true);
        _attacking = true;
        _col.enabled = true;


        _originalEulerZ = transform.parent.localEulerAngles.z % 360.0f;

        Debug.Log("Original Euler Z: " + _originalEulerZ);

        _eulerZTargetAngle = _originalEulerZ + (_weaponSwingAmount / 2.0f) % 360.0f;

        float angle = _originalEulerZ - (_weaponSwingAmount / 2.0f) % 360.0f;

        Debug.Log("Original Euler Z after swing offset: " + angle);
        Debug.Log("Target Angle: " + _eulerZTargetAngle);

        Vector3 eulers = transform.parent.localEulerAngles;
        eulers.z = angle;
        transform.parent.localEulerAngles = eulers;
    }

    public void EndSwing()
    {
        Vector3 eulers = transform.parent.localEulerAngles;
        eulers.z = _originalEulerZ;
        transform.parent.localEulerAngles = eulers;

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

        foreach (string s in _acceptableTags)
        {
            if (collision.CompareTag(s))
            {
                Health targetHealth = collision.GetComponent<Health>();

                if (targetHealth)
                {
                    targetHealth.TakeDamage(_config._damage);
                    
                    _impulseSource.GenerateImpulse();
                }
            }
        }
    }
}
