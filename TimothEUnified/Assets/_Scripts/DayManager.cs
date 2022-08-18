using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayManager : MonoBehaviour
{
    [SerializeField] Seasons _currentSeason;
    public Seasons CurrentSeason { get => _currentSeason; }

    public void NewDay()
    {
        FarmableLand[] fls = FindObjectsOfType<FarmableLand>();

        foreach (FarmableLand fl in fls)
        {
            if(fl.IsOccupied)
            {
                Crop c = fl.GetComponentInChildren<Crop>();
                if (c)
                {
                    c.ProgressDay();
                }
            }
        }
    }
}
