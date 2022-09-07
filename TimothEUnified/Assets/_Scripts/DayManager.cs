using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Days
{
    Monday,
    Tuesday,
    Wednesday,
    Thursday,
    Friday,
    Saturday,
    Sunday
}
public enum Seasons
{
    Spring,
    Summer,
    Autumn,
    Winter
}

public enum SeasonPhase
{
    Early = 7,
    Mid = 14,
    Late = 21
}



public class DayManager : MonoBehaviour
{
    [SerializeField] Seasons _currentSeason;
    public Seasons CurrentSeason { get => _currentSeason; }

    public bool _isDay;

    private int _currentDay;
    private int _currentYear;
    private Days _currentDayOfWeek;
    float _currentDayTimer = 0.0f;
    private int _currentWeekOfMonth;

    private SeasonPhase _currentSeasonPhase;

    float _lengthOfDay = 180.0f; //180 is temp


    public void ProgressDay()
    {
        _currentDay++;
        //Day Count Progression
        if(_currentDay == 84 * _currentYear)
        {
            ProgressYear();
        }
        _currentDayOfWeek++; 
        if(_currentDayOfWeek == Days.Sunday)
        {
            ProgressWeek();
        }

        //Crop Progression
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

    public void StartNight()
    {

    }

    public void ProgressWeek()
    {
        _currentWeekOfMonth++;
        if (_currentWeekOfMonth == 1)
        {
            _currentSeasonPhase = SeasonPhase.Early;
        }
        else if (_currentWeekOfMonth == 2)
        {
            _currentSeasonPhase = SeasonPhase.Mid;
        }
        else if (_currentWeekOfMonth == 3)
        {
            _currentSeasonPhase = SeasonPhase.Late;
        }
        else if (_currentWeekOfMonth == 4)
        {
            ProgressSeason();
        }

    }

    public void ProgressSeason()
    {
        _currentWeekOfMonth = 1;
        switch (_currentSeason)
        {
            case Seasons.Spring:
                _currentSeason = Seasons.Summer;
                _lengthOfDay = 600;
                break;
            case Seasons.Summer:
                _currentSeason = Seasons.Autumn;
                _lengthOfDay = 540;
                break;
            case Seasons.Autumn:
                _currentSeason = Seasons.Winter;
                _lengthOfDay = 480;
                break;
            case Seasons.Winter:
                _currentSeason = Seasons.Spring;
                _lengthOfDay = 540;
                break;
            default:
                _currentSeason = Seasons.Spring;
                break;
        }
    }


    public void ProgressYear()
    {
        _currentYear++;
    }





    
}
