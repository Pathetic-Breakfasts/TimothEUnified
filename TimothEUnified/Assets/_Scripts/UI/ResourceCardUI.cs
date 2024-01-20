using GameDevTV.Inventories;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourceCardUI : MonoBehaviour
{
    [SerializeField] Image _resourceIcon;
    [SerializeField] TextMeshProUGUI _resourceName;
    [SerializeField] TextMeshProUGUI _resourceQuantity;

    public void Setup(ResourceType type, int number)
    {
        _resourceName.text = type.ToString();
        _resourceQuantity.text = number.ToString();

        ResourceIconLookup lookup = FindObjectOfType<ResourceIconLookup>();
        if (lookup != null)
        {
            _resourceIcon.sprite = lookup.GetResourceSprite(type);
        }
    }
}
