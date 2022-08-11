using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Configs/Weapon Config")]
public class WeaponConfig : ScriptableObject
{
    public Sprite _horizontalSprite;
    public Sprite _verticalSprite;
    public float _damage;
}
