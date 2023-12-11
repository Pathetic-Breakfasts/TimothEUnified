using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameDevTV.Inventories;

namespace GameDevTV.UI.Inventories
{
    /// <summary>
    /// To be placed on the root of the inventory UI. Handles spawning all the
    /// inventory slot prefabs.
    /// </summary>
    public class InventoryUI : MonoBehaviour
    {
        // CONFIG DATA
        [SerializeField] InventorySlotUI InventoryItemPrefab = null;

        // CACHE
        public Inventory DisplayedInventory { get => _inventoryToDisplay; set
            {
                //Unsubscribe the current inventory from the redraw
                if (_inventoryToDisplay)
                {
                    _inventoryToDisplay.inventoryUpdated -= Redraw;
                }

                //Sets our new inventory to display and ensures it isn't subscibed to this Redraw already
                _inventoryToDisplay = value;
                _inventoryToDisplay.inventoryUpdated -= Redraw;
                _inventoryToDisplay.inventoryUpdated += Redraw;
            }
        }
        Inventory _inventoryToDisplay;

        // LIFECYCLE METHODS

        private void Awake() 
        {
            //_inventoryToDisplay = Inventory.GetPlayerInventory();
            //_inventoryToDisplay.inventoryUpdated += Redraw;
        }

        private void Start()
        {
            Redraw();
        }

        // PRIVATE

        public void Redraw()
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            for (int i = 0; i < _inventoryToDisplay.GetSize(); i++)
            {
                var itemUI = Instantiate(InventoryItemPrefab, transform);
                itemUI.Setup(_inventoryToDisplay, i);
            }
        }
    }
}