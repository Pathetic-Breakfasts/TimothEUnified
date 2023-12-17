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

    [System.Serializable]
    public enum ToolType
    {
        Hoe,
        Pickaxe,
        Axe
    }

    [System.Serializable]
    public enum ArmorType
    {
        Head,
        Chest,
        Arms, 
        Legs
    }


    /// <summary>
    /// A ScriptableObject that represents any item that can be put in an
    /// inventory.
    /// </summary>
    /// <remarks>
    /// In practice, you are likely to use a subclass such as `ActionItem` or
    /// `EquipableItem`.
    /// </remarks>
    [CreateAssetMenu(menuName = ("Inventory/Inventory Item"))]
    public class InventoryItem : ScriptableObject, ISerializationCallbackReceiver
    {
        // CONFIG DATA
        [Tooltip("Auto-generated UUID for saving/loading. Clear this field if you want to generate a new one.")]
        [SerializeField] public string itemID = null;
        [Tooltip("Item name to be displayed in UI.")]
        [SerializeField] public string displayName = "";
        [Tooltip("Item description to be displayed in UI.")]
        [SerializeField][TextArea] public string description = "";
        [Tooltip("The UI icon to represent this item in the inventory.")]
        [SerializeField] public Sprite icon = null;
        [Tooltip("If true, multiple items of this type can be stacked in the same inventory slot.")]
        [SerializeField] public bool isStackable = true;
        [Tooltip("How much this item is worth before before factoring local merchant rates. Set to -1 for Unsellable Items")]
        [Min(-1f)][SerializeField] public int itemValue = 0;
        [Tooltip("The type of item this is. Controls how the item behaves and what variables are modifiable.")]
        [SerializeField] public ItemType itemType;

        // STATE
        static Dictionary<string, InventoryItem> itemLookupCache;

        //Begining Of Crop Config
        [Header("Seed Settings")]
        [Tooltip("Sprites Used for Growing the Crop Throughout it's lifetime.")]
        public Sprite[] growthSpriteArray;
        [Tooltip("The amount of in-game days required for the seed to grow into a harvestable")]
        [Min(1.0f)]public int daysToGrow = 1;
        [Tooltip("The correct season for the crop to grow in")]
        public Seasons correctSeason;
        [Tooltip("The crop the player will be given upon harvesting")]
        public InventoryItem grownCropItem;
        //End of Crop Config


        //Begining of Weapon Config
        [Header("Weapon Settings")]
        [Tooltip("The amount of damage this weapon will do")]
        [Min(0.01f)]public float damage = 10.0f;
        [Tooltip("The attack range of this weapon (USed by AI)")]
        [Min(0.01f)]public float attackRange = 1.5f;
        [Tooltip("The amount of time between attacks")]
        [Min(0.01f)]public float attackSpeed = 0.2f;
        [Tooltip("The damage bonus when the player performs a heavy attack")]
        [Min(0.01f)]public float heavyAttackDamageBoost = 2.0f;
        [Tooltip("How fast the weapon will swing during a light attack")]
        [Min(0.01f)]public float lightAttackSwingRate = 15.0f;
        [Tooltip("How fast the weapon will swing during a heavy attack")]
        [Min(0.01f)]public float heavyAttackSwingRate = 7.5f;
        [Tooltip("How large (In Degrees) the swing of the weapon is. Performs half of this on each side of the player")]
        [Min(0.01f)]public float weaponSwingDistance = 90.0f;
        public Sprite weaponSprite;
        [Header("Ranged Weapon Settings")]
        [Tooltip("Is this a ranged weapon? I.e. a bow")]
        public bool isRanged = false;
        [Tooltip("The projectile launched by this weapon")]
        public ProjectileConfig projectileConfig;
        //End of Weapon Config


        //Beginning of Tool Config
        [Header("Tool Settings")]
        [Tooltip("The type of tool this is")]
        public ToolType toolType;
        [Tooltip("Used to determine how much each hit with the tool will damage the resource node")]
        [Min(0.01f)]public float toolPower = 50.0f;
        [Tooltip("How much energy using this item will take from the player")]
        [Min(0.01f)]public float energyConsumption = 3.0f;
        [Tooltip("The sprite used when the tool is being swung sideways")]
        public Sprite horizontalToolSprite;
        [Tooltip("The sprite used when the tool is being swung vertically")]
        public Sprite verticalToolSprite;
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
            GameObject pickupObject = Instantiate(Resources.Load("Pickup")) as GameObject;
            pickupObject.transform.position = position;

            Pickup pickup = pickupObject.GetComponent<Pickup>();
            if (pickup)
            {
                pickup.Setup(this, number);
            }
            else
            {
                Debug.LogError("No Pickup Component Found");
            }

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
