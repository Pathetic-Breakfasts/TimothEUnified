using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour
{
    bool _attacking = false;

    Animator _animator;
    InteractionPointManager _interactPoints;

    [SerializeField] GameObject _weaponToSwing;
    InteractDirection _interactionDirection;

    public bool IsAttacking { get => _attacking; }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _interactPoints = GetComponentInChildren<InteractionPointManager>();
    }

    private void Start()
    {
        _weaponToSwing.SetActive(false);

    }

    public void Attack(InteractDirection desiredDirection, bool heavyAttack)
    {
        _weaponToSwing.SetActive(true);
        _interactionDirection = desiredDirection;
        _attacking = true;

        float x = 0.0f;
        float y = 0.0f;

        //Decide the animation values based on the attack direction
        switch (desiredDirection)
        {
            case InteractDirection.None:
                break;
            case InteractDirection.Up:
                y = 1.0f;
                break;
            case InteractDirection.Down:
                y = -1.0f;
                break;
            case InteractDirection.Left:
                x = -1.0f;
                break;
            case InteractDirection.Right:
                x = 1.0f;
                break;
        }

        _animator.SetFloat("CombatHorizontal", x);
        _animator.SetFloat("CombatVertical", y);

        _animator.SetBool("Attacking", _attacking);
        _animator.SetBool("HeavyAttack", heavyAttack);
    }


    public void AttackFinished()
    {
        _attacking = false;
        _animator.SetBool("Attacking", _attacking);
        _weaponToSwing.SetActive(false);

        InteractionPoint point = _interactPoints.GetInteractionPoint(_interactionDirection);
        foreach (GameObject obj in point.ObjectsInTrigger)
        {
            Debug.Log(obj.name);
        }
    }
}
