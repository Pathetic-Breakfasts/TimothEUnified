using GameFramework.Inventories;
using GameFramework.Saving;
using UnityEngine;

[RequireComponent(typeof(Inventory))]
[RequireComponent(typeof(BoxCollider2D))]
public class Chest : MonoBehaviour, ISaveable, IInteractable
{
    public Inventory ChestInventory { get => _chestInventory; }
    Inventory _chestInventory;
    PlayerController _playerController;

    //////////////////////////////////////////////////
    private void Awake()
    {
        _chestInventory = GetComponent<Inventory>();
        _playerController = FindObjectOfType<PlayerController>();
    }

    //////////////////////////////////////////////////
    public object CaptureState()
    {
        return null;
        //throw new System.NotImplementedException();
    }

    //////////////////////////////////////////////////
    public void RestoreState(object state)
    {
        //throw new System.NotImplementedException();
    }

    //////////////////////////////////////////////////
    public void OnUse(PlayerController controller)
    {
        controller.GameUIManager.ChestInventoryUI.DisplayedInventory = _chestInventory;
        _playerController.SetChestUIVisibility(true);
    }
}
