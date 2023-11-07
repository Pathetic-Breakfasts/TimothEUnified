using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using TMPro.EditorUtilities;

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
    [SerializeField] TimeFormat _timeFormat = TimeFormat.Hour_12;

    [SerializeField] private bool _isDay;

    [SerializeField] private int _currentDay;
    [SerializeField] private int _currentYear;
    [SerializeField] private Days _currentDayOfWeek;
    [SerializeField] private  int _currentWeekOfMonth;
    [SerializeField] private float _secsPerMinute = 60;


    [SerializeField] private TextMeshProUGUI _timeText;
    [SerializeField] private TextMeshProUGUI _seasonText;

    public Seasons CurrentSeason { get => _currentSeason; }
    public bool IsTimePaused { get => _bIsTimePaused; set => _bIsTimePaused = value; }

    private bool _bIsTimePaused = false;

    double _currentSecondTimer = 0.0f;
    private bool _isAM = false;
    int _hours;
    int _minutes;

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
        _hours              = 6;
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
    }

    public void StartNight()
    {
        _bIsTimePaused = true;
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
                _lengthOfDay = 600;
                break;
            case Seasons.Summer:
                _lengthOfDay = 540;
                break;
            case Seasons.Autumn:
                _lengthOfDay = 480;
                break;
            case Seasons.Winter:
                _lengthOfDay = 540;
                break;
            default:
                break;
        }
    }

    public void SleepSkipTime()
    {
        _hours++;
    }


    public void ProgressYear()
    {
        _currentYear++;
    }

    private void ProgressHour(int numHours)
    {
        _minutes = 0;
        _hours++;

        if (_hours >= _hoursPerDay)
        {
            ProgressDay();
        }
    }

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
        _seasonString = _currentSeasonPhase.ToString() + " " + _currentSeason.ToString();

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
