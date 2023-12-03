using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    PlayerController _playerController;

    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            _playerController.UseInteractable();
        }

        _playerController.IsHeavyAttack = Input.GetKeyDown(KeyCode.LeftShift);

        if (Input.GetMouseButtonDown(0))
        {
            _playerController.UseEquipped();
        }

        if(Input.mouseScrollDelta.y > 0)
        {
            FindObjectOfType<HotbarManager>().ModifyIndex(1);
        }
        else if(Input.mouseScrollDelta.y < 0)
        {
            FindObjectOfType<HotbarManager>().ModifyIndex(-1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            FindObjectOfType<HotbarManager>().SetIndex(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            FindObjectOfType<HotbarManager>().SetIndex(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            FindObjectOfType<HotbarManager>().SetIndex(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            FindObjectOfType<HotbarManager>().SetIndex(3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            FindObjectOfType<HotbarManager>().SetIndex(4);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            FindObjectOfType<HotbarManager>().SetIndex(5);
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            FindObjectOfType<HotbarManager>().SetIndex(6);
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            FindObjectOfType<HotbarManager>().SetIndex(7);
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            FindObjectOfType<HotbarManager>().SetIndex(8);
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            FindObjectOfType<HotbarManager>().SetIndex(9);
        }

        Vector2 movement;
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        _playerController.SetMovement(movement);
    }
}
