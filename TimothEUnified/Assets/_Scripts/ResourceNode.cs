using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ResourceNodeType
{
    Wood,
    Stone,
    Coal,
    Iron,
    Copper
}

public class ResourceNode : MonoBehaviour
{
    [SerializeField] float _nodeMaxHealth = 100.0f;
    [SerializeField] ResourceNodeType _type;

    [SerializeField] int _amount = 1;

    [SerializeField] float _minimumToolPower = 30.0f;
    [SerializeField] ToolType _typeToDestroy;

    float _currentHealth;

    private void Start()
    {
        _currentHealth = _nodeMaxHealth;
    }

    public bool CanDestroy(ToolConfig config)
    {
        return (config._type == _typeToDestroy && config._toolPower > _minimumToolPower);
    }

    public void TakeHit(float damage)
    {
        _currentHealth -= damage;

        if (_currentHealth <= 0.0f)
        {
            Destroy(gameObject);
        }
    }
}
