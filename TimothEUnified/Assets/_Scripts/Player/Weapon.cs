using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Events;
using GameDevTV.Inventories;

public class Weapon : MonoBehaviour
{
    [Header("Weapon")]
    [SerializeField] InventoryItem _config;
    [SerializeField] string[] _acceptableTags;

    [Header("Events")]
    [SerializeField] UnityEvent _onStartSwing;
    [SerializeField] UnityEvent _onEndSwing;
    [SerializeField] UnityEvent _onHit;
    
    TrailRenderer _trail;
    SpriteRenderer _renderer;
    Collider2D _col;
    
    bool _attacking;
    bool _heavyAttack;
    float _originalEulerZ;
    float _eulerZTargetAngle;
    float _currentAttackSpeed;

    float _timeSinceLastAttack = 0.0f;
    float _attackCooldown;
    
    public bool IsAttacking { get => _attacking; }
    public InventoryItem  GetWeaponConfig { get => _config; }

    public bool HasWeapon { get => _hasWeapon; }
    bool _hasWeapon = false;

    private void Awake()
    {
        _trail = GetComponentInChildren<TrailRenderer>();
        _renderer = GetComponent<SpriteRenderer>();
        _trail.gameObject.SetActive(false);
        _col = GetComponent<Collider2D>();

        SetWeaponActive(false);
    }
    private void Update()
    {
        if (!_attacking)
        {
            _timeSinceLastAttack += Time.deltaTime;
        }

        //Swings a melee based weapon provided that we are attacking
        if (_attacking && !_config.isRanged)
        {
            //Calculates the new rotation
            Vector3 euler = transform.parent.localEulerAngles;
            euler.z = Mathf.LerpAngle(euler.z, _eulerZTargetAngle, _currentAttackSpeed * Time.deltaTime) % 360.0f;
            transform.parent.localEulerAngles = euler;

            //are we close enough to finishing a swing to say that we have completed an attack
            float diffToTarget = Mathf.Abs(euler.z - _eulerZTargetAngle) % 360.0f;
            if (diffToTarget < 3.0f) EndSwing();
        }
    }

    /// <summary>
    /// Starts a weapon attack. Angles the weapon in the direction of the target (if passed in).
    /// </summary>
    /// <param name="target">The target the weapon should look to. If null is passed in the weapon will use the parent transform</param>
    /// <param name="heavyAttack">Should this be a heavy attack. Defaulted to false</param>
    public void StartSwing(Transform target, bool heavyAttack = false)
    {

        //Stops us attacking until we have already attacked
        if (_attacking || _timeSinceLastAttack < _attackCooldown || !_hasWeapon) return;

        _renderer.enabled = true;

        _timeSinceLastAttack = 0.0f;

        //Sets if we are heavy attacking or not (affects swing speed and damage)
        _heavyAttack = heavyAttack;
        _currentAttackSpeed = heavyAttack ? _config.heavyAttackSwingRate : _config.lightAttackSwingRate;

        //If we have a target passed in then update the weapons rotation to the appropriate angle
        if (target)
        {
            Vector2 dir = (Vector2)target.position - (Vector2)transform.parent.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            transform.parent.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        //If we are using a ranged weapon, then spawn a projectile
        if (_config.isRanged)
        {
            Projectile projectile = Instantiate(_config.projectilePrefab);
            projectile.transform.position = transform.position;

            projectile.SetTarget(transform.parent.rotation, _config.damage, _acceptableTags);

            _attacking = false;
        }
        //We are using a melee weapon
        else
        {
            //Activate our trail renderer and collider
            _trail.gameObject.SetActive(true);
            _col.enabled = true;
            
            _attacking = true;

            //Invokes our start swing event. 
            _onStartSwing.Invoke();

            //Gets our current euler angle
            _originalEulerZ = transform.parent.localEulerAngles.z % 360.0f;
            //Calculates our target euler angle
            _eulerZTargetAngle = _originalEulerZ + (_config.weaponSwingDistance / 2.0f) % 360.0f;
            //Gets a offset euler angle for where the swing should start from
            float angle = _originalEulerZ - (_config.weaponSwingDistance / 2.0f) % 360.0f;
            
            //sets the weapons angle
            Vector3 eulers = transform.parent.localEulerAngles;
            eulers.z = angle;
            transform.parent.localEulerAngles = eulers;
        }
    }

    public void EndSwing()
    {
        //Sets the weapon back to default angle
        Vector3 eulers = transform.parent.localEulerAngles;
        eulers.z = _originalEulerZ;
        transform.parent.localEulerAngles = eulers;

        //Invokes our on end swing event
        _onEndSwing.Invoke();

        //Disables our trail renderer and collider
        _trail.gameObject.SetActive(false);
        _col.enabled = false;

        _attacking = false;

        _renderer.enabled = false;

    }

    public void EquipWeapon(InventoryItem config)
    {
        _config = config;

        if(_config == null)
        {
            SetWeaponActive(false);
            _hasWeapon = false;
            return;
        }

        
        _hasWeapon = true;
        SetWeaponActive(true);
        _renderer.sprite = _config.weaponSprite;
        _attackCooldown = _config.attackSpeed;
    }

    public void SetWeaponActive(bool active)
    {
        //_renderer.enabled = active;
        _trail.enabled  = active;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Stops us from causing damage while the weapon is not being swung/used
        if (!_attacking) return;

        //Cycles through the acceptable tags on the weapon
        foreach (string s in _acceptableTags)
        {
            //Checks if the colliding object is acceptable for us to hit
            if (collision.CompareTag(s))
            {
                //Checks for a health component
                Health targetHealth = collision.GetComponent<Health>();

                if (targetHealth)
                {
                    //Calculates damage based on if this is a heavy attack or not
                    float damage = _heavyAttack ? _config.damage * _config.heavyAttackDamageBoost : _config.damage;

                    //Deals damage
                    targetHealth.TakeDamage(damage);

                    //Invokes the OnHit event, causes the camera shake for the player etc.
                    _onHit.Invoke();
                }
            }
        }
    }
}
