using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class InteractableVolume : MonoBehaviour
{
    PlayerController _playerController;

    //////////////////////////////////////////////////
    private void Awake()
    {
        _playerController = FindObjectOfType<PlayerController>();
    }


    //////////////////////////////////////////////////
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag(GameTagManager._playerTag))
        {
            _playerController.AddInteractable(this);
        }
    }

    //////////////////////////////////////////////////
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(GameTagManager._playerTag))
        {
            _playerController.RemoveInteractable(this);
        }
    }
}
