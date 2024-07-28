using UnityEngine;
using GameFramework.Core.Dev;

public enum Days
{
    Monday = 1,
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



public class TimeManager : MonoBehaviour
{
    [SerializeField] private float _secsPerMinute = 60;

    private bool _isDay;

    public Days CurrentDay { get => (Days)(_currentDay % 7); }
    private Seasons _currentSeason = Seasons.Spring;

    private int _currentDay = 1;
    private int _currentYear = 1;
    private Days _currentDayOfWeek = Days.Monday;
    private  int _currentWeekOfMonth = 1;


    public Seasons CurrentSeason { get => _currentSeason; }
    public bool IsTimePaused { get => _bIsTimePaused; set => _bIsTimePaused = value; }

    private bool _bIsTimePaused = false;

    double _currentSecondTimer = 0.0f;


    public int Hour { get => _hours; }
    public int Minute { get => _minutes; }

    int _hours = 6;
    int _minutes = 0;

    int _hoursPerDay = 24;
    int _minsPerHour = 60;

    public static DebugCommand PROGRESS_HOUR; 
    public static DebugCommand PROGRESS_DAY;
    public static DebugCommand PROGRESS_WEEK;
    public static DebugCommand PROGRESS_SEASON;
    public static DebugCommand PROGRESS_YEAR;

    //////////////////////////////////////////////////
    private void Start()
    {
        DebugController.Instance.CreateCommand(PROGRESS_HOUR, "time_progressHour", "Progresses game time by one hour", () => ProgressHour(1));
        DebugController.Instance.CreateCommand(PROGRESS_DAY, "time_progressDay", "Progresses game time by one day", () =>  ProgressDay());
        DebugController.Instance.CreateCommand(PROGRESS_WEEK, "time_progressWeek", "Progresses game time by one week", () => ProgressWeek());
        DebugController.Instance.CreateCommand(PROGRESS_SEASON, "time_progressSeason", "Progresses game time by one season", () => ProgressSeason());
        DebugController.Instance.CreateCommand(PROGRESS_YEAR, "time_progressYear", "Progresses game time by one year", () => ProgressYear());
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
            _currentSecondTimer = 0;
        }
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
}
