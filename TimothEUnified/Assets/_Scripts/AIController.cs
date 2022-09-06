using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{

    Transform _target;

    ActiveWeapon _weapon;

    [Header("Temporary Debug Variables")]
    [SerializeField] float _attackDistance = 1.5f;
    private void Awake()
    {
        _weapon = GetComponent<ActiveWeapon>();
    }

    // Start is called before the first frame update
    void Start()
    {
        EnemyPathfinder pathfinder = GetComponent<EnemyPathfinder>();

        PlayerInput input = FindObjectOfType<PlayerInput>();

        pathfinder.SetTargetTransform(input.transform, true, 2.5f);

        _target = input.transform;
    }



    // Update is called once per frame
    void Update()
    {
        if (!_target) return;

        Health targetHealth = _target.GetComponent<Health>();

        if (targetHealth)
        {
            float distance = Vector2.Distance(transform.position, _target.position);

            if(distance < _attackDistance)
            {
                //Calculate actual attack direction
                _weapon.Attack(InteractDirection.Left, true);

            }
        }
    }
}
