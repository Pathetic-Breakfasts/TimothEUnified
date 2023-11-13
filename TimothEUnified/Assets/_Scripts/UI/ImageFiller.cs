using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageFiller : MonoBehaviour
{
    [SerializeField] Image _fillImage;

    public void SetFillRatio(float ratio)
    {
        if(!_fillImage)
        {
            Debug.LogError(gameObject.name + " Has no Fill Image!");
        }

        _fillImage.fillAmount = ratio;
    }
}
