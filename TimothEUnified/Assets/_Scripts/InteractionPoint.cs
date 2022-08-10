using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InteractionPoint : MonoBehaviour
{

    public List<GameObject> ObjectsInTrigger { get => _objectsInTrigger; }

    List<GameObject> _objectsInTrigger;
    [SerializeField] string[] _acceptedTags;

    private void Awake()
    {
        _objectsInTrigger = new List<GameObject>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        foreach (string tag in _acceptedTags)
        {
            if (col.CompareTag(tag))
            {
                _objectsInTrigger.Add(col.gameObject);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        foreach (string tag in _acceptedTags)
        {
            if (col.CompareTag(tag))
            {
                _objectsInTrigger.Remove(col.gameObject);
            }
        }
    }
}
