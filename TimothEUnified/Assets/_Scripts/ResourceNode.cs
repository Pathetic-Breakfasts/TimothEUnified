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

    int _currentSpriteIndex = 0;

    [SerializeField] Sprite[] _sprites;
    [SerializeField] float _minimumToolPower = 30.0f;
    [SerializeField] ToolType _typeToDestroy;

    float _currentHealth;

    float _spriteSwapTarget;

    SpriteRenderer _renderer;

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        _currentHealth = _nodeMaxHealth;
        _renderer.sprite = _sprites[0];

        int lengthOfSpritesArr = _sprites.Length;

        _spriteSwapTarget = _nodeMaxHealth / lengthOfSpritesArr;
    }

    public bool CanDestroy(ToolConfig config)
    {
        return (config._type == _typeToDestroy && config._toolPower >= _minimumToolPower);
    }

    public void TakeHit(float damage)
    {
        _currentHealth -= damage;



        if(_currentHealth <= _nodeMaxHealth - (_spriteSwapTarget * (_currentSpriteIndex + 1)))
        {
            _currentSpriteIndex = (_currentSpriteIndex + 1) % _sprites.Length;
            _renderer.sprite = _sprites[_currentSpriteIndex];
        }

        if (_currentHealth <= 0.0f)
        {
            Destroy(gameObject);
        }
    }
}
