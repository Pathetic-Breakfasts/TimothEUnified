using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform _followTarget;
    [SerializeField] Vector2 _followOffset = Vector2.zero;
    [SerializeField] float _zoom = 5.0f;

    Camera _camera;


    private void Awake()
    {
        _camera = GetComponent<Camera>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_camera == null) return;
        if (_followTarget == null) return;


        Vector3 followPos = _followTarget.position;

        gameObject.transform.position = new Vector3(followPos.x + _followOffset.x, followPos.y + _followOffset.y, -5.0f);

        _camera.orthographicSize = _zoom;
    }
}
