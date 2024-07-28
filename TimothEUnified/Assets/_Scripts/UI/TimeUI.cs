using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.UI;

public enum TimeFormat
{
    Hour_24,
    Hour_12
}

public class TimeUI : MonoBehaviour
{
    TimeManager _timeManager;

    [SerializeField] TimeFormat _timeFormat = TimeFormat.Hour_12;

    [SerializeField] private TextMeshProUGUI _timeText;
    [SerializeField] private TextMeshProUGUI _seasonText;
    [SerializeField] private TextMeshProUGUI _dayText;

    //////////////////////////////////////////////////
    private void Awake()
    {
        _timeManager = FindObjectOfType<TimeManager>();

        if (!_timeText)
        {
            Debug.LogError("DayManager " + gameObject.name + " is missing TimeText!");
        }

        if (!_seasonText)
        {
            Debug.LogError("DayManager " + gameObject.name + " is missing SeasonText");
        }
    }

    //////////////////////////////////////////////////
    private void Update()
    {
        SetTimeDateString();
    }

    //////////////////////////////////////////////////
    void SetTimeDateString()
    {
        if (_timeText)
        {
            string timeString = "";
            int hour = _timeManager.Hour;
            int minute = _timeManager.Minute;

            switch (_timeFormat)
            {
                case TimeFormat.Hour_12:
                    {
                        int sanitisedHours = (hour % 12) + 1;
                        timeString = sanitisedHours < 10 ? "0" + sanitisedHours + ":" : sanitisedHours + ":";
                        timeString += minute < 10 ? "0" + minute.ToString() : minute.ToString();
                        timeString += hour < 12 ? " AM" : " PM";
                        break;
                    }
                case TimeFormat.Hour_24:
                    {
                        timeString += hour < 10 ? "0" + hour.ToString() : hour.ToString();
                        timeString += minute < 10 ? "0" + minute.ToString() : minute.ToString();
                        break;
                    }
            }
            _timeText.text = timeString;
        }

        if (_dayText)
        {
            Days day = _timeManager.CurrentDay;
            _dayText.text = day.ToString();
        }

        if (_seasonText)
        {
            Seasons season = _timeManager.CurrentSeason;
            _seasonText.text = season.ToString();
        }
    }
}
