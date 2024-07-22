using UnityEngine;
using UnityEngine.UI;

public class BuildModeUI : MonoBehaviour
{
    [SerializeField] Image _preview;

    public StructureConfig SelectedConfig {
        set 
        {
            _selectedConfig = value; 
            FindObjectOfType<BuildModeController>().CurrentConfig = value;
            _preview.gameObject.SetActive(_selectedConfig != null);
        } 
    }
    StructureConfig _selectedConfig = null;

    //////////////////////////////////////////////////
    public void SetBoxPosition(Vector3 position, Vector2 size, bool bCanPlace)
    {
        if (!_selectedConfig)
        {
            return;
        }

        _preview.transform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, position);
        _preview.color = bCanPlace ? Color.white : Color.red;
        _preview.rectTransform.sizeDelta = new Vector2(size.x * 32.0f * 2.0f, size.y * 32.0f * 2.0f);
    }
}
