using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestInputLayer : IInputLayer
{
    PlayerController _playerController;

    public void Initialize()
    {
        _playerController = GameObject.FindObjectOfType<PlayerController>();
    }

    public void UpdateLayer()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            _playerController.SetChestUIVisibility(false);
        }
    }
}
