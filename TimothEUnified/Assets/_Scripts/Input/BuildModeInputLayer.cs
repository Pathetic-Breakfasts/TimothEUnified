using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildModeInputLayer : IInputLayer
{
    PlayerController _playerController;
    BuildModeUI _buildModeUI;
    BuildModeController _buildModeController;
    Mover _buildTargetMover;

    public Transform BuildModeFollowTarget { set
        {
            _buildModeFollowTarget = value;
            if (_buildModeFollowTarget)
                _buildTargetMover = _buildModeFollowTarget.GetComponent<Mover>();
            else _buildTargetMover = null;
        } 
    }
    Transform _buildModeFollowTarget;


    //////////////////////////////////////////////////
    public void Initialize()
    {
        _playerController = GameObject.FindObjectOfType<PlayerController>();
        _buildModeUI = GameObject.FindObjectOfType<BuildModeUI>();
        _buildModeController = GameObject.FindObjectOfType<BuildModeController>();
    }

    //////////////////////////////////////////////////
    public void UpdateLayer()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            _playerController.SetBuildModeActive(false);
        }

        if (Input.GetMouseButtonDown(0))
        {
            _buildModeController.PlaceStructure();
        }
        else if (Input.GetMouseButtonDown(1))
        {
            _buildModeUI.SelectedConfig = null;
        }

        if (_buildTargetMover)
        {
            float horizontalMovement = Input.GetAxisRaw("Horizontal");
            float verticalMovement = Input.GetAxisRaw("Vertical");
            _buildTargetMover.Move(new Vector2(horizontalMovement, verticalMovement));
        }
    }
}
