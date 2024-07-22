using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildModeInputLayer : IInputLayer
{
    PlayerController _playerController;

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
    }
}
