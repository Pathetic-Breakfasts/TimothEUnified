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

public enum TimeFormat
{
    Hour_24,
    Hour_12
}

public class DayManager : MonoBehaviour
{
    [SerializeField] TimeFormat _timeFormat = TimeFormat.Hour_12;
    [SerializeField] private float _secsPerMinute = 60;
    [SerializeField] private TextMeshProUGUI _timeText;
    [SerializeField] private TextMeshProUGUI _seasonText;

    private bool _isDay;

    private Seasons _currentSeason = Seasons.Spring;
    private int _currentDay = 1;
    private int _currentYear = 1;
    private Days _currentDayOfWeek = Days.Monday;
    private  int _currentWeekOfMonth = 1;


    public Seasons CurrentSeason { get => _currentSeason; }
    public bool IsTimePaused { get => _bIsTimePaused; set => _bIsTimePaused = value; }

    private bool _bIsTimePaused = false;

    double _currentSecondTimer = 0.0f;
    private bool _isAM = false;
    int _hours = 6;
    int _minutes = 0;

    int _hoursPerDay = 24;
    int _minsPerHour = 60;

    private string _timeString;
    private string _seasonString;

    //////////////////////////////////////////////////
    private void Awake()
    {
        if (_hours < 12)
        {
            _isAM = true;
        }

        SetTimeDateString();
    }

    //////////////////////////////////////////////////
    private void Start()
    {
        if(!_timeText)
        {
            Debug.LogError("DayManager " + gameObject.name + " is missing TimeText!");
        }

        if(!_seasonText)
        {
            Debug.LogError("DayManager " + gameObject.name + " is missing SeasonText");
        }
    }

    //////////////////////////////////////////////////
    public void Update()
    {
        if(Debug.isDebugBuild)
        {
            DebugUpdate();
        }

        //Early-Out if time is paused
        if(_bIsTimePaused)
        {
            return;
        }

        _currentSecondTimer += Time.deltaTime;
        if (_currentSecondTimer >= _secsPerMinute)
        {
            _minutes++;
            if(_minutes >= _minsPerHour){
                ProgressHour(1);
            }
            SetTimeDateString();
            _currentSecondTimer = 0;
        }

        _isAM = _hours < 12;
    }

    //////////////////////////////////////////////////
    private void DebugUpdate()
    {
        if(Input.GetKeyDown(KeyCode.P)) 
        {
            Debug.Log("Skipping Day");
            ProgressDay();
        }
        if(Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log("Skipping Hour");
            ProgressHour(1);
        }
    }

    //////////////////////////////////////////////////
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
        FarmableLand[] farmableLand = FindObjectsOfType<FarmableLand>();
        foreach (FarmableLand farmland in farmableLand)
        {
            farmland.ProgressDay();
        }

        _hours = 0;
        _minutes = 0;

        _bIsTimePaused = false;

        StartNight();
    }

    //////////////////////////////////////////////////
    public void StartNight()
    {
        IsTimePaused = true;
    }

    //////////////////////////////////////////////////
    public void EndNight()
    {
        IsTimePaused = false;

        ProgressHour(2);
    }

    //////////////////////////////////////////////////
    public void ProgressWeek()
    {
        _currentWeekOfMonth++;
        _currentDayOfWeek = Days.Monday;
        
        if (_currentWeekOfMonth == 4)
        {
            ProgressSeason();
        }

    }

    //////////////////////////////////////////////////
    public void ProgressSeason()
    {
        _currentWeekOfMonth = 1;
    }

    //////////////////////////////////////////////////
    public void SleepSkipTime(int desiredSleep)
    {
        ProgressHour(desiredSleep);
    }

    //////////////////////////////////////////////////
    public void ProgressYear()
    {
        _currentYear++;
    }

    //////////////////////////////////////////////////
    private void ProgressHour(int numHours)
    {
        _minutes = 0;
        _hours++;

        if (_hours >= _hoursPerDay)
        {
            ProgressDay();
        }
    }

    //////////////////////////////////////////////////
    void SetTimeDateString()
    {
        switch (_timeFormat)
        {
            case TimeFormat.Hour_12:
            {
                int sanitisedHours = (_hours % 12) + 1;
                _timeString = sanitisedHours < 10 ? "0" + sanitisedHours + ":" : sanitisedHours + ":";
                _timeString += _minutes < 10 ? "0" + _minutes.ToString() : _minutes.ToString();
                _timeString += _isAM ? " AM" : "PM";
                break;
            }
            case TimeFormat.Hour_24:
            {
                _timeString += _hours < 10 ? "0" + _hours.ToString() : _hours.ToString();
                _timeString += _minutes < 10 ? "0" + _minutes.ToString() : _minutes.ToString();
                break;
            }
        }

        if (_timeText)
        {
            _timeText.text = _timeString;
        }

        if (_seasonText)
        {
            _seasonText.text = _seasonString;
        }
    }
}
