using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] GameObject _weaponToSwing;


    Animator _animator;


    float _attackingDuration = 0.5f;
    float _attackingTimer = 0.0f;

    bool _combatMode = false;
    bool _attacking = false;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _weaponToSwing.SetActive(false);
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

    }

    private void AttackingInput()
    {
        //If we are in the attacking state
        if (_attacking)
        {
            //Increment the attack timer by frame time
            _attackingTimer += Time.deltaTime;

            //If we have exceeded an attacks duration
            if (_attackingTimer > _attackingDuration)
            {
                //Reset the timer and the attacking states
                _attackingTimer = 0.0f;
                _attacking = false;
                _animator.SetBool("Attacking", _attacking);
                _animator.SetBool("HeavyAttack", true);
                _weaponToSwing.SetActive(false);
            }
        }

        //stops us from being able to do multiple attacks in one go
        if (_attacking) return;

        if (Input.GetMouseButtonDown(0))
        {
            _attacking = true;

            //Play Attack Animation
            _animator.SetBool("Attacking", _attacking);
            _animator.SetBool("HeavyAttack", true);

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
            }
            else
            {
                float y = verticalDistance > 0.0f ? -1.0f : 1.0f;

                _animator.SetFloat("CombatHorizontal", 0.0f);
                _animator.SetFloat("CombatVertical", y);
            }
        }
    }
}
