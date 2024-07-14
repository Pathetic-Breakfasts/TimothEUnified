using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TimothE.Gameplay.Interactables;

//////////////////////////////////////////////////
public class UpdateWeaponPositionByDirection : MonoBehaviour
{

    [SerializeField] Transform _weaponAttachPoint;
    Weapon _weapon;
    EnemyPathfinder _pathfinder;

    //////////////////////////////////////////////////
    private void Awake()
    {
        _weapon = GetComponentInChildren<Weapon>();
        _pathfinder = GetComponent<EnemyPathfinder>();
    }

    //////////////////////////////////////////////////
    void Update()
    {
        if (!_weapon.IsAttacking)
        {
            Vector3 eulers = _weaponAttachPoint.localEulerAngles;

            InteractDirection dir = CalculateDirection(_pathfinder.DirectionVector);

            float zRot = 0.0f;
            switch (dir)
            {
                case InteractDirection.None:
                    break;
                case InteractDirection.Up:
                    zRot = 150.0f;
                    break;
                case InteractDirection.Down:
                    zRot = 300.0f;
                    break;
                case InteractDirection.Left:
                    zRot = 225.0f;
                    break;
                case InteractDirection.Right:
                    zRot = 370.0f;
                    break;
            }

            eulers.z = zRot;
            _weaponAttachPoint.localEulerAngles = eulers;
        }
    }

    //////////////////////////////////////////////////
    public InteractDirection CalculateDirection(Vector2 a)
    {
        InteractDirection dir;

        float horizontal = a.x;
        float vertical = a.y;

        //Finds out if the distance in the x axis or the y axis is greatest
        float xDistToZero = horizontal < 0.0f ? Mathf.Abs(horizontal) : horizontal;
        float yDistToZero = vertical < 0.0f ? Mathf.Abs(vertical) : vertical;

        if (xDistToZero > yDistToZero)
        {
            dir = horizontal < 0.0f ? InteractDirection.Left : InteractDirection.Right;
        }
        else
        {
            dir = vertical < 0.0f ? InteractDirection.Down : InteractDirection.Up;
        }

        return dir;
    }
}
