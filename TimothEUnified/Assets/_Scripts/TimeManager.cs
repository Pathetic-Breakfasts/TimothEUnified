using UnityEngine;
using GameFramework.Core.Dev;
using System;

public enum Days
{
    Sunday,
    Monday,
    Tuesday,
    Wednesday,
    Thursday,
    Friday,
    Saturday
}
public enum Seasons
{
    Spring = 1,
    Summer,
    Autumn,
    Winter,
}


public class TimeManager : MonoBehaviour
{
    [SerializeField] private float _secsPerMinute = 60;

    public Days CurrentDay { get => (Days)(_currentDay % 7); }
    public Seasons CurrentSeason { get => (Seasons)_currentSeason; }
    public int CurrentYear { get => _currentYear; }

    private int _currentDay = 1;
    private int _currentSeason = 1;
    private int _currentYear = 1;

    public bool IsTimePaused { get => _bIsTimePaused; set => _bIsTimePaused = value; }

    private bool _bIsTimePaused = false;

    double _currentSecondTimer = 0.0f;

    public int Hour { get => _hours; }
    public int Minute { get => _minutes; }

    int _hours = 6;
    int _minutes = 0;

    int _hoursPerDay = 24;
    int _minsPerHour = 60;
    int _daysInSeason = 28;
    int _seasonsInYear = 4;

    public Action OnHourChanged;
    public Action OnDayChanged;
    public Action OnSeasonChanged;
    public Action OnYearChanged;

    public static DebugCommand PROGRESS_HOUR; 
    public static DebugCommand PROGRESS_DAY;
    public static DebugCommand PROGRESS_SEASON;
    public static DebugCommand PROGRESS_YEAR;

    //////////////////////////////////////////////////
    private void Start()
    {
        DebugController.Instance.CreateCommand(PROGRESS_HOUR, "time_progressHour", "Progresses game time by one hour", () => ProgressHour(1));
        DebugController.Instance.CreateCommand(PROGRESS_DAY, "time_progressDay", "Progresses game time by one day", () =>  ProgressDay());
        DebugController.Instance.CreateCommand(PROGRESS_SEASON, "time_progressSeason", "Progresses game time by one season", () => ProgressSeason());
        DebugController.Instance.CreateCommand(PROGRESS_YEAR, "time_progressYear", "Progresses game time by one year", () => ProgressYear());
    }

    //////////////////////////////////////////////////
    public void Update()
    {
        //Early-Out if time is paused
        if(_bIsTimePaused)
        {
            return;
        }

        _currentSecondTimer += Time.deltaTime;
        if (_currentSecondTimer >= _secsPerMinute)
        {
            _currentSecondTimer = 0;
            _minutes++;
            if(_minutes >= _minsPerHour){
                ProgressHour(1);
            }
        }
    }

    //////////////////////////////////////////////////
    public void SleepSkipTime(int desiredSleep)
    {
        ProgressHour(desiredSleep);
    }

    //////////////////////////////////////////////////
    private void ProgressHour(int numHours)
    {
        OnHourChanged.Invoke();
        _minutes = 0;
        _hours++;

        if (_hours > _hoursPerDay)
        {
            ProgressDay();
        }
    }

    //////////////////////////////////////////////////
    public void ProgressDay()
    {
        OnDayChanged.Invoke();
        _currentDay++;
        _hours = 1;
        _minutes = 0;
        
        //Day Count Progression
        if(_currentDay == _daysInSeason + 1)
        {
            ProgressSeason();
        }
    }

    //////////////////////////////////////////////////
    private void ProgressSeason()
    {
        OnSeasonChanged.Invoke();
        _currentDay = 1;
        _currentSeason++;
        if(_currentSeason == _currentSeason + 1)
        {
            ProgressYear();
        }
    }

    //////////////////////////////////////////////////
    private void ProgressYear()
    {
        OnYearChanged.Invoke();
        _currentSeason = 1;
        _currentYear++;
    }
}
