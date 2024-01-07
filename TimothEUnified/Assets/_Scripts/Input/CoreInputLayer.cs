using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreInputLayer : IInputLayer
{
    PlayerController _playerController;
    HotbarManager _hotbarManager;

    //////////////////////////////////////////////////
    public void Initialize()
    {
        GameObject player = GameObject.FindGameObjectWithTag(GameTagManager._playerTag);
        if(player)
        {
            _playerController = player.GetComponent<PlayerController>();
        }
        _hotbarManager = GameObject.FindObjectOfType<HotbarManager>();
    }

    //////////////////////////////////////////////////
    public void UpdateLayer()
    {
        if(_playerController == null || _hotbarManager == null) 
        {
            Debug.LogError("CoreInputLayer has not been fully initialized! PlayerController:" + _playerController + " HotbarManager: "+ _hotbarManager);
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            _playerController.SetInventoryUIVisibility(true);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            _playerController.UseInteractable();
        }

        _playerController.IsHeavyAttack = Input.GetKeyDown(KeyCode.LeftShift);

        if (Input.GetMouseButtonDown(0))
        {
            _playerController.InteractPressed();
        }
        if (Input.GetMouseButtonDown(1))
        {
            _playerController.InspectPressed();
        }

        if (Input.mouseScrollDelta.y > 0)
        {
            _hotbarManager.ModifyIndex(1);
        }
        else if (Input.mouseScrollDelta.y < 0)
        {
            _hotbarManager.ModifyIndex(-1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _hotbarManager.SetIndex(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _hotbarManager.SetIndex(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            _hotbarManager.SetIndex(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            _hotbarManager.SetIndex(3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            _hotbarManager.SetIndex(4);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            _hotbarManager.SetIndex(5);
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            _hotbarManager.SetIndex(6);
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            _hotbarManager.SetIndex(7);
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            _hotbarManager.SetIndex(8);
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            _hotbarManager.SetIndex(9);
        }

        Vector2 movement;
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        _playerController.SetMovement(movement);
    }
}
