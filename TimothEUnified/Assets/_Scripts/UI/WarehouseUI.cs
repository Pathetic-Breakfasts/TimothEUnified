using GameFramework.Inventories;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TimothE.Gameplay.Interactables;
public class WarehouseUI : MonoBehaviour
{
    [SerializeField] ResourceCardUI _resourceCardPrefab;
    [SerializeField] Transform _desiredWarehouseParent;
    [SerializeField] Transform _totalWarehouseParent;


    List<ResourceCardUI> _resourceCards;

    private void Awake()
    {
        _resourceCards = new List<ResourceCardUI>();
    }

    //////////////////////////////////////////////////
    public void Redraw(Warehouse desiredWarehouse)
    {
        if (!desiredWarehouse) return;

        foreach(ResourceCardUI ui in _resourceCards)
        {
            Destroy(ui.gameObject);
        }
        _resourceCards.Clear();


        Dictionary<ResourceType, int> map = desiredWarehouse.ResourceMap;
        foreach(KeyValuePair<ResourceType, int> kvp in map)
        {
            ResourceCardUI ui = Instantiate(_resourceCardPrefab);
            if (ui != null)
            {
                ui.transform.SetParent(_desiredWarehouseParent, false);
                ui.Setup(kvp.Key, kvp.Value);
                _resourceCards.Add(ui);
            }
        }

        Dictionary<ResourceType, int> completeMap = FindObjectOfType<WarehouseManager>().GetResourceMap();
        foreach (KeyValuePair<ResourceType, int> kvp in completeMap)
        {
            ResourceCardUI ui = Instantiate(_resourceCardPrefab);
            if (ui != null)
            {
                ui.transform.SetParent(_totalWarehouseParent, false);
                ui.Setup(kvp.Key, kvp.Value);
                _resourceCards.Add(ui);
            }
        }
    }
}
