using System;
using System.Collections.Generic;
using UnityEngine;
using GameDevTV.Saving;

namespace GameDevTV.Inventories
{
    /// <summary>
    /// Provides a store for the items equipped to a player. Items are stored by
    /// their equip locations.
    /// 
    /// This component should be placed on the GameObject tagged "Player".
    /// </summary>
    public class Equipment : MonoBehaviour, ISaveable
    {
        // STATE
        Dictionary<ArmorType, InventoryItem> equippedItems = new Dictionary<ArmorType, InventoryItem>();

        // PUBLIC

        /// <summary>
        /// Broadcasts when the items in the slots are added/removed.
        /// </summary>
        public event Action equipmentUpdated;

        /// <summary>
        /// Return the item in the given equip location.
        /// </summary>
        public InventoryItem GetItemInSlot(ArmorType equipLocation)
        {
            if (!equippedItems.ContainsKey(equipLocation))
            {
                return null;
            }

            return equippedItems[equipLocation];
        }

        /// <summary>
        /// Add an item to the given equip location. Do not attempt to equip to
        /// an incompatible slot.
        /// </summary>
        public void AddItem(ArmorType slot, InventoryItem item)
        {
            Debug.Assert(item.armorType == slot);

            equippedItems[slot] = item;

            if (equipmentUpdated != null)
            {
                equipmentUpdated();
            }
        }

        /// <summary>
        /// Remove the item for the given slot.
        /// </summary>
        public void RemoveItem(ArmorType slot)
        {
            equippedItems.Remove(slot);
            if (equipmentUpdated != null)
            {
                equipmentUpdated();
            }
        }

        /// <summary>
        /// Enumerate through all the slots that currently contain items.
        /// </summary>
        public IEnumerable<ArmorType> GetAllPopulatedSlots()
        {
            return equippedItems.Keys;
        }

        /// <summary>
        /// Enumerate through all the slots that currently contain items.
        /// </summary>
        public IEnumerable<InventoryItem> GetAllWornItems()
        {
            return equippedItems.Values;
        }

        // PRIVATE

        object ISaveable.CaptureState()
        {
            var equippedItemsForSerialization = new Dictionary<ArmorType, string>();
            foreach (var pair in equippedItems)
            {
                equippedItemsForSerialization[pair.Key] = pair.Value.itemID;
            }
            return equippedItemsForSerialization;
        }

        void ISaveable.RestoreState(object state)
        {
            equippedItems = new Dictionary<ArmorType, InventoryItem>();

            var equippedItemsForSerialization = (Dictionary<ArmorType, string>)state;

            foreach (var pair in equippedItemsForSerialization)
            {
                var item = InventoryItem.GetFromID(pair.Value);
                if (item != null)
                {
                    equippedItems[pair.Key] = item;
                }
            }
        }
    }
}