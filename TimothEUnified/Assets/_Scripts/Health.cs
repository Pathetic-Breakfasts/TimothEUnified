using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [Header("General Settings")]
    [SerializeField] float _startingHealth = 50.0f;
    [SerializeField] float _maxHealth = 100.0f;

    [Header("Death Related Settings")]
    [SerializeField] bool _immortal = false;
    [SerializeField] bool _destroyOnDeath = false;

    [Header("Events")]
    [SerializeField] UnityEvent _onDeathAction;
    [SerializeField] UnityEvent _onDamageAction;
    [SerializeField] UnityEvent _onHealAction;


    bool _dead = false;
    float _currentHealth;

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

        if(_currentHealth == 0.0f)
        {
            Kill();
        }

        OnDamage(amount);
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
