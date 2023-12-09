using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterSeconds : MonoBehaviour
{

    [Min(0f)][SerializeField] float _lifetime = 10.0f;
    float _timer;

    public void SetLifetime(float duration)
    {
        _lifetime = duration;
    }

    // Start is called before the first frame update
    void Start()
    {
        _timer = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        _timer += Time.deltaTime;

        if(_timer > _lifetime)
        {
            Destroy(gameObject);
        }
    }
}
