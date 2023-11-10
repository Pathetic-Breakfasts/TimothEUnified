using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface CharacterResources
{
    public void TakeResource(float amount);

    public bool IsOutOf();
}
