using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI _quantityText;
    [SerializeField] Image _itemIcon;

    private void Start()
    {
        _itemIcon.enabled = false;
        _quantityText.enabled = false;
    }

    public void SetItem(ItemConfig config, int quantity)
    {
        if(config)
        {
            _quantityText.enabled = quantity > 1;
            _itemIcon.enabled = true;
            _itemIcon.sprite = config.icon;
            _quantityText.text = quantity.ToString();
        }
        else
        {
            _quantityText.enabled = false;
            _itemIcon.enabled = false;
            _itemIcon.sprite = null;
        }
    }
}
