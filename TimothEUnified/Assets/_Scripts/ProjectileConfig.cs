using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Combat/ProjectileConfig")]
public class ProjectileConfig : ScriptableObject
{
    public Sprite projectileSprite = null;

    [Min(0.1f)] public float movementSpeed = 5.0f;

    [Min(0.1f)] public float lifetime = 10.0f;
}
