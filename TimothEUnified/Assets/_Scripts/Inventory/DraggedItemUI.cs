using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DraggedItemUI : MonoBehaviour
{
    [SerializeField] Image _icon;
    [SerializeField] TextMeshProUGUI _quantityText;

    // Start is called before the first frame update
    void Start()
    {
        _icon.enabled = false;
        _quantityText.text = string.Empty;
    }

    public void UpdatePosition(Vector3 position)
    {
        transform.position = position;
    }

    public void SetAttributes(Sprite sprite, int quantity)
    {
        _icon.enabled = sprite != null;
        _icon.sprite = sprite;

        _quantityText.enabled = quantity != 0;
        _quantityText.text = quantity.ToString();
    }
}
