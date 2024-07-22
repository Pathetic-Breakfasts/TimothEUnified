using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildModeInputLayer : IInputLayer
{
    PlayerController _playerController;

    public Transform BuildModeFollowTarget { set
        {
            _buildModeFollowTarget = value;
            if (_buildModeFollowTarget)
                _buildTargetMover = _buildModeFollowTarget.GetComponent<Mover>();
            else _buildTargetMover = null;
        } 
    }
    Transform _buildModeFollowTarget;

    Mover _buildTargetMover;

    //////////////////////////////////////////////////
    public void Initialize()
    {
        _playerController = GameObject.FindObjectOfType<PlayerController>();
    }

    //////////////////////////////////////////////////
    public void UpdateLayer()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            _playerController.SetBuildModeActive(false);
        }


        if (_buildTargetMover)
        {
            float horizontalMovement = Input.GetAxisRaw("Horizontal");
            float verticalMovement = Input.GetAxisRaw("Vertical");
            _buildTargetMover.Move(new Vector2(horizontalMovement, verticalMovement));
        }
    }
}
