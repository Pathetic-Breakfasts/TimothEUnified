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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _playerController.IsInCombatMode = !_playerController.IsInCombatMode;
        }

        if (Input.GetMouseButtonDown(0))
        {
            _playerController.UseEquipped();
        }

        Vector2 movement;
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        _playerController.SetMovement(movement);
    }
}
