using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CurrencyStore : MonoBehaviour
{
    public int CurrencyAmount { get => _currencyAmount; }
    int _currencyAmount = 0;

    public UnityEvent OnCurrencyAmountChanged;

#if DEBUG
    [SerializeField] bool _logCurrency = false;
#endif 

    public void GainMoney(int amount)
    {
        _currencyAmount += amount;

        if (OnCurrencyAmountChanged != null)
        {
            OnCurrencyAmountChanged.Invoke();
        }

#if DEBUG
        if (_logCurrency)
        {
            Debug.Log(gameObject.name + " is gaining " + amount + " current balance is now: " + _currencyAmount);
        }
#endif 

    }

    public void SpendMoney(int amount)
    {
        _currencyAmount -= amount;

        if (OnCurrencyAmountChanged != null)
        {
            OnCurrencyAmountChanged.Invoke();
        }

#if DEBUG
        if (_logCurrency)
        {
            Debug.Log(gameObject.name + " is spending " + amount + " current balance is now: " + _currencyAmount);
        }
#endif 
    }

    public bool CanAfford(int amount)
    {
        return amount <= _currencyAmount;
    }
}
