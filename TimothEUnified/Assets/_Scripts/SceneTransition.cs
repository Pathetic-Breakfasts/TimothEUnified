using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    [Header("Other Scene's Attributes")]
    [Tooltip("The scene that moving through this transition will take the player to")]
    [SerializeField] string _levelToTransitionTo;
    [Tooltip("The transition point in the other scene that this transition will take the player to")]
    [SerializeField] int _indexToTransitionTo;

    [Header("This Scene's Attributes")]
    [Tooltip("The ID that this transition point will be referred to as")]
    [SerializeField] int _transitionId;

    public int TransitionID { get => _transitionId; }

    IEnumerator LoadScene()
    {
        DontDestroyOnLoad(gameObject);

        Fader fade = FindObjectOfType<Fader>();

        yield return StartCoroutine(fade.FadeIn());

        yield return SceneManager.LoadSceneAsync(_levelToTransitionTo);

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
        if(trans != null)
        {
            FindObjectOfType<PlayerInput>().transform.position = trans.transform.GetChild(0).position;
        }
        else
        {
            Debug.LogWarning("Scene transition could not find a transition with the ID of: " + _indexToTransitionTo);
        }

        yield return StartCoroutine(fade.FadeOut());

        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Player")) return;

        StartCoroutine(LoadScene());
    }

    private void OnDrawGizmos()
    {
        Transform child = transform.GetChild(0);

        if (!child) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(child.position, 0.5f);
    }

}
