using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStructure
{
    void OnConstruction();
    void OnDestruction();
    void OnHourElapsed();
    void OnDayElapsed();

    StructureConfig GetConfig();
}
