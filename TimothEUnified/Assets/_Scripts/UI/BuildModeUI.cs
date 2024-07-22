using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildModeUI : MonoBehaviour
{
    [SerializeField] Image _preview;

    void Update()
    {
        
    }

    public void SetBoxPosition(Vector3 position, Vector2 size, bool bCanPlace)
    {
        
        _preview.transform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, position);
        _preview.transform.localScale = size;
        _preview.color = bCanPlace ? Color.white : Color.red;
    }
}
