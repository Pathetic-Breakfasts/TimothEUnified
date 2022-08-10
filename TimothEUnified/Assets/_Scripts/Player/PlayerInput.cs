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

public class PlayerInput : MonoBehaviour
{
    //[SerializeField] GameObject _weaponToSwing;


    Fighter _fighter;
    Animator _animator;


    Mover _mover;
    Vector2 _movement;

    bool _combatMode = false;

    public InteractDirection InteractionDirection { get => _interactionDirection; set => _interactionDirection = value; } 

    InteractDirection _interactionDirection;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _mover = GetComponent<Mover>();
        _fighter = GetComponent<Fighter>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _interactionDirection = InteractDirection.None;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _combatMode = !_combatMode;

            _animator.SetBool("InCombatMode", _combatMode);
        }

        if (_combatMode)
        {
            AttackingInput();
        }

        if (_fighter.IsAttacking)
        {
            _movement = Vector2.zero;
            return;
        }

        _movement.x = Input.GetAxisRaw("Horizontal");
        _movement.y = Input.GetAxisRaw("Vertical");

        _animator.SetFloat("Horizontal", _movement.x);
        _animator.SetFloat("Vertical", _movement.y);
        _animator.SetFloat("Speed", _movement.sqrMagnitude);
    }

    private void FixedUpdate()
    {
        if (_movement == Vector2.zero) return;

        _mover.Move(_movement);

        _animator.SetFloat("LastHorizontal", _movement.x);
        _animator.SetFloat("LastVertical", _movement.y);
    }

    private void AttackingInput()
    {
        //stops us from being able to do multiple attacks in one go
        if (_fighter.IsAttacking) return;


        if (Input.GetMouseButtonDown(0))
        {
            //Finds the pixel position of the mouse and the player
            Vector2 mousePosition = Input.mousePosition;
            Vector2 playerScreenPos = Camera.main.WorldToScreenPoint(gameObject.transform.position);

            //Gets the distance between the player and mouse 
            float horizontalDistance = playerScreenPos.x - mousePosition.x;
            float verticalDistance = playerScreenPos.y - mousePosition.y;

            //Finds out if the distance in the x axis or the y axis is greatest
            float xDistToZero = horizontalDistance < 0.0f ? Mathf.Abs(horizontalDistance) : horizontalDistance;
            float yDistToZero = verticalDistance < 0.0f ? Mathf.Abs(verticalDistance) : verticalDistance;

            //Attack on the left or right
            if (xDistToZero > yDistToZero)
            {
                _interactionDirection = horizontalDistance > 0.0f ? InteractDirection.Left : InteractDirection.Right;
            }
            //attack up or down
            else
            {
                _interactionDirection = verticalDistance > 0.0f ? InteractDirection.Down : InteractDirection.Up;
            }


            //Should we heavy attack
            bool heavyAttack = Input.GetKey(KeyCode.LeftShift);
            _fighter.Attack(_interactionDirection, heavyAttack);
        }
    }

}
