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
    [SerializeField] GameObject _weaponToSwing;


    Animator _animator;


    InteractionPointManager _interactPoints;

    Mover _mover;
    Vector2 _movement;

    bool _combatMode = false;
    bool _attacking = false;
    bool _heavyAttack = false;

    public InteractDirection InteractionDirection { get => _interactionDirection; set => _interactionDirection = value; } 

    InteractDirection _interactionDirection;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _mover = GetComponent<Mover>();
        _interactPoints = GetComponentInChildren<InteractionPointManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _weaponToSwing.SetActive(false);
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

        if (_attacking)
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
        if (_attacking) return;

        _heavyAttack = Input.GetKey(KeyCode.LeftShift);

        if (Input.GetMouseButtonDown(0))
        {
            _attacking = true;

            //Play Attack Animation
            _animator.SetBool("Attacking", _attacking);
            _animator.SetBool("HeavyAttack", _heavyAttack);

            _weaponToSwing.SetActive(true);

            Vector2 mousePosition = Input.mousePosition;

            Vector2 playerScreenPos = Camera.main.WorldToScreenPoint(gameObject.transform.position);

            float horizontalDistance = playerScreenPos.x - mousePosition.x;
            float verticalDistance = playerScreenPos.y - mousePosition.y;

            float xDistToZero = horizontalDistance < 0.0f ? Mathf.Abs(horizontalDistance) : horizontalDistance;
            float yDistToZero = verticalDistance < 0.0f ? Mathf.Abs(verticalDistance) : verticalDistance;

            if (xDistToZero > yDistToZero)
            {
                float x = horizontalDistance > 0.0f ? -1.0f : 1.0f;

                _animator.SetFloat("CombatHorizontal", x);
                _animator.SetFloat("CombatVertical", 0.0f);

                _interactionDirection = x < 0.0f ? InteractDirection.Left : InteractDirection.Right;
            }
            else
            {
                float y = verticalDistance > 0.0f ? -1.0f : 1.0f;

                _animator.SetFloat("CombatHorizontal", 0.0f);
                _animator.SetFloat("CombatVertical", y);

                _interactionDirection = y < 0.0f ? InteractDirection.Down : InteractDirection.Up;
            }
        }
    }

    public void AttackFinished()
    {
        _attacking = false;
        _animator.SetBool("Attacking", _attacking);
        _animator.SetBool("HeavyAttack", _heavyAttack);
        _weaponToSwing.SetActive(false);

        InteractionPoint point = _interactPoints.GetInteractionPoint(_interactionDirection);
        foreach (GameObject obj in point.ObjectsInTrigger)
        {
            Debug.Log(obj.name);
        }
    }
}
