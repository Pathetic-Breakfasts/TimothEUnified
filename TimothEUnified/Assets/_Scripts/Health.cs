using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField] bool _immortal = false;

    [SerializeField] float _startingHealth = 50.0f;
    [SerializeField] float _maxHealth = 100.0f;

    [SerializeField] UnityEvent _onDeathAction;
    [SerializeField] UnityEvent _onDamageAction;
    [SerializeField] UnityEvent _onHealAction;

    bool _dead = false;

    [SerializeField] bool _destroyOnDeath = false;

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

        Debug.Log(gameObject.name + " Health: " + _currentHealth);
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
