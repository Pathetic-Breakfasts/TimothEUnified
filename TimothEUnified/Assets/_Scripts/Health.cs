using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;
using CustomAttributes;

public class Health : MonoBehaviour
{
    [Header("General Settings")]
    [SerializeField] private float _startingHealth = 100.0f;
    [SerializeField] private float _maxHealth = 100.0f;
    [CustomAttributes.ReadOnly][SerializeField] float _currentHealth;


    [Header("Death Related Settings")]
    [SerializeField] private bool _immortal = false;
    [SerializeField] private bool _destroyOnDeath = false;

    [Header("Events")]
    [SerializeField] private UnityEvent _onDeathAction;
    [SerializeField] private UnityEvent _onDamageAction;
    [SerializeField] private UnityEvent _onHealAction;

    private bool _dead = false;
    
    public float CurrentHealth { get => _currentHealth; }
    public float MaxHealth { get => _maxHealth; }

    public float HealthRatio { get => _currentHealth / _maxHealth; }

    private void LateUpdate()
    {
        if(_dead && _destroyOnDeath)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        _currentHealth = _startingHealth;
        _currentHealth = Mathf.Clamp(_currentHealth, 0.0f, _maxHealth);
    }

    public virtual void OnDamage(float amount)
    {
        _onDamageAction.Invoke();
    }

    public virtual void OnDeath()
    {
        _onDeathAction.Invoke();
    }

    public virtual void OnHeal(float amount)
    {
        _onHealAction.Invoke();
    }


    public void TakeDamage(float amount)
    {
        _currentHealth = Mathf.Clamp(_currentHealth - amount, 0.0f, _maxHealth);
        OnDamage(amount);

        if(_currentHealth == 0.0f)
        {
            Kill();
        }
    }

    public void Kill()
    {
        _dead = true;
        OnDeath();
    }

    public void Heal(float amount)
    {
        _currentHealth = Mathf.Clamp(_currentHealth + amount, 0.0f, _maxHealth);

        OnHeal(amount);
    }
}
