using Pathfinding;
using UnityEngine;

//Adds Rigidbody2D and Seeker components to this object by default if they do not exist
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Seeker))]
public class EnemyPathfinder : MonoBehaviour
{
    [Header("Destination Settings")]
    [SerializeField] Transform _target;

    [Header("Speed Settings")]
    [SerializeField] float _moveSpeed = 200.0f;

    [Header("Tolerance Settings")]
    [Tooltip("Next Waypoint Tolerance specifies the amount of distance required between the moving object and it's current waypoint before moving to it's next target")]
    [Min(0.01f)][SerializeField] float _nextWaypointTolerance = 3f;

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
        //If the pathfinder has a target by default then navigate to it on Start()
        if(_target != null)
        {
            SetTargetTransform(_target);
        }
    }

    void UpdatePath()
    {
        //Safety check
        if (_target == null) return;

        //Checks that the seeker has finished calculating it's current path. Stops the risk of continually calculating a path
        if (_seeker.IsDone())
        {
            _seeker.StartPath(_rb.position, _target.position, OnPathComplete);
        }
    }

    void OnPathComplete(Path p)
    {
        //If the path did not have an error then set the path point
        if (!p.error)
        {
            _path = p;
            _currentWaypoint = 0;
        }
    }

    void FixedUpdate()
    {
        if(_path != null)
        {
            Move();
        }    
    }

    void Move()
    {
        //Have we finished the path
        _reachedEndOfPath = _currentWaypoint >= _path.vectorPath.Count;

        //Don't calculate movement forces if we do not need to move
        if (_reachedEndOfPath) return;

        //Calculate our direction vector and the force vector
        Vector2 direction = ((Vector2)_path.vectorPath[_currentWaypoint] - _rb.position).normalized;
        Vector2 force = direction * _moveSpeed * Time.fixedDeltaTime; //Move() is called in the FixedUpdate() hence the need for fixedDeltaTime

        //Adds the force to our rigidbody
        _rb.AddForce(force);

        //checks to see if we are within distance of our next waypoint
        float distance = Vector2.Distance(_rb.position, _path.vectorPath[_currentWaypoint]);
        if (distance < _nextWaypointTolerance)
        {
            _currentWaypoint++;
        }
    }

    /// <summary>
    /// Sets the target for this pathfinder
    /// </summary>
    /// <param name="t">New target transform</param>
    /// <param name="repeatDestination">If this is set to true then the path will be updated every updateIncrement (next paramater)</param>
    /// <param name="updateIncrement">The amount of time between calculating a updated path</param>
    public void SetTargetTransform(Transform t, bool repeatDestination = false, float updateIncrement = 1.0f)
    {
        //Sets the target
        _target = t;

            
        //Cancel the current invoke of the update path method
        //Regardless of if the repeatDestination is true or false this needs to happen to avoid the same method being invoked multiple times
        CancelInvoke("UpdatePath");

        if (repeatDestination)
        {
            //Reinvokes the update path method with the desired update increment
            InvokeRepeating("UpdatePath", 0f, updateIncrement);
        }
        
    }
}
