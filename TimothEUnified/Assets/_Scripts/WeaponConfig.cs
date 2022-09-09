using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Configs/Weapon Config")]
public class WeaponConfig : ScriptableObject
{
    [Header("General Settings")]
    public float _damage = 10.0f;
    public float _attackRange = 1.5f;
    public float _attackSpeed = 1.5f;

    [Header("Graphics Settings")]
    public Sprite _sprite;

    [Header("Ranged Weapon Settings")]
    public bool _isRanged = false;
    public Projectile _projectilePrefab;
}
