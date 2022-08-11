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

    WeaponConfig _current;
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

    public void Attack(InteractDirection desiredDirection, bool heavyAttack, WeaponConfig config)
    {
        _current = config;
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



    //Called by animation event
    public void AttackFinished()
    {
        //No longer attacking
        _attacking = false;
        _animator.SetBool("Attacking", _attacking);
        _weaponToSwing.SetActive(false);

        //Gets the desired interaction point
        InteractionPoint point = _interactPoints.GetInteractionPoint(_interactionDirection);
        foreach (GameObject obj in point.ObjectsInTrigger)
        {
            Health health = obj.GetComponent<Health>();

            if (health)
            {
                health.TakeDamage(_current._damage); //TODO: Swap this for weapon damage
            }
        }
    }
}
