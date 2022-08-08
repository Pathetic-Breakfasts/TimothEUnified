using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] GameObject _weaponToSwing;


    Animator _animator;

    bool _attacking = false;

    float _attackingDuration = 1.0f;
    float _attackingTimer = 0.0f;

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
        AttackingInput();
    }

    private void AttackingInput()
    {
        if (_attacking)
        {
            _attackingTimer += Time.deltaTime;

            if (_attackingTimer > _attackingDuration)
            {
                _attackingTimer = 0.0f;
                _attacking = false;
                _animator.SetBool("Attacking", _attacking);
                _weaponToSwing.SetActive(false);
            }
        }

        if (_attacking) return;

        if (Input.GetMouseButtonDown(0))
        {
            _attacking = true;

            //Play Attack Animation
            _animator.SetBool("Attacking", _attacking);

            _weaponToSwing.SetActive(true);
        }
    }
}
