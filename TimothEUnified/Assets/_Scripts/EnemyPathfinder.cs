using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathfinder : MonoBehaviour
{
    [SerializeField] Transform _target;

    [SerializeField] float _moveSpeed;

    [SerializeField] float _speed = 200.0f;
    [SerializeField] float _nextWaypointDistance = 3f;

    Path _path;
    Seeker _seeker;
    int _currentWaypoint;
    bool _reachedEndOfPath = false;

    
    Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _seeker = GetComponent<Seeker>();
        
    }

    private void Start()
    {
        InvokeRepeating("UpdatePath", 0f, 1f);
    }

    void UpdatePath()
    {
        if (_seeker.IsDone())
        {
            _seeker.StartPath(_rb.position, _target.position, OnPathComplete);
        }
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            _path = p;
            _currentWaypoint = 0;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(_path == null)
        {
            return;
        }    

        if(_currentWaypoint >= _path.vectorPath.Count)
        {
            _reachedEndOfPath = true;
            return;
        }
        else
        {
            _reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2)_path.vectorPath[_currentWaypoint] - _rb.position).normalized;
        Vector2 force = direction * _moveSpeed * Time.fixedDeltaTime;

        _rb.AddForce(force);

        float distance = Vector2.Distance(_rb.position, _path.vectorPath[_currentWaypoint]);
        if(distance < _nextWaypointDistance)
        {
            _currentWaypoint++;
        }


    }
}
