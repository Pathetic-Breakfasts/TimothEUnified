using GameDevTV.Inventories;
using GameDevTV.Saving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Inventory))]
[RequireComponent(typeof(BoxCollider2D))]
public class Chest : MonoBehaviour, ISaveable
{
    public Inventory ChestInventory { get => _chestInventory; }
    Inventory _chestInventory;
    PlayerController _playerController;

    private void Awake()
    {
        _chestInventory = GetComponent<Inventory>();
        _playerController = FindObjectOfType<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(GameTagManager._playerTag))
        {
            _playerController.NearbyChest = this;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(GameTagManager._playerTag))
        {
            _playerController.NearbyChest = null;
        }
    }

    public object CaptureState()
    {
        return null;
        //throw new System.NotImplementedException();
    }

    public void RestoreState(object state)
    {
        //throw new System.NotImplementedException();
    }
}
