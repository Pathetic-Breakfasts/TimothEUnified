using UnityEngine;

namespace GameDevTV.Inventories
{
    /// <summary>
    /// To be placed at the root of a Pickup prefab. Contains the data about the
    /// pickup such as the type of item and the number.
    /// </summary>
    public class Pickup : MonoBehaviour
    {
        // STATE
        InventoryItem item;
        int number = 1;

        // CACHED REFERENCE
        Inventory _inventory;

        SpriteRenderer _spriteRenderer;


        // LIFECYCLE METHODS

        private void Awake()
        {
            _inventory = Inventory.GetPlayerInventory();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        // PUBLIC

        /// <summary>
        /// Set the vital data after creating the prefab.
        /// </summary>
        /// <param name="item">The type of item this prefab represents.</param>
        /// <param name="number">The number of items represented.</param>
        public void Setup(InventoryItem item, int number)
        {
            this.item = item;
            if (!item.isStackable)
            {
                number = 1;
            }
            this.number = number;

            if(_spriteRenderer)
            {
                _spriteRenderer.sprite = item.icon;
            }
        }

        public InventoryItem GetItem()
        {
            return item;
        }

        public int GetNumber()
        {
            return number;
        }

        public void PickupItem()
        {
            bool foundSlot = _inventory.AddToFirstEmptySlot(item, number);
            if (foundSlot)
            {
                Destroy(gameObject);
            }
        }

        public bool CanBePickedUp()
        {
            return _inventory.HasSpaceFor(item);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag(GameTagManager._playerTag))
            {
                if (CanBePickedUp())
                {
                    PickupItem();
                }
            }
        }
    }
}