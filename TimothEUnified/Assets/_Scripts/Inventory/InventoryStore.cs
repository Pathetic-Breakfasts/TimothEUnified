using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public struct InventoryRecord
{
    public ItemConfig config;
    public int quantity;
}

public class InventoryStore : MonoBehaviour
{
    [SerializeField] int _numOfSlots = 40;

    InventoryRecord[] _inventory;

    [SerializeField] UnityEvent _onInventoryUpdate;

    private void Awake()
    {
        _inventory = new InventoryRecord[_numOfSlots];

        for(int i  = 0; i < _numOfSlots; ++i)
        {
            _inventory[i] = new InventoryRecord();
            _inventory[i].quantity = 0;
            _inventory[i].config = null;
        }
    }

    public void AddItem(ItemConfig config, int quantity = 1)
    {
        int index = FindItemIndex(config);

        //We already have this item
        if(index != -1)
        {
            //Is Item Stackable
            if (config.isStackable)
            {
                //Will stacking these items exceed the max stack limit for this item
                if (_inventory[index].quantity + quantity > config.maxInStack)
                {
                    //Calculate how many items will be left after the stack
                    int remainder = (_inventory[index].quantity + quantity) - config.maxInStack;

                    //Add the correct amount to the current stack
                    _inventory[index].quantity += quantity - remainder;

                    int nextIndex = FindItemIndex(config, index + 1);
                    if(nextIndex != -1)
                    {
                        if (_inventory[nextIndex].quantity + quantity > config.maxInStack)
                        {
                            remainder = (_inventory[nextIndex].quantity - quantity) - config.maxInStack;
                            _inventory[nextIndex].quantity += quantity - remainder;

                            quantity = remainder;
                        }
                        else
                        {
                            quantity = remainder;
                            _inventory[nextIndex].quantity += quantity;
                        }
                        _onInventoryUpdate.Invoke();
                        //return;
                    }
                }
                //Stacking these items will not exceed the limit
                else
                {
                    _inventory[index].quantity += quantity;
                    _onInventoryUpdate.Invoke();
                    return;
                }
            }
        }

        //We do not have this item and need to find a empty slot
        index = FindFirstEmptyIndex();
        if(index != -1)
        {
            _inventory[index].config = config;

            if (config.isStackable)
            {
                //Will stacking these items exceed the max stack limit for this item
                if (_inventory[index].quantity + quantity > config.maxInStack)
                {
                    //Calculate how many items will be left after the stack
                    int remainder = (_inventory[index].quantity + quantity) - config.maxInStack;

                    //Add the correct amount to the current stack
                    _inventory[index].quantity += quantity - remainder;

                    //Find our next empty slot
                    int newStack = FindFirstEmptyIndex(index + 1);
                    if (newStack != -1)
                    {
                        //Put in the remainder quantity into that new slot
                        _inventory[index].quantity = remainder;
                        _inventory[index].config = config;
                    }
                }
                //Stacking these items will not exceed the limit
                else
                {
                    _inventory[index].quantity += quantity;
                }
                _onInventoryUpdate.Invoke();
            }
            else
            {
                _inventory[index].quantity = 1;
                
                if(quantity - 1 > 0)
                {
                    AddItem(config, quantity - 1);
                }
            }

            _onInventoryUpdate.Invoke();
            return;
        }

        //Full inventory
        //TODO: Inventory full UI Prompt
        return;
    }


    public int FindFirstEmptyIndex(int startingPoint = 0)
    {
        for (int i = startingPoint; i < _inventory.Length; ++i)
        {
            if (!_inventory[i].config)
            {
                return i;
            }
        }

        return -1;
    }

    public void RemoveItem(ItemConfig config, int quantity = 1) 
    {
        int index = FindItemIndex(config);
        if(index != -1)
        {
            if(HasItem(config, quantity))
            {
                _inventory[index].quantity -= quantity;

                if (_inventory[index].quantity <= 0)
                {
                    _inventory[index].config = null;
                    _onInventoryUpdate.Invoke();
                }
            }
        }
    }

    public InventoryRecord[] GetInventoryRecords()
    {
        return _inventory;
    }

    public bool HasItem(ItemConfig config, int quanity = 1) 
    {
        int index = FindItemIndex(config);
        if(index != -1)
        {
            return _inventory[index].quantity >= quanity;
        }

        return false;
    }

    public int FindItemIndex(ItemConfig config, int startingPoint = 0)
    {
        for(int i = startingPoint; i < _inventory.Length; ++i)
        {
            if (_inventory[i].config && _inventory[i].config == config)
            {
                return i;
            }
        }

        return -1;
    }

}
