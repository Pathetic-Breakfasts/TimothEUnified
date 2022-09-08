using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum Days
{
    Monday,
    Tuesday,
    Wednesday,
    Thursday,
    Friday,
    Saturday,
    Sunday,
    EndWeek
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

public enum TimeFormat
{
    Hour_24,
    Hour_12
}

public class DayManager : MonoBehaviour
{
    [SerializeField] Seasons _currentSeason;
    public Seasons CurrentSeason { get => _currentSeason; }
    public TimeFormat _timeFormat = TimeFormat.Hour_12;

    public bool _isDay;

    public int _currentDay;
    public  int _currentYear;
    public  Days _currentDayOfWeek;
    public  int _currentWeekOfMonth;

    double _currentDayTimer = 0.0f;
    private bool _isAM = false;

    public TextMeshProUGUI[] _timeText;
    public TextMeshProUGUI[] _seasonText;
    
    int _hours;
    int _minutes;

    public float _secsPerMinute = 60;
    int _hoursPerDay = 24;
    int _minsPerHour = 60;
    int _monthsPerYear = 12;

    private string _timeString;
    private string _seasonString;

    private SeasonPhase _currentSeasonPhase;

    float _lengthOfDay = 180.0f; //180 is temp
    private void Awake()
    {
        //Set Temp Date and Time
        _hours              = 8;
        _minutes            = 0;
        _currentSeason      = Seasons.Spring;
        _currentSeasonPhase = SeasonPhase.Early;
        _currentDay         = 1;
        _currentYear        = 1;
        _currentWeekOfMonth = 1;

        if (_hours < 12)
        {
            _isAM = true;
        }

        SetTimeDateString();
    }

    public void Update()
    {
        
        if(_currentDayTimer >= _secsPerMinute)
        {
            _minutes++;
            if(_minutes >= _minsPerHour){
                _minutes = 0;
                _hours++;
                if(_hours >= _hoursPerDay)
                {
                    _hours = 0;
                    
                    //if(_currentDayOfWeek == Days.Sunday)
                    //{
                    //    ProgressWeek();
                        
                    //}
                    ProgressDay();
                    
                }
            }
            SetTimeDateString();
            _currentDayTimer = 0;
        }
        else
        {
            _currentDayTimer += Time.deltaTime;
        }

        if(_hours < 12)
        {
            _isAM = true;
        }
        else
        {
            _isAM = false;
        }
    }

    public void ProgressDay()
    {
        _currentDay++;
        
        //Day Count Progression
        if(_currentDay == 84 * _currentYear)
        {
            ProgressYear();
        }
        _currentDayOfWeek++; 
        if(_currentDayOfWeek == Days.EndWeek)
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
        _currentDayOfWeek = Days.Monday;
        
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
        _currentSeasonPhase = SeasonPhase.Early;
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
                
                break;
        }
    }

    public void SleepSkipTime()
    {

    }


    public void ProgressYear()
    {
        _currentYear++;
    }

    void SetTimeDateString()
    {
        switch (_timeFormat)
        {
            case TimeFormat.Hour_12:
                {
                    int h;
                    
                    if(_hours >= 13)
                    {
                        h = _hours - 12;
                        
                    }
                    else if (_hours == 0)
                    {
                        h = 12;
                        
                    }
                    else
                    {
                        h = _hours;
                    }
                    _timeString = h + ":";

                    if (_minutes <= 9)
                    {
                        _timeString += "0" + _minutes;
                    }
                    else
                    {
                        _timeString += _minutes;
                    }
                }

                if (_isAM)
                {
                    _timeString += " AM";
                }
                else
                {
                    _timeString += " PM";
                }
                break;

            case TimeFormat.Hour_24:
                {
                    if (_hours <= 9)
                    {
                        _timeString = "0" + _hours + ":";
                    }
                    else
                    {
                        _timeString = _hours + ":";
                    }

                    if (_minutes <= 9)
                    {
                        _timeString += "0" + _minutes;
                    }
                    else
                    {
                        _timeString += _minutes;
                    }


                       
                }
                break;

            default:

                break;

                
        }
        _seasonString = _currentSeasonPhase.ToString() + " " + _currentSeason.ToString();
        

        for (int i = 0; i < _timeText.Length; i++)
        {
            _timeText[i].text = _timeString;
        }
        
        for (int i = 0; i < _seasonText.Length; i++)
        {
            _seasonText[i].text = _seasonString;
        }
    }

  


}
