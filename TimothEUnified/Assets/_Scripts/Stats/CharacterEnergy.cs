using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterEnergy : MonoBehaviour
{
    [Min(0f)][SerializeField] float _StartingEnergy = 100.0f;

    float _currentEnergy;

    public UnityEvent _onEnergyChanged;


    // Start is called before the first frame update
    void Start()
    {
        _currentEnergy = _StartingEnergy;    
    }

    public float GetEnergyRatio()
    {
        return _currentEnergy / _StartingEnergy;
    }

    public bool IsOutOfEnergy()
    {
        return _currentEnergy == 0.0f;
    }

    public bool CanUseAmount(float amount)
    {
        return _currentEnergy - amount > 0.0f;
    }

    public void UseEnergy(float amount)
    {
        _currentEnergy = Mathf.Clamp(_currentEnergy - amount, 0.0f, _StartingEnergy);

        _onEnergyChanged.Invoke();

        UpdateUI();
    }

    public void RegainEnergy(float amount)
    {
        UseEnergy(-amount);
    }

    private void UpdateUI()
    {
        //TODO: Setup energy bar

        Debug.Log("Current Energy: " + _currentEnergy);
    }
}
