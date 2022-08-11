using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ResourceNodeType
{
    Wood,
    Stone,
    Coal,
    Iron,
    Copper
}

public class ResourceNode : MonoBehaviour
{
    [SerializeField] float _nodeMaxHealth = 100.0f;
    [SerializeField] ResourceNodeType _type;

    [SerializeField] int _amount = 1;
 
    float _currentHealth;

    private void Start()
    {
        _currentHealth = _nodeMaxHealth;
    }

    public void TakeHit(float damage)
    {
        _currentHealth -= damage;
        //Debug.Log("Remaining Health: " + _currentHealth);

        if(_currentHealth <= 0.0f)
        {

            //TODO: Signal to the player to gain a resource
            //Debug.Log("Gained " + _amount + " " + _type.ToString());


            Destroy(gameObject);
        }
    }
}
