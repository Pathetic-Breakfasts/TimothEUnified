using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AIController : MonoBehaviour
{
    EnemyPathfinder _pathfinder;
    Transform _playerTrans;

    [Header("Temporary Debug Variables")]
    [SerializeField] float _attackDistance = 1.5f;


    Transform _target;
    List<GameObject> _targets;
    List<GameObject> _objectsToRemove;

    Weapon _aiWeapon;

    private void Awake()
    {
        _aiWeapon = GetComponentInChildren<Weapon>();
        _pathfinder = GetComponent<EnemyPathfinder>();
        _playerTrans = FindObjectOfType<PlayerInput>().transform;
        _objectsToRemove = new List<GameObject>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //TODO: Automate this process with a target tag array.
        _targets = new List<GameObject>();
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Defence Structure"))
        {
            _targets.Add(obj);
        }
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Offence Structure"))
        {
            _targets.Add(obj);
        }
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Valuable Structure"))
        {
            _targets.Add(obj);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //TODO: Change this to check a pathfinding distance to the player instead of a euclidean distance
        float distanceToPlayer = Vector2.Distance(transform.position, _playerTrans.position);
        if (distanceToPlayer < 2.5f)
        {
            AttemptAttackOnTransform(_playerTrans);
        }
        else
        {
            if (_target == null)
            {
                FindTarget();
            }

            AttemptAttackOnTransform(_target);
        }
    }

    private void AttemptAttackOnTransform(Transform target)
    {
        if (!target) return;

        Health targetHealth = target.GetComponent<Health>();

        if (targetHealth)
        {
            float distance = Vector2.Distance(transform.position, target.position);

            if (distance < _attackDistance)
            {
                _aiWeapon.StartSwing(target);
            }
        }
    }

    private void LateUpdate()
    {
        foreach (GameObject obj in _objectsToRemove)
        {
            _targets.Remove(obj);
        }
        _objectsToRemove.Clear();
    }

    private void FindTarget()
    {
        float closestDistance = float.MaxValue;
        GameObject closestObj = null;
        foreach (GameObject obj in _targets)
        {
            if (obj == null)
            {
                _objectsToRemove.Add(obj);
                continue;
            }

            float distance = Vector2.Distance(transform.position, obj.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestObj = obj;
            }
        }

        if (closestObj != null)
        {
            SetTarget(closestObj.transform);
        }
        else
        {
            SetTarget(_playerTrans);
        }
    }

    public void SetTarget(Transform target)
    {
        _target = target;
        _pathfinder.SetTargetTransform(target, true, 2.5f);
    }
}
