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

    [SerializeField] int _amount = 1;

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


    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _source = GetComponent<AudioSource>();
        _shakeAnim = GetComponent<Animation>();
        _health = GetComponent<Health>();
    }

    private void Start()
    {
        _renderer.sprite = _sprites[0];

        int lengthOfSpritesArr = _sprites.Length;

        _spriteSwapTarget = _health.MaxHealth / lengthOfSpritesArr;
    }

    public bool CanDestroy(ToolConfig config)
    {
        bool returnVal = (config._type == _typeToDestroy && config._toolPower >= _minimumToolPower);

        if (!returnVal)
        {
            if(_source != null && _cantDestroyHitSfx != null)
            {
                _source.PlayOneShot(_cantDestroyHitSfx);
            }
        }

        return returnVal;
    }

    public void ProcessHit()
    {
        if(_health.CurrentHealth <= _health.MaxHealth - (_spriteSwapTarget * (_currentSpriteIndex + 1)))
        {
            _currentSpriteIndex = (_currentSpriteIndex + 1) % _sprites.Length;
            _renderer.sprite = _sprites[_currentSpriteIndex];
        }

        if (_health.CurrentHealth <= 0.0f)
        {
            if(_source != null && _destroyedSfx != null)
            {
                _source.PlayOneShot(_destroyedSfx);
            }

            Destroy(gameObject);
        }
        else
        {
            if(_source != null && _canDestroyHitSfx != null)
            {
                _source.PlayOneShot(_canDestroyHitSfx);
                _shakeAnim.Play();
            }
        }
    }
}
