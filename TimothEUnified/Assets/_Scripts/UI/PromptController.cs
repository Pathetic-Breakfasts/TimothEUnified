using TMPro;
using UnityEngine;

public class PromptController : MonoBehaviour
{
    private CanvasGroup _canvasGroup;

    [SerializeField] private TextMeshProUGUI _promptText;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public void SetPromptVisibility(bool promptVisibility)
    {
        if(!_canvasGroup)
        {
            Debug.LogError(gameObject.name + " has no Canvas Group Component!");
            return;
        }

        _canvasGroup.alpha = promptVisibility ? 1.0f : 0.0f;
    }

    public void SetPromptText(string prompt)
    {
        if(_promptText)
        {
            _promptText.text = prompt;
        }
    }
}
