using GameDevTV.UI.Inventories;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private PromptController _promptController;

    [SerializeField] private ImageFiller _energyBarFiller;
    [SerializeField] private ImageFiller _healthBarFiller;

    public InventoryUI PlayerInventoryUI { get => _playerInventoryUI; }
    [SerializeField] private InventoryUI _playerInventoryUI;

    public InventoryUI PlayerChestInventoryUI { get => _playerChestInventoryUI; }
    [SerializeField] private InventoryUI _playerChestInventoryUI;

    public InventoryUI ChestInventoryUI { get => _chestInventoryUI; }
    [SerializeField] private InventoryUI _chestInventoryUI;


    [SerializeField] GameObject _chestInventoryScreen;

    private void Start()
    {
        _promptController?.gameObject.SetActive(true);
        _promptController?.SetPromptVisibility(false);
    }

    public void ToggleChestUI()
    {
        _chestInventoryUI.Redraw();
        _chestInventoryScreen.SetActive(!_chestInventoryScreen.activeSelf);
    }

    public void SetInputPromptVisibility(bool visible)
    {
        _promptController?.SetPromptVisibility(visible);
    }

    public void SetInputPromptText(string promptText)
    {
        _promptController?.SetPromptText(promptText);
    }

    public void SetEnergyBarFillRatio(float ratio)
    {
        _energyBarFiller?.SetFillRatio(ratio);
    }

    public void SetHealthBarFillRatio(float ratio)
    {
        _healthBarFiller?.SetFillRatio(ratio);
    }
}
