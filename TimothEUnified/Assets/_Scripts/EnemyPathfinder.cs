using Pathfinding;
using UnityEngine;

//Adds Rigidbody2D and Seeker components to this object by default if they do not exist
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Seeker))]
public class EnemyPathfinder : MonoBehaviour
{
    [Header("Destination Settings")]
    [SerializeField] Transform _target;

    [Tooltip("Currently stopping distance should be no lower than 1.0. This is due to Rigidbodies can push each other around. Will require extreme fuckery to fix.")]
    [Min(1.01f)][SerializeField] float _stoppingDistance = 0.5f;


    [Header("Speed Settings")]
    [SerializeField] float _moveSpeed = 200.0f;

    [Header("Tolerance Settings")]
    [Tooltip("Next Waypoint Tolerance specifies the amount of distance required between the moving object and it's current waypoint before moving to it's next target")]
    [Min(0.01f)][SerializeField] float _nextWaypointTolerance = 3f;

    Path _path;
    Seeker _seeker;
    int _currentWaypoint;
    bool _reachedEndOfPath = false;

    public Vector2 DirectionVector { get => _directionVector; }

    Vector2 _directionVector;
    
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

        Debug.Log("Updating path");

        //Checks that the seeker has finished calculating it's current path. Stops the risk of continually calculating a path
        if (_seeker.IsDone())
        {
            Debug.Log("Finding new path");
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
        else
        {
            Debug.Log("Pathing on agent: " + transform.name + " failed. Reason: " + p.errorLog);
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

        float remainingDistance = Vector2.Distance(_rb.position, _path.vectorPath[_path.vectorPath.Count - 1]);
        if(remainingDistance < _stoppingDistance)
        {
            _reachedEndOfPath = true;
            _rb.velocity = Vector2.zero;
            _rb.angularVelocity = 0.0f;
        }

        //Don't calculate movement forces if we do not need to move
        if (_reachedEndOfPath) return;


        //Calculate our direction vector
        _directionVector = ((Vector2)_path.vectorPath[_currentWaypoint] - _rb.position).normalized;


        //Calculates our new position
        Vector2 newPos = transform.position;
        newPos += _directionVector * _moveSpeed * Time.fixedDeltaTime;
        
        //Sets our new position
        transform.position = newPos;


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

        if (repeatDestination)
        {
            //Reinvokes the update path method with the desired update increment
            InvokeRepeating("UpdatePath", 0f, updateIncrement);
        }
        else
        {
            UpdatePath();
        }
        
    }
}
