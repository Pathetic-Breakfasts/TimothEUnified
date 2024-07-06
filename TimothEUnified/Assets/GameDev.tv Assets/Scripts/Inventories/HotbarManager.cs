using GameFramework.Inventories;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace GameFramework.UI.Inventories
{
    public class HotbarManager : MonoBehaviour
    {
        HotbarUI[] _hotbarSlots;
        PlayerController _playerController;

        int _currentSlot = 0;


        private void Awake()
        {
            List<HotbarUI> hotbarSlots = FindObjectsOfType<HotbarUI>().ToList();
            hotbarSlots.Reverse();
            _hotbarSlots = hotbarSlots.ToArray();

            _playerController = FindObjectOfType<PlayerController>();
            Inventory playerInventory = Inventory.GetPlayerInventory();
            playerInventory.inventoryUpdated += UpdateHotbar;
        }

        public void UpdateHotbar()
        {
            for (int i = 0; i < _hotbarSlots.Length; i++)
            {
                _hotbarSlots[i].Setup();

                if (i == _currentSlot)
                {
                    _hotbarSlots[i].SetSelected();
                }
                else
                {
                    _hotbarSlots[i].SetUnselected();
                }
            }

            UpdateHeld();
        }

        public void ModifyIndex(int num)
        {
            _hotbarSlots[_currentSlot].SetUnselected();
            _currentSlot = (_currentSlot + num) % _hotbarSlots.Length;
            if (_currentSlot < 0) _currentSlot = _hotbarSlots.Length - 1;
            _hotbarSlots[_currentSlot].SetSelected();
            UpdateHeld();
        }

        public void SetIndex(int index)
        {
            if (index < 0 || index >= _hotbarSlots.Length)
            {
                Debug.LogError("HotbarManager::SetIndex Index: " + index + " is out of bounds");
                return;
            }

            _hotbarSlots[_currentSlot].SetUnselected();
            _currentSlot = index;
            _hotbarSlots[_currentSlot].SetSelected();
            UpdateHeld();
        }

        private void UpdateHeld()
        {
            _playerController.EquipItem(_hotbarSlots[_currentSlot].GetItem());
        }

    }
}
