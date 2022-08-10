using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InteractionPoint : MonoBehaviour
{

    public List<GameObject> ObjectsInTrigger { get => _objectsInTrigger; }

    List<GameObject> _objectsInTrigger;

    private void Awake()
    {
        _objectsInTrigger = new List<GameObject>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            _objectsInTrigger.Add(col.gameObject);
        }

    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            _objectsInTrigger.Remove(col.gameObject);
        }
    }
}
