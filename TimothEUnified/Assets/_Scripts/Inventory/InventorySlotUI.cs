using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{

    [SerializeField] TextMeshProUGUI _quantityText;
    [SerializeField] Image _itemIcon;

    InventoryUI _inventoryUI;

    private void Start()
    {
        _itemIcon.enabled = false;
        _quantityText.enabled = false;
        _inventoryUI = GetComponentInParent<InventoryUI>();
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

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(_itemIcon.enabled == false)
        {
            return;
        }

        _inventoryUI.SetDragged(this);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _inventoryUI.SetDragged(null);
    }

    public void OnDrag(PointerEventData eventData)
    {
        _inventoryUI.UpdateDragged(eventData);
    }
}
