using System.Collections.Generic;
using TimothE.Utility;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuildModeUI : MonoBehaviour
{
    [SerializeField] Image _preview;

    BuildModeController _buildController;

    //////////////////////////////////////////////////
    public StructureConfig SelectedConfig {
        set 
        {
            _selectedConfig = value;
            if (_buildController)
            {
                _buildController.CurrentConfig = value;
            }
        } 
    }
    StructureConfig _selectedConfig = null;

    //////////////////////////////////////////////////
    private void Awake()
    {
        _buildController = FindObjectOfType<BuildModeController>();
    }

    //////////////////////////////////////////////////
    private void Update()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> raysastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raysastResults);
        bool bEnablePreview = true;

        for (int index = 0; index < raysastResults.Count; index++)
        {
            RaycastResult curRaysastResult = raysastResults[index];
            if (curRaysastResult.gameObject.layer == GameUtilities.UILayer)
            {
                bEnablePreview = false;
            }

        }

        _preview.gameObject.SetActive(bEnablePreview && _selectedConfig != null);

        if(_selectedConfig != null)
        {
            SetBoxPosition(_buildController.CurrentTilePosition, _selectedConfig.structureSize, _buildController.CanPlace);
        }
    }

    //////////////////////////////////////////////////
    public void SetBoxPosition(Vector3 position, Vector2 size, bool bCanPlace)
    {
        if (!_selectedConfig || !_preview)
        {
            return;
        }

        _preview.transform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, position);
        _preview.color = bCanPlace ? Color.white : Color.red;
        _preview.rectTransform.sizeDelta = new Vector2(size.x * 32.0f * 2.0f, size.y * 32.0f * 2.0f);
    }
}
