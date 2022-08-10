using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InteractionPoint : MonoBehaviour
{

    public List<GameObject> ObjectsInTrigger { get => _objectsInTrigger; }

    List<GameObject> _objectsInTrigger;

    void OnTriggerEnter2D(Collider2D col)
    {
        _objectsInTrigger.Add(col.gameObject);
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        _objectsInTrigger.Remove(col.gameObject);
    }
}
