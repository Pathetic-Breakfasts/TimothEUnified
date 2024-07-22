using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StructureCardUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _title;
    [SerializeField] Image _icon;

    StructureConfig _config;

    //////////////////////////////////////////////////
    public void Initialize(StructureConfig config)
    {
        _config = config;
        _title.text = config.name;
        _icon.sprite = config.icon;
    }

    //////////////////////////////////////////////////
    public void OnClicked()
    {
        BuildModeUI buildModeUI = GetComponentInParent<BuildModeUI>();
        if (buildModeUI)
        {
            buildModeUI.SelectedConfig = _config;
        }
    }
}
