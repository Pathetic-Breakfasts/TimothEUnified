using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildModeUI : MonoBehaviour
{
    [SerializeField] Image _preview;


    //////////////////////////////////////////////////
    public void SetBoxPosition(Vector3 position, Vector2 size, bool bCanPlace)
    {
        
        _preview.transform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, position);
        //_preview.transform.localScale = size;
        _preview.color = bCanPlace ? Color.white : Color.red;
        _preview.rectTransform.sizeDelta = new Vector2(size.x * 32.0f * 2.0f, size.y * 32.0f * 2.0f);
    }
}
