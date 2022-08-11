using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveWeapon : MonoBehaviour
{
    [SerializeField] WeaponConfig _config;
    [SerializeField] SpriteRenderer _swordSpriteRenderer;

    Fighter _fighter;

    private void Awake()
    {
        _fighter = GetComponent<Fighter>();
        ChangeWeapon(_config);
    }

    public void ChangeWeapon(WeaponConfig newConfig)
    {
        _config = newConfig;
        _swordSpriteRenderer.sprite = _config._horizontalSprite;
    }

    public void Attack(InteractDirection _interactionDirection, bool heavyAttack)
    {
        if (heavyAttack)
        {
            if(_interactionDirection == InteractDirection.Down || _interactionDirection == InteractDirection.Up)
            {
                _swordSpriteRenderer.sprite = _config._verticalSprite;
            }
            else
            {
                _swordSpriteRenderer.sprite = _config._horizontalSprite;
            }
        }
        else
        {
            _swordSpriteRenderer.sprite = _config._horizontalSprite;
        }

        _fighter.Attack(_interactionDirection, heavyAttack, _config);
    }
}
