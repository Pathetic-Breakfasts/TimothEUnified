using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum InteractDirection
{
    None = -1,
    Up = 0,
    Down = 1,
    Left = 2,
    Right = 3
}


public class GameUtilities : MonoBehaviour
{
    public static InteractDirection CalculateDirection(Vector2 a, Vector2 b)
    {
        InteractDirection dir;

        //Gets the distance between the player and mouse 
        float horizontalDistance = a.x - b.x;
        float verticalDistance = a.y - b.y;

        //Finds out if the distance in the x axis or the y axis is greatest
        float xDistToZero = horizontalDistance < 0.0f ? Mathf.Abs(horizontalDistance) : horizontalDistance;
        float yDistToZero = verticalDistance < 0.0f ? Mathf.Abs(verticalDistance) : verticalDistance;

        if (xDistToZero > yDistToZero)
        {
            dir = horizontalDistance > 0.0f ? InteractDirection.Left : InteractDirection.Right;
        }
        else
        {
            dir = verticalDistance > 0.0f ? InteractDirection.Down : InteractDirection.Up;
        }

        return dir;
    }

    public static Vector2 GetDirectionVector(InteractDirection dir)
    {
        Vector2 vec = Vector2.zero;

        switch (dir)
        {
            case InteractDirection.None:
                break;
            case InteractDirection.Up:
                vec.y = 1.0f;
                break;
            case InteractDirection.Down:
                vec.y = -1.0f;
                break;
            case InteractDirection.Left:
                vec.x = -1.0f;
                break;
            case InteractDirection.Right:
                vec.x = 1.0f;
                break;
        }

        return vec;
    }
}
