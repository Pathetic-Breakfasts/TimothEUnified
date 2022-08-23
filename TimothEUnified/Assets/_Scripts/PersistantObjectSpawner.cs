using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistantObjectSpawner : MonoBehaviour
{
    [SerializeField] GameObject persistantObject = null;

    static bool hasSpawned = false;

    void Awake()
    {
        if (hasSpawned) return;

        hasSpawned = true;

        SpawnObjects();
    }

    private void SpawnObjects()
    {
        GameObject objectInstance = Instantiate(persistantObject);
        DontDestroyOnLoad(objectInstance);
    }
}
