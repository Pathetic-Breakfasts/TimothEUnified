using GameDevTV.Inventories;
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
    [SerializeField] ResourceNodeType _type;

    [SerializeField] LootTable _droppedItemTable = null;
    [Min(0f)][SerializeField] int _numberOfGuranteedItems = 1;
    [Min(0f)][SerializeField] int _numberOfAdditionalItems = 0;


    int _currentSpriteIndex = 0;
    Animation _shakeAnim;


    [SerializeField] float _minimumToolPower = 30.0f;
    [SerializeField] ToolType _typeToDestroy;

    [Header("Graphics Settings")]
    [SerializeField] Sprite[] _sprites;

    [Header("Sound Settings")]
    [SerializeField] AudioClip _canDestroyHitSfx;
    [SerializeField] AudioClip _cantDestroyHitSfx;
    [SerializeField] AudioClip _destroyedSfx;

    Health _health;

    float _spriteSwapTarget;

    
    SpriteRenderer _renderer;
    AudioSource _source;

    //////////////////////////////////////////////////
    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _source = GetComponent<AudioSource>();
        _shakeAnim = GetComponent<Animation>();
        _health = GetComponent<Health>();
    }

    //////////////////////////////////////////////////
    private void Start()
    {
        _renderer.sprite = _sprites[0];

        int lengthOfSpritesArr = _sprites.Length;

        _spriteSwapTarget = _health.MaxHealth / lengthOfSpritesArr;
    }

    //////////////////////////////////////////////////
    public bool CanDestroy(InventoryItem config)
    {
        bool returnVal = (config.toolType == _typeToDestroy && config.toolPower >= _minimumToolPower);

        if (!returnVal)
        {
            if(_source && _cantDestroyHitSfx)
            {
                _source.PlayOneShot(_cantDestroyHitSfx);
            }
        }

        return returnVal;
    }

    //Called via OnDamage from the Health Component
    //////////////////////////////////////////////////
    public void ProcessHit()
    {
        if(_health.CurrentHealth <= _health.MaxHealth - (_spriteSwapTarget * (_currentSpriteIndex + 1)))
        {
            _currentSpriteIndex = (_currentSpriteIndex + 1) % _sprites.Length;
            _renderer.sprite = _sprites[_currentSpriteIndex];
        }

        if (_health.CurrentHealth <= 0.0f)
        {
            if(_source && _destroyedSfx)
            {
                _source.PlayOneShot(_destroyedSfx);
            }

            if(_droppedItemTable)
            {
                _droppedItemTable.SpawnItems(transform.position, _numberOfGuranteedItems, _numberOfAdditionalItems);
            }
            else
            {
                Debug.LogWarning(gameObject.name + " has no Loot Table Set!");
            }

            Destroy(gameObject);
        }
        else
        {
            if(_source && _canDestroyHitSfx)
            {
                _source.PlayOneShot(_canDestroyHitSfx);
                _shakeAnim.Play();
            }
        }
    }
}
