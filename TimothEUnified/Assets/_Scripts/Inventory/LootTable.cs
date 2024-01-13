using GameDevTV.Inventories;
using System.Collections.Generic;
using UnityEngine;


//////////////////////////////////////////////////
[System.Serializable]
struct LootTableRecord
{
    public InventoryItem item;
    [Range(0f,1f)]public float chanceOfSpawningItem;
}

//////////////////////////////////////////////////
[CreateAssetMenu(menuName =("Inventory/Loot Table"))]
public class LootTable : ScriptableObject
{
    [SerializeField] InventoryItem[] guranteedItems;
    [SerializeField] LootTableRecord[] additionalItems;

    //////////////////////////////////////////////////
    public void SpawnItems(Vector3 position, int numberOfGuranteedItems, int numberOfAdditionalItems)
    {
        List<InventoryItem> itemsToSpawn = new List<InventoryItem>();

        //Decide how many of the guranteed items we will be spawning 
        if(guranteedItems.Length > 0)
        {
            int numberSpawned = 0;

            while(numberSpawned <= numberOfGuranteedItems)
            {
                foreach(InventoryItem item in guranteedItems)
                {
                    itemsToSpawn.Add(item);
                    numberSpawned++;

                    //Breaks out of this for loop if we are now going to be spawning too many items
                    if(numberSpawned >= numberOfAdditionalItems)
                    {
                        break;
                    }
                }
            }
        }

        //Decide how many of the additional items we will be spawning
        if(additionalItems.Length > 0)
        {
            int numberSpawned = 0;

            while(numberSpawned < numberOfAdditionalItems)
            {
                foreach(LootTableRecord record in additionalItems)
                {
                    if(record.chanceOfSpawningItem == 1.0f)
                    {
                        itemsToSpawn.Add(record.item);
                        numberSpawned++;

                        if(numberSpawned >= numberOfAdditionalItems)
                        {
                            break;
                        }
                    }
                    else if(1.0f - Random.Range(0.0f, 1.0f) > record.chanceOfSpawningItem)
                    {
                        itemsToSpawn.Add(record.item);
                        numberSpawned++;

                        //Breaks out of this for loop if we are now going to be spawning too many items
                        if (numberSpawned >= numberOfAdditionalItems)
                        {
                            break;
                        }
                    }
                }
            }
        }

        //Spawn the items we have selected
        foreach(InventoryItem item in itemsToSpawn)
        {
            item.SpawnPickup(position, 1);
        }
    }
}
