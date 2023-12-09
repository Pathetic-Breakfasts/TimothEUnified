using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameDevTV.Inventories;
using Unity.VisualScripting;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Weapon _playerWeapon;
    ActiveTool _activeTool;

    [SerializeField] Transform _weaponAttach;

    [SerializeField] LayerMask _interactableLayer;

    [SerializeField] InventoryLoadout _startingLoadout;

    GameDevTV.Inventories.Inventory _inventory;

    CharacterEnergy _characterEnergy;
    Mover _mover;
    DayManager _dayManager;
    Animator _animator;
    Health _playerHealth;

    UIManager _uiManager;

    [SerializeField] GameObject _heldItemGO;

    Vector2 _movement;
    Vector2 _lastMovment;
    Vector2 _mousePosAtClick;
    Vector2 _mousePos;

    InventoryItem _currentItem;

    InteractDirection _toolUseDirection;
    public InteractDirection ToolUseDirection;

    public Bed CurrentBed { get => _bed; set { _bed = value; UpdatePrompts(); } }
    Bed _bed;

    public DoorController NearbyDoor { get => _doorController; set { _doorController = value; UpdatePrompts(); } }
    DoorController _doorController;
    public bool IsHeavyAttack { get => _isHeavyAttack; set => _isHeavyAttack = value; }
    bool _isHeavyAttack = false;

    private void Awake()
    {
        _characterEnergy = GetComponent<CharacterEnergy>();
        _animator = GetComponent<Animator>();
        _activeTool = GetComponent<ActiveTool>();
        _mover = GetComponent<Mover>();
        _playerHealth = GetComponent<Health>();
        _inventory = GetComponent<Inventory>();
    }

    private void Start()
    {
        _activeTool.ChangeTool(null);

        _dayManager = FindObjectOfType<DayManager>();
        _uiManager = FindObjectOfType<UIManager>();

        if (_startingLoadout)
        {
            _startingLoadout.SpawnItems(_inventory);
        }
    }

    private void FixedUpdate()
    {
        if (_movement == Vector2.zero) return;

        _mover.Move(_movement);

        _animator.SetFloat("LastHorizontal", _movement.x);
        _animator.SetFloat("LastVertical", _movement.y);
        _lastMovment = _movement;
    }

    private void Update()
    {
        if (Debug.isDebugBuild)
        {
            DebugUpdate();
        }

        _mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (_activeTool.UsingTool || _playerWeapon.IsAttacking)
        {
            _movement = Vector2.zero;
            _animator.SetFloat("Speed", 0.0f);
        }

        if (!_playerWeapon.IsAttacking && _playerWeapon.HasWeapon && _playerWeapon.IsRanged)
        {
            _playerWeapon.SetAimPosition(_mousePos);
        }

    }

    private void DebugUpdate()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            _dayManager.ProgressDay();
        }
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

        if (_doorController)
        {
            _uiManager.SetInputPromptVisibility(true);
            _uiManager.SetInputPromptText("Use Door");
            return;
        }

        _uiManager.SetInputPromptVisibility(false);

    }

    public void EquipItem(InventoryItem item)
    {
        _currentItem = item;

        //Reset state for changing item
        _heldItemGO.SetActive(false);
        _activeTool.ChangeTool(null);
        _playerWeapon.EquipWeapon(null);
        _animator.SetBool("InCombatMode", false);

        if (!item)
        {
            return;
        }

        //Handle state based off our item type
        switch (item.itemType)
        {
            case ItemType.HOLDABLE:
                _heldItemGO.SetActive(true);
                _heldItemGO.GetComponent<SpriteRenderer>().sprite = item.icon;

                break;
            case ItemType.SEED:
                _heldItemGO.SetActive(true);
                _heldItemGO.GetComponent<SpriteRenderer>().sprite = item.icon;

                break;
            case ItemType.WEAPON:
                _playerWeapon.EquipWeapon(item);

                break;
            case ItemType.TOOL:
                _activeTool.ChangeTool(item);

                break;
            case ItemType.ARMOR:

                break;
            default:
                Debug.LogWarning("Non-Supported Item type passed into EquipItem");
                break;
        }
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

    public void InteractPressed()
    {
        //Order of priority for Interactions
        //Doors
        //Chest
        //NPCs
        //Harvestable Plant
        //Use Tool/Weapon

        //Doors
        if (_doorController)
        {
            //TODO: Use door enter logic

            return;
        }

        //Farmland
        if (Vector2.Distance(_mousePosAtClick, transform.position) < 1.75f)
        {
            RaycastHit2D hit = Physics2D.Raycast(_mousePosAtClick, Vector2.zero, 10.0f, _interactableLayer);
            if (hit.collider != null)
            {
                FarmableLand farmableLand = hit.collider.GetComponent<FarmableLand>();
                if (farmableLand)
                {
                    if (farmableLand.ReadyToHarvest())
                    {
                        farmableLand.Harvest();
                        return;
                    }
                    else if (farmableLand.ReadyToPlant())
                    {
                        if (_currentItem && _currentItem.itemType == ItemType.SEED)
                        {
                            farmableLand.Plant(_currentItem);
                            _inventory.RemoveItem(_currentItem, 1);

                            if (!_inventory.HasItem(_currentItem, 1))
                            {
                                EquipItem(null);
                            }
                            return;
                        }
                    }
                }
            }
        }

        //Tools/Weapons
        UseEquipped();
    }

    public void InspectPressed()
    {

    }

    public void UseEquipped()
    {
        if (_playerWeapon.IsAttacking || _activeTool.UsingTool || !_currentItem)
        {
            return;
        }

        _mousePosAtClick = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        _toolUseDirection = GameUtilities.CalculateDirection(gameObject.transform.position, _mousePosAtClick);
        Vector2 directionVec = GameUtilities.GetDirectionVector(_toolUseDirection);

        _animator.SetFloat("CombatHorizontal", directionVec.x);
        _animator.SetFloat("CombatVertical", directionVec.y);
        _animator.SetFloat("LastHorizontal", directionVec.x);
        _animator.SetFloat("LastVertical", directionVec.y);

        if (_playerWeapon.HasWeapon && !_playerWeapon.IsRanged)
        {
            float angle = Mathf.Atan2(directionVec.y, directionVec.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            _weaponAttach.eulerAngles = rotation.eulerAngles;
            _playerWeapon.SetCurrentDirection(_toolUseDirection);
            
        }

        if (_currentItem.itemType == ItemType.WEAPON)
        {
            _playerWeapon.StartSwing(null, _isHeavyAttack);
        }
        else if(_currentItem.itemType == ItemType.TOOL)
        {
            if (_activeTool.HasTool)
            {
                _activeTool.UseTool(_mousePosAtClick);
                _characterEnergy.UseEnergy(_activeTool.EnergyConsumption);
            }
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
        else if (_doorController)
        {

        }
    }
}
