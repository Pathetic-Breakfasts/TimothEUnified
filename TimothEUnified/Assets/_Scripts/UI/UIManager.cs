using GameDevTV.UI.Inventories;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    [SerializeField] GameObject _inventoryScreen;

    [SerializeField] TextMeshProUGUI _coinText;

    private void Start()
    {
        _promptController?.gameObject.SetActive(true);
        _promptController?.SetPromptVisibility(false);
    }

    public void SetChestUIVisibility(bool val)
    {
        _chestInventoryUI.Redraw();
        _chestInventoryScreen.SetActive(val);
    }

    public void SetInventoryUIVisibilty(bool val)
    {
        _playerInventoryUI.Redraw();
        _inventoryScreen.SetActive(val);
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

    public void SetCoinTextAmount(int amount)
    {
        _coinText.text = amount.ToString();
    }
}
