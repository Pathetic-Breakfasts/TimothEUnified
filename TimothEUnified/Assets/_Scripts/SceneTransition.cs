using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    [SerializeField] string _levelToTransitionTo;
    [SerializeField] int _indexToTransitionTo;

    [SerializeField] int _transitionId;

    public int TransitionID { get => _transitionId; }

    IEnumerator LoadScene()
    {
        DontDestroyOnLoad(gameObject);

        Fader fade = FindObjectOfType<Fader>();

        yield return StartCoroutine(fade.FadeIn());

        yield return SceneManager.LoadSceneAsync(_levelToTransitionTo);

        fade = FindObjectOfType<Fader>();
        fade.FadeOutImmediate();

        yield return StartCoroutine(fade.FadeWait());

        SceneTransition trans = null;
        foreach (SceneTransition transition in FindObjectsOfType<SceneTransition>())
        {
            if (transition.TransitionID == _indexToTransitionTo)
            {
                trans = transition;
                break;
            }
        }

        FindObjectOfType<PlayerInput>().transform.position = trans.transform.GetChild(0).position;

        yield return StartCoroutine(fade.FadeOut());

        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Player")) return;

        StartCoroutine(LoadScene());

    }

}
