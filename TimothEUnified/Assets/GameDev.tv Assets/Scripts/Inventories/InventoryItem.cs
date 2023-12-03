using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameDevTV.Inventories
{
    [System.Serializable]
    public enum ItemType
    {
        HOLDABLE,
        WEAPON,
        TOOL,
        SEED,
        ARMOR
    }
    public enum CropType
    {
        Carrot,
        Potatoes,
        Beetroots,
        Oat,
        Wheat,
        Tomatoes,
        Lettuce,
        Cabbage
    }

    public enum ToolType
    {
        Hoe,
        Pickaxe,
        Axe
    }


    /// <summary>
    /// A ScriptableObject that represents any item that can be put in an
    /// inventory.
    /// </summary>
    /// <remarks>
    /// In practice, you are likely to use a subclass such as `ActionItem` or
    /// `EquipableItem`.
    /// </remarks>
    /// [CreateAssetMenu(menuName = ("GameDevTV/GameDevTV.UI.InventorySystem/Inventory Item"))]
    public abstract class InventoryItem : ScriptableObject, ISerializationCallbackReceiver
    {
        // CONFIG DATA
        [Tooltip("Auto-generated UUID for saving/loading. Clear this field if you want to generate a new one.")]
        [SerializeField] public string itemID = null;
        [Tooltip("Item name to be displayed in UI.")]
        [SerializeField] public string displayName = null;
        [Tooltip("Item description to be displayed in UI.")]
        [SerializeField][TextArea] public string description = null;
        [Tooltip("The UI icon to represent this item in the inventory.")]
        [SerializeField] public Sprite icon = null;
        [Tooltip("The prefab that should be spawned when this item is dropped.")]
        [SerializeField] public Pickup pickup = null;
        [Tooltip("If true, multiple items of this type can be stacked in the same inventory slot.")]
        [SerializeField] public bool isStackable = false;

        [SerializeField] public ItemType itemType;

        //[SerializeField] public ToolConfig toolConfig;
        //[SerializeField] public WeaponConfig weaponConfig;
        //[SerializeField] public CropConfig cropConfig;

        // STATE
        static Dictionary<string, InventoryItem> itemLookupCache;

        //Begining Of Crop Config
        public CropType type;

        public Sprite[] growthSpriteArray;

        public int daysToGrow;
        [Range(0.0f, 1.0f)] public float incorrectSeasonPentalty = 0.2f;

        public Seasons idealSeasonToGrow;

        public InventoryItem _grownCropItem;
        //End of Crop Config


        //Begining of Weapon Config
        [Header("General Settings")]
        public float _damage = 10.0f;
        public float _attackRange = 1.5f;
        public float _attackSpeed = 1.5f;
        public float _heavyAttackDamageBoost = 2.0f;
        public float _lightAttackSwingRate = 15.0f;
        public float _heavyAttackSwingRate = 7.5f;
        public float _weaponSwingDistance = 90.0f;

        [Header("Graphics Settings")]
        public Sprite _sprite;

        [Header("Ranged Weapon Settings")]
        public bool _isRanged = false;
        public Projectile _projectilePrefab;
        //End of Weapon Config


        //Beginning of Tool Config
        public ToolType _type;
        public float _toolPower = 50.0f;

        public float _energyConsumption = 3.0f;

        public Sprite _horizontalSprite;
        public Sprite _verticalSprite;
        //End of Tool Config





        // PUBLIC

        /// <summary>
        /// Get the inventory item instance from its UUID.
        /// </summary>
        /// <param name="itemID">
        /// String UUID that persists between game instances.
        /// </param>
        /// <returns>
        /// Inventory item instance corresponding to the ID.
        /// </returns>
        public static InventoryItem GetFromID(string itemID)
        {
            if (itemLookupCache == null)
            {
                itemLookupCache = new Dictionary<string, InventoryItem>();
                var itemList = Resources.LoadAll<InventoryItem>("");
                foreach (var item in itemList)
                {
                    if (itemLookupCache.ContainsKey(item.itemID))
                    {
                        Debug.LogError(string.Format("Looks like there's a duplicate GameDevTV.UI.InventorySystem ID for objects: {0} and {1}", itemLookupCache[item.itemID], item));
                        continue;
                    }

                    itemLookupCache[item.itemID] = item;
                }
            }

            if (itemID == null || !itemLookupCache.ContainsKey(itemID)) return null;
            return itemLookupCache[itemID];
        }
        
        /// <summary>
        /// Spawn the pickup gameobject into the world.
        /// </summary>
        /// <param name="position">Where to spawn the pickup.</param>
        /// <param name="number">How many instances of the item does the pickup represent.</param>
        /// <returns>Reference to the pickup object spawned.</returns>
        public Pickup SpawnPickup(Vector3 position, int number)
        {
            var pickup = Instantiate(this.pickup);
            pickup.transform.position = position;
            pickup.Setup(this, number);
            return pickup;
        }

        // PRIVATE
        
        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            // Generate and save a new UUID if this is blank.
            if (string.IsNullOrWhiteSpace(itemID))
            {
                itemID = System.Guid.NewGuid().ToString();
            }
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            // Require by the ISerializationCallbackReceiver but we don't need
            // to do anything with it.
        }
    }
}
