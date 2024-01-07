using GameDevTV.Inventories;
using UnityEngine;


[System.Serializable]
public struct DirectionSet
{
    public Sprite[] sprites;

    public DirectionSet(int num = 4)
    {
        sprites = new Sprite[num];
    }
}

[System.Serializable]
public struct BodyPieceSpriteCollection
{
    [Tooltip("First Array is Direction (Up Down Left Right), Second Array is Sprites (Idle, 1st,2nd,3rd Frame")]
    [SerializeField] public DirectionSet[] directions;
    [SerializeField] public DirectionSet[] alternativeDirections;

    [SerializeField] public bool canUseAlternativeSprites;

    public BodyPieceSpriteCollection(int numDirs = 4, bool canUse = false)
    {
        canUseAlternativeSprites = canUse;
        directions = new DirectionSet[4];
        alternativeDirections = new DirectionSet[4];
        for(int i = 0; i < numDirs; i++)
        {
            directions[i] = new DirectionSet(numDirs);
            alternativeDirections[i] = new DirectionSet(numDirs);
        }
    }
}


public class CharacterSpriteController : MonoBehaviour
{
    //None (-1), Up (0), Down (1), Left (2), Right (3)
    public int currentDirection {  get => _currentDirection; set { _currentDirection = value; } }
    [HideInInspector][SerializeField] int _currentDirection = 0;

    public int CurrentSpriteIndex { get => _currentSpriteIndex; set { _currentSpriteIndex = value; } }
    [HideInInspector][SerializeField] int _currentSpriteIndex = 0;

    public bool FlipXAxis {  get => _flipXAxis; set { _flipXAxis = value; } }
    [HideInInspector][SerializeField] bool _flipXAxis = false;

    public bool UseAlternativeSpriteSet { get => _useAlternativeSpriteSet; set => _useAlternativeSpriteSet = value; }
    [HideInInspector][SerializeField] bool _useAlternativeSpriteSet = false;

    //Helmet (0), Chest (1), Arms (2), Legs (3)
    [SerializeField] SpriteRenderer[] _spriteRenderers;
    [SerializeField] SpriteRenderer[] _overlayedSpriteRenderers;

    BodyPieceSpriteCollection[] _characterSpriteArray;
    BodyPieceSpriteCollection[] _overlayedCharacterSpriteArray;

    //////////////////////////////////////////////////
    public void Awake()
    {
        _characterSpriteArray = new BodyPieceSpriteCollection[4];
        _overlayedCharacterSpriteArray = new BodyPieceSpriteCollection[4];

        for(int i  = 0; i < 4; i++)
        {
            _characterSpriteArray[i] = new BodyPieceSpriteCollection(4);
            _overlayedCharacterSpriteArray[i] = new BodyPieceSpriteCollection(4);
        }
    }

    //////////////////////////////////////////////////
    public void SetSpriteSet(ArmorType armorType, BodyPieceSpriteCollection sprite)
    {
        _characterSpriteArray[(int)armorType] = sprite;
    }

    //////////////////////////////////////////////////
    public void SetOverlaySpriteSet(ArmorType armorType, BodyPieceSpriteCollection overlaySprite)
    {
        _overlayedCharacterSpriteArray[(int)armorType] = overlaySprite;
    }

    //////////////////////////////////////////////////
    public void ClearOverlaySpriteSets()
    {
        _overlayedCharacterSpriteArray = new BodyPieceSpriteCollection[4];
        for(int i = 0; i < 4; i++)
        {
            _overlayedCharacterSpriteArray[i] = new BodyPieceSpriteCollection(4);
        }
    }

    //////////////////////////////////////////////////
    public void UpdateSprite()
    {
        foreach(SpriteRenderer spriteRenderer in _spriteRenderers) { spriteRenderer.flipX = _flipXAxis; }
        foreach(SpriteRenderer spriteRenderer in _overlayedSpriteRenderers) {  spriteRenderer.flipX = _flipXAxis;}

        //Head, Chest, Arms, Legs
        if(_useAlternativeSpriteSet)
        {
            //Head
            if (_characterSpriteArray[0].canUseAlternativeSprites)
            {
                _spriteRenderers[0].sprite = _characterSpriteArray[0].alternativeDirections[_currentDirection].sprites[0];
            }
            else
            {
                _spriteRenderers[0].sprite = _characterSpriteArray[0].directions[_currentDirection].sprites[0];
            }

            //Chest
            if (_characterSpriteArray[1].canUseAlternativeSprites)
            {
                _spriteRenderers[1].sprite = _characterSpriteArray[1].alternativeDirections[_currentDirection].sprites[0];
            }
            else
            {
                _spriteRenderers[1].sprite = _characterSpriteArray[1].directions[_currentDirection].sprites[0];
            }

            //Arms
            if (_characterSpriteArray[2].canUseAlternativeSprites)
            {
                _spriteRenderers[2].sprite = _characterSpriteArray[2].alternativeDirections[_currentDirection].sprites[_currentSpriteIndex];
            }
            else
            {
                _spriteRenderers[2].sprite = _characterSpriteArray[2].directions[_currentDirection].sprites[_currentSpriteIndex];
            }

            //Legs
            if (_characterSpriteArray[3].canUseAlternativeSprites)
            {
                _spriteRenderers[3].sprite = _characterSpriteArray[3].alternativeDirections[_currentDirection].sprites[_currentSpriteIndex];
            }
            else
            {
                _spriteRenderers[3].sprite = _characterSpriteArray[3].directions[_currentDirection].sprites[_currentSpriteIndex];
            }

            //Overlayed Sprites (Armor Sets)
            //Head
            if (_overlayedCharacterSpriteArray[0].canUseAlternativeSprites)
            {
                _overlayedSpriteRenderers[0].sprite = _overlayedCharacterSpriteArray[0].alternativeDirections[_currentDirection].sprites[0];
            }
            else
            {
                _overlayedSpriteRenderers[0].sprite = _overlayedCharacterSpriteArray[0].directions[_currentDirection].sprites[0];
            }

            //Chest
            if (_overlayedCharacterSpriteArray[1].canUseAlternativeSprites)
            {
                _overlayedSpriteRenderers[1].sprite = _overlayedCharacterSpriteArray[1].alternativeDirections[_currentDirection].sprites[0];
            }
            else
            {
                _overlayedSpriteRenderers[1].sprite = _overlayedCharacterSpriteArray[1].directions[_currentDirection].sprites[0];
            }

            //Arms
            if (_overlayedCharacterSpriteArray[2].canUseAlternativeSprites)
            {
                _overlayedSpriteRenderers[2].sprite = _overlayedCharacterSpriteArray[2].alternativeDirections[_currentDirection].sprites[_currentSpriteIndex];
            }
            else
            {
                _overlayedSpriteRenderers[2].sprite = _overlayedCharacterSpriteArray[2].directions[_currentDirection].sprites[_currentSpriteIndex];
            }

            //Legs
            if (_overlayedCharacterSpriteArray[3].canUseAlternativeSprites)
            {
                _overlayedSpriteRenderers[3].sprite = _overlayedCharacterSpriteArray[3].alternativeDirections[_currentDirection].sprites[_currentSpriteIndex];
            }
            else
            {
                _overlayedSpriteRenderers[3].sprite = _overlayedCharacterSpriteArray[3].directions[_currentDirection].sprites[_currentSpriteIndex];
            }
        }
        else
        {
            _spriteRenderers[0].sprite = _characterSpriteArray[0].directions[_currentDirection].sprites[0];
            _spriteRenderers[1].sprite = _characterSpriteArray[1].directions[_currentDirection].sprites[0];
            _spriteRenderers[2].sprite = _characterSpriteArray[2].directions[_currentDirection].sprites[_currentSpriteIndex];
            _spriteRenderers[3].sprite = _characterSpriteArray[3].directions[_currentDirection].sprites[_currentSpriteIndex];

            _overlayedSpriteRenderers[0].sprite = _overlayedCharacterSpriteArray[0].directions[_currentDirection].sprites[0];
            _overlayedSpriteRenderers[1].sprite = _overlayedCharacterSpriteArray[1].directions[_currentDirection].sprites[0];
            _overlayedSpriteRenderers[2].sprite = _overlayedCharacterSpriteArray[2].directions[_currentDirection].sprites[_currentSpriteIndex];
            _overlayedSpriteRenderers[3].sprite = _overlayedCharacterSpriteArray[3].directions[_currentDirection].sprites[_currentSpriteIndex];
        }
    }
}
