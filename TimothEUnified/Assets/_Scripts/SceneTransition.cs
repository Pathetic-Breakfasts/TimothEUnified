using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    [SerializeField] string levelToTransitionTo;
    [SerializeField] int indexToTransitionTo;


    IEnumerator LoadScene()
    {
        DontDestroyOnLoad(gameObject);

        Fader fade = FindObjectOfType<Fader>();

        yield return StartCoroutine(fade.FadeIn());

        yield return SceneManager.LoadSceneAsync(levelToTransitionTo);


        yield return StartCoroutine(fade.FadeWait());


        fade = FindObjectOfType<Fader>();
        yield return StartCoroutine(fade.FadeOut());

        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Player")) return;

        StartCoroutine(LoadScene());

    }

}
