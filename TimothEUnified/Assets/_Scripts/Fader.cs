using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fader : MonoBehaviour
{
    private CanvasGroup canvas;

    Coroutine currentActiveFade = null;

    [SerializeField] float fadeTime = 2.5f;
    [SerializeField] float fadeWaitTime = 1.0f;

    void Awake()
    {
        canvas = GetComponent<CanvasGroup>();
    }
    public void FadeOutImmediate()
    {
        canvas.alpha = 1f;
    }

    public IEnumerator FadeOut()
    {
        if (currentActiveFade != null)
        {
            StopCoroutine(currentActiveFade);
        }

        canvas.alpha = 1.0f;
        currentActiveFade = StartCoroutine(FadeRoutine(0f));

        yield return currentActiveFade;
    }


    public IEnumerator FadeIn()
    {
        if (currentActiveFade != null)
        {
            StopCoroutine(currentActiveFade);
        }

        canvas.alpha = 0.0f;
        currentActiveFade = StartCoroutine(FadeRoutine(1f));

        yield return currentActiveFade;
    }

    private IEnumerator FadeRoutine(float target)
    {
        while (!Mathf.Approximately(canvas.alpha, target))
        {
            canvas.alpha = Mathf.MoveTowards(canvas.alpha, target, Time.deltaTime / fadeTime);

            yield return new WaitForEndOfFrame();
        }
    }

    public IEnumerator FadeWait()
    {
        yield return new WaitForSeconds(fadeWaitTime);
    }
}
