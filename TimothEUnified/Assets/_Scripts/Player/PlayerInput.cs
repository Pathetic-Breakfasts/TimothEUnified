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
    Fighter _fighter;
    Animator _animator;

    Mover _mover;
    ActiveWeapon _activeWeapon;
    ActiveTool _activeTool;

    Vector2 _movement;
    Vector2 _mousePosAtClick;

    bool _combatMode = false;
    //bool _usingTool = false;

    public InteractDirection InteractionDirection { get => _interactionDirection; set => _interactionDirection = value; } 

    InteractDirection _interactionDirection;

    InteractionPointManager _interactPoints;

    //TODO: This is temporary until we have a hot bar setup
    [SerializeField] ToolConfig _axeConfig;
    [SerializeField] ToolConfig _pickaxeConfig;
    [SerializeField] ToolConfig _hoeConfig;

    private void Awake()
    {
        _activeWeapon = GetComponent<ActiveWeapon>();
        _activeTool = GetComponent<ActiveTool>();

        _animator = GetComponent<Animator>();
        _mover = GetComponent<Mover>();
        _fighter = GetComponent<Fighter>();
        _interactPoints = GetComponentInChildren<InteractionPointManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _interactionDirection = InteractDirection.None;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _activeTool.ChangeTool(_axeConfig);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _activeTool.ChangeTool(_pickaxeConfig);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            _activeTool.ChangeTool(_hoeConfig);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _combatMode = !_combatMode;

            _animator.SetBool("InCombatMode", _combatMode);
        }

        if (_combatMode)
        {
            AttackingInput();
        }
        else
        {
            ToolInput();
        }

        if (_fighter.IsAttacking || _activeTool.UsingTool)
        {
            _movement = Vector2.zero;
            _animator.SetFloat("Speed", 0.0f);
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
        if (_fighter.IsAttacking || _activeTool.UsingTool) return;


        if (Input.GetMouseButtonDown(0))
        {
            //Finds the pixel position of the mouse and the player
            Vector2 mousePosition = Input.mousePosition;
            Vector2 playerScreenPos = Camera.main.WorldToScreenPoint(gameObject.transform.position);

            _interactionDirection = CalculateDirection(playerScreenPos, mousePosition);

            //Should we heavy attack
            bool heavyAttack = Input.GetKey(KeyCode.LeftShift);
            _activeWeapon.Attack(_interactionDirection, heavyAttack);
        }
    }

    private void ToolInput()
    {
        if (_activeTool.UsingTool || _fighter.IsAttacking) return;

        if (Input.GetMouseButtonDown(0))
        {
            _mousePosAtClick = Input.mousePosition;
            Vector2 playerScreenPos = Camera.main.WorldToScreenPoint(gameObject.transform.position);

            _interactionDirection = CalculateDirection(playerScreenPos, _mousePosAtClick);
            Vector2 dir = GetDirectionVector(_interactionDirection);

            _activeTool.UseTool(_interactionDirection, _mousePosAtClick);

            _animator.SetFloat("CombatHorizontal", dir.x);
            _animator.SetFloat("CombatVertical", dir.y);
            _animator.SetFloat("LastHorizontal", dir.x);
            _animator.SetFloat("LastVertical", dir.y);
        }
    }

    public InteractDirection CalculateDirection(Vector2 a, Vector2 b)
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

    private Vector2 GetDirectionVector(InteractDirection dir)
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
