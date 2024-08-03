using UnityEngine;

//////////////////////////////////////////////////
public class Structure : MonoBehaviour, IStructure
{

    protected bool _bIsBuilt = false;
    int _currentBuildTime = 0;

    [SerializeField] StructureConfig _config;

    [SerializeField] bool _bUseHourlyUpdates = false;
    [SerializeField] bool _bUseDailyUpdates = false;

    //////////////////////////////////////////////////
    protected void Start()
    {
        TimeManager timeManager = FindObjectOfType<TimeManager>();
        if (!timeManager)
        {
            Debug.LogError(gameObject.name + " cannot find a TimeManager object!");
            return;
        }

        if (_config && _config.hoursToBuild > 0)
        {
            timeManager.OnHourChanged += OnHourElapsed;
        }
        else
        {
            _bIsBuilt = true;
        }

        if (_bUseHourlyUpdates)
        {
            timeManager.OnHourChanged -= OnHourElapsed;
            timeManager.OnHourChanged += OnHourElapsed;
        }

        if (_bUseDailyUpdates)
        {
            timeManager.OnDayChanged += OnDayElapsed;
        }
    }

    //IStructure Start
    //////////////////////////////////////////////////
    public StructureConfig GetConfig()
    {
        return _config;
    }

    //////////////////////////////////////////////////
    public void OnConstruction()
    {
    }

    //////////////////////////////////////////////////
    public void OnDayElapsed()
    {

    }

    //////////////////////////////////////////////////
    public void OnDestruction()
    {
        throw new System.NotImplementedException();
    }

    //////////////////////////////////////////////////
    public void OnHourElapsed()
    {
        if(!_bIsBuilt && _config.hoursToBuild > 0)
        {
            _currentBuildTime++;
            if(_currentBuildTime >= _config.hoursToBuild)
            {
                _bIsBuilt = true;

                //Unsubcribe from the hour elapsed event if we do not need it
                if (!_bUseHourlyUpdates)
                {
                    TimeManager timeManager = FindObjectOfType<TimeManager>();
                    timeManager.OnHourChanged -= OnHourElapsed;
                }
                OnConstruction();
            }
        }
    }

    //////////////////////////////////////////////////
    public bool IsBuilt()
    {
        return _bIsBuilt;
    }
    //IStructure End
}
