using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //TEMP
    [SerializeField] ToolConfig _axeConfig;
    [SerializeField] ToolConfig _pickaxeConfig;
    [SerializeField] ToolConfig _hoeConfig;
    CropConfig _selectedConfig;

    //TEMP ITEMS
    //[SerializeField] ItemConfig _itemConfig1;
    //[SerializeField] ItemConfig _itemConfig2;
    //[SerializeField] ItemConfig _itemConfig3;
    //[SerializeField] ItemConfig _itemConfig4;
    //[SerializeField] ItemConfig _itemConfig5;
    //[SerializeField] ItemConfig _itemConfig6;

    [SerializeField] Weapon _playerWeapon;
    ActiveTool _activeTool;

    [SerializeField] Transform _weaponAttach;

    [SerializeField] LayerMask _interactableLayer;
    [SerializeField] CropConfig _carrotConfig;
    [SerializeField] CropConfig _tomatoesConfig;
    [SerializeField] CropConfig _lettuceConfig;

    CharacterEnergy _characterEnergy;
    Mover _mover;
    DayManager _dayManager;
    Animator _animator;
    Health _playerHealth;

    UIManager _uiManager;

    Vector2 _movement;
    Vector2 _mousePosAtClick;

    public Bed CurrentBed { get => _bed; set { _bed = value; UpdatePrompts(); } }
    Bed _bed;

    public DoorController NearbyDoor { get => _doorController; set { _doorController = value; UpdatePrompts(); } }
    DoorController _doorController;
    public bool IsHeavyAttack { get => _isHeavyAttack; set=>_isHeavyAttack = value; }
    bool _isHeavyAttack = false;

    public bool IsInCombatMode { get => _isInCombatMode; set 
        { 
            _isInCombatMode = value;
            _weaponAttach.GetChild(0).gameObject.SetActive(_isInCombatMode);
            _animator.SetBool("InCombatMode", _isInCombatMode);
        } 
    }
    bool _isInCombatMode = false;

    private void Awake()
    {
        _characterEnergy = GetComponent<CharacterEnergy>();
        _animator = GetComponent<Animator>();
        _activeTool = GetComponent<ActiveTool>();
        _mover = GetComponent<Mover>();
        _playerHealth = GetComponent<Health>();
    }

    private void Start()
    {
        _activeTool.ChangeTool(_hoeConfig);
        _selectedConfig = _carrotConfig;

        _dayManager = FindObjectOfType<DayManager>();
        _uiManager = FindObjectOfType<UIManager>();
    }

    private void FixedUpdate()
    {
        if (_movement == Vector2.zero) return;

        _mover.Move(_movement);

        _animator.SetFloat("LastHorizontal", _movement.x);
        _animator.SetFloat("LastVertical", _movement.y);


    }

    private void Update()
    {
        if (Debug.isDebugBuild)
        {
            DebugUpdate();
        }  


        if(_activeTool.UsingTool || _playerWeapon.IsAttacking)
        {
            _movement = Vector2.zero;
            _animator.SetFloat("Speed", 0.0f);
        }

        if(!_playerWeapon.IsAttacking) 
        {
            Vector2 mPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Vector2 dir = mPos - (Vector2)transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            _weaponAttach.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

    }

    private void DebugUpdate()
    {
        //TESTING CODE START
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _activeTool.ChangeTool(_axeConfig);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _activeTool.ChangeTool(_pickaxeConfig);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            _activeTool.ChangeTool(_hoeConfig);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            _selectedConfig = _carrotConfig;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            _selectedConfig = _tomatoesConfig;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            _selectedConfig = _lettuceConfig;
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            _dayManager.ProgressDay();
        }

        //INV TEST CODE
        //if (Input.GetKeyDown(KeyCode.I))
        //{
        //    if (_inventoryStore != null)
        //    {
        //        _inventoryStore.AddItem(_itemConfig1, 1);
        //        _inventoryStore.AddItem(_itemConfig2, 2);
        //        _inventoryStore.AddItem(_itemConfig3, 3);
        //        _inventoryStore.AddItem(_itemConfig4, 4);
        //        _inventoryStore.AddItem(_itemConfig5, 5);
        //        _inventoryStore.AddItem(_itemConfig6, 500);
        //    }
        //}
        //END OF INV TEST CODE
    }

    public void SetMovement(Vector2 movement)
    {
        _animator.SetFloat("Horizontal", movement.x);
        _animator.SetFloat("Vertical", movement.y);
        _animator.SetFloat("Speed", movement.sqrMagnitude);
        _movement = movement;
    }

    private void UpdatePrompts()
    {
        if (_bed)
        {
            _uiManager.SetInputPromptVisibility(true);
            _uiManager.SetInputPromptText("Use Bed");
            return;
        }
        
        if(_doorController)
        {
            _uiManager.SetInputPromptVisibility(true);
            _uiManager.SetInputPromptText("Use Door");
            return;
        }

        _uiManager.SetInputPromptVisibility(false);

    }

    public void OnEnergyChanged()
    {
        //TODO: Remove this hard codedness
        if (_characterEnergy.GetEnergyRatio() < 0.25f)
        {
            //TODO: Display a UI Prompt for first time passing this
            //Also add in a proper on energy changed event that invokes when energy is consumed (Saves: processing time and unlinks systems)
            _mover.MovementSpeedRatio = 0.5f;
        }
        else
        {
            _mover.MovementSpeedRatio = 1.0f;
        }

        _uiManager?.SetEnergyBarFillRatio(_characterEnergy.GetEnergyRatio());
    }

    public void OnHealthChanged()
    {
        _uiManager?.SetHealthBarFillRatio(_playerHealth.HealthRatio);
    }

    public void UseEquipped()
    {
        if(_playerWeapon.IsAttacking || _activeTool.UsingTool)
        {
            return;
        }

        _mousePosAtClick = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        InteractDirection direction = GameUtilities.CalculateDirection(gameObject.transform.position, _mousePosAtClick);
        Vector2 directionVec = GameUtilities.GetDirectionVector(direction);

        _animator.SetFloat("CombatHorizontal", directionVec.x);
        _animator.SetFloat("CombatVertical", directionVec.y);
        _animator.SetFloat("LastHorizontal", directionVec.x);
        _animator.SetFloat("LastVertical", directionVec.y);

        if (_isInCombatMode)
        {
            _playerWeapon.StartSwing(null, _isHeavyAttack);
        }
        else
        {
            if (Vector2.Distance(_mousePosAtClick, transform.position) < 1.75f)
            {
                RaycastHit2D hit = Physics2D.Raycast(_mousePosAtClick, Vector2.zero, 10.0f, _interactableLayer);
                if (hit.collider != null)
                {
                    FarmableLand farmableLand = hit.collider.GetComponent<FarmableLand>();
                    if (farmableLand && farmableLand.ReadyToPlant())
                    {
                        farmableLand.Plant(_selectedConfig);
                        return;
                    }
                    else if (farmableLand && farmableLand.ReadyToHarvest())
                    {
                        farmableLand.Harvest();
                        return;
                    }
                }
            }

            _activeTool.UseTool(_mousePosAtClick);
            _characterEnergy.UseEnergy(_activeTool.EnergyConsumption);
        }

    }

    public void UseInteractable()
    {
        if (_bed)
        {
            //TODO: Spawn UI Prompt for specifying amount of sleep to get
            //TODO: Add player energy per hour of sleep 
            _bed.Sleep(1);
            _characterEnergy.RegainEnergy(25.0f);
        }
        else if(_doorController)
        {

        }
    }
}
