using GameDevTV.Inventories;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;


[System.Serializable]
public struct DirectionSet
{
    public Sprite[] sprites;
}

[System.Serializable]
public struct CharacterSprite
{
    [Tooltip("First Array is Direction (Up Down Left Right), Second Array is Sprites (Idle, 1st,2nd,3rd Frame")]
    [SerializeField] public DirectionSet[] directions;
}


public class CharacterSpriteController : MonoBehaviour
{
    //None (-1), Up (0), Down (1), Left (2), Right (3)
    public int currentDirection {  get => _currentDirection; set { _currentDirection = value; } }
    [SerializeField] int _currentDirection = 0;

    public int CurrentSpriteIndex { get => _currentSpriteIndex; set { _currentSpriteIndex = value; } }
    [SerializeField] int _currentSpriteIndex = 0;

    CharacterSprite[] _characterSpriteArray;

    public bool FlipXAxis {  get => _flipXAxis; set { _flipXAxis = value; } }
    [SerializeField] bool _flipXAxis = false;

    
    //Helmet (0), Chest (1), Arms (2), Legs (3)
    [SerializeField] SpriteRenderer[] _spriteRenderers;

    public void Awake()
    {
        _characterSpriteArray = new CharacterSprite[4];
    }

    public void SetSpriteSet(ArmorType armorType, CharacterSprite sprite)
    {
        _characterSpriteArray[(int)armorType] = sprite;
    }

    public void UpdateSprite()
    {
        _spriteRenderers[0].sprite = _characterSpriteArray[0].directions[_currentDirection].sprites[0];
        _spriteRenderers[1].sprite = _characterSpriteArray[1].directions[_currentDirection].sprites[0];
        _spriteRenderers[2].sprite = _characterSpriteArray[2].directions[_currentDirection].sprites[_currentSpriteIndex];
        _spriteRenderers[3].sprite = _characterSpriteArray[3].directions[_currentDirection].sprites[_currentSpriteIndex];

        _spriteRenderers[0].flipX = _flipXAxis;
        _spriteRenderers[1].flipX = _flipXAxis;
        _spriteRenderers[2].flipX = _flipXAxis;
        _spriteRenderers[3].flipX = _flipXAxis;
    }
}
