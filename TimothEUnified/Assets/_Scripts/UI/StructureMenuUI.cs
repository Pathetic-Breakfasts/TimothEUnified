using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureMenuUI : MonoBehaviour
{
    [SerializeField] Transform _structureCardParent;


    [SerializeField] StructureCardUI _structureCardUIPrefab;

    
    [SerializeField] List<StructureConfig> _structureConfigList;

    public void Redraw()
    {
        foreach (Transform child in _structureCardParent)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < _structureConfigList.Count; i++)
        {
            StructureCardUI structureCard = Instantiate(_structureCardUIPrefab, transform);
            structureCard.Initialize(_structureConfigList[i]);
        }
    }
}
