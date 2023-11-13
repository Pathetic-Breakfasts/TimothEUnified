using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private PromptController _promptController;

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
}
