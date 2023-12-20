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

    public BodyPieceSpriteCollection(int numDirs = 4)
    {
        directions = new DirectionSet[4];
        for(int i = 0; i < numDirs; i++)
        {
            directions[i] = new DirectionSet(numDirs);
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

    //Helmet (0), Chest (1), Arms (2), Legs (3)
    [SerializeField] SpriteRenderer[] _spriteRenderers;
    [SerializeField] SpriteRenderer[] _overlayedSpriteRenderers;

    BodyPieceSpriteCollection[] _characterSpriteArray;
    BodyPieceSpriteCollection[] _overlayedCharacterSpriteArray;

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

    public void SetSpriteSet(ArmorType armorType, BodyPieceSpriteCollection sprite)
    {
        _characterSpriteArray[(int)armorType] = sprite;
    }

    public void SetOverlaySpriteSet(ArmorType armorType, BodyPieceSpriteCollection overlaySprite)
    {
        _overlayedCharacterSpriteArray[(int)armorType] = overlaySprite;
    }

    public void UpdateSprite()
    {
        _spriteRenderers[0].sprite = _characterSpriteArray[0].directions[_currentDirection].sprites[0];
        _spriteRenderers[1].sprite = _characterSpriteArray[1].directions[_currentDirection].sprites[0];
        _spriteRenderers[2].sprite = _characterSpriteArray[2].directions[_currentDirection].sprites[_currentSpriteIndex];
        _spriteRenderers[3].sprite = _characterSpriteArray[3].directions[_currentDirection].sprites[_currentSpriteIndex];

        foreach(SpriteRenderer spriteRenderer in _spriteRenderers) { spriteRenderer.flipX = _flipXAxis; }

        _overlayedSpriteRenderers[0].sprite = _overlayedCharacterSpriteArray[0].directions[_currentDirection].sprites[0];
        _overlayedSpriteRenderers[1].sprite = _overlayedCharacterSpriteArray[1].directions[_currentDirection].sprites[0];
        _overlayedSpriteRenderers[2].sprite = _overlayedCharacterSpriteArray[2].directions[_currentDirection].sprites[_currentSpriteIndex];
        _overlayedSpriteRenderers[3].sprite = _overlayedCharacterSpriteArray[3].directions[_currentDirection].sprites[_currentSpriteIndex];

        foreach(SpriteRenderer spriteRenderer in _overlayedSpriteRenderers) {  spriteRenderer.flipX = _flipXAxis;}
    }
}
