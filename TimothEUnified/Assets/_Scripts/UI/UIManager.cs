using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private PromptController _promptController;

    [SerializeField] private ImageFiller _energyBarFiller;
    [SerializeField] private ImageFiller _healthBarFiller;

    private void Start()
    {
        _promptController?.SetPromptVisibility(false);
    }

    public void SetInputPromptVisibility(bool visible)
    {
        _promptController?.SetPromptVisibility(visible);
    }

    public void SetInputPromptText(string promptText)
    {
        _promptController?.SetPromptText(promptText);
    }

    public void SetEnergyBarFillRatio(float ratio)
    {
        _energyBarFiller?.SetFillRatio(ratio);
    }

    public void SetHealthBarFillRatio(float ratio)
    {
        _healthBarFiller?.SetFillRatio(ratio);
    }
}
