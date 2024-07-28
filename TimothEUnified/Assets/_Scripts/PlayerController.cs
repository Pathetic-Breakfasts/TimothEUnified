using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Inventories;
using TimothE.Gameplay.Interactables;
using TimothE.Utility;
using Cinemachine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    Weapon _playerWeapon;
    ActiveTool _activeTool;

    [SerializeField] Transform _weaponAttach;
    [SerializeField] LayerMask _interactableLayer;
    [SerializeField] LayerMask _farmableLayer;
    [SerializeField] InventoryLoadout _startingLoadout;
    [SerializeField] GameObject _heldItemGO;

    [SerializeField] Transform _buildModeTarget;

    [SerializeField] BodyPieceSpriteCollection _headSprites;
    [SerializeField] BodyPieceSpriteCollection _chestSprites;
    [SerializeField] BodyPieceSpriteCollection _armSprites;
    [SerializeField] BodyPieceSpriteCollection _legSprites;

    Inventory _inventory;
    Equipment _equipment;
    Health _playerHealth;
    CharacterEnergy _characterEnergy;

    Mover _mover;
    Animator _animator;

    TimeManager _dayManager;

    InputLayerManager _inputLayerManager;

    CoreInputLayer _coreInputLayer;
    ChestInputLayer _chestInputLayer;
    WarehouseUIInputLayer _warehouseInputLayer;
    InventoryInputLayer _inventoryInputLayer;
    BuildModeInputLayer _buildModeInputLayer;

    BuildModeController _buildModeController;

    [SerializeField] CinemachineVirtualCamera _camera;

    CurrencyStore _currencyStore;

    CharacterSpriteController _characterSpriteController;

    List<InteractableVolume> _nearbyInteractables;
    public UIManager GameUIManager { get => _uiManager; }
    UIManager _uiManager;


    Vector2 _movement;
    Vector2 _activeMousePos;
    Vector2 _mousePosAtClick;

    Vector3 _lastValidDir;
    bool _bIsInValidAimingDir = false;

    InventoryItem _currentItem;

    InteractDirection _lookDirection;
    public InteractDirection LookDirection { get => _lookDirection; }

    public bool IsHeavyAttack { get => _bIsHeavyAttack; set => _bIsHeavyAttack = value; }
    bool _bIsHeavyAttack = false;
    bool _bIsInBuildMode = false;

    //////////////////////////////////////////////////
    private void Awake()
    {
        _characterSpriteController = GetComponent<CharacterSpriteController>();
        _characterEnergy = GetComponent<CharacterEnergy>();
        _animator = GetComponent<Animator>();
        _activeTool = GetComponent<ActiveTool>();
        _playerWeapon = GetComponentInChildren<Weapon>();
        _mover = GetComponent<Mover>();
        _playerHealth = GetComponent<Health>();
        _inventory = GetComponent<Inventory>();
        _inputLayerManager = GetComponent<InputLayerManager>();
        _currencyStore = GetComponent<CurrencyStore>();
        _equipment = GetComponent<Equipment>();
        _buildModeController = FindObjectOfType<BuildModeController>();

        _coreInputLayer = new CoreInputLayer();
        _coreInputLayer.Initialize();
        _inputLayerManager.AddLayer( _coreInputLayer );

        _inventoryInputLayer = new InventoryInputLayer();
        _inventoryInputLayer.Initialize();

        _chestInputLayer = new ChestInputLayer();
        _chestInputLayer.Initialize();

        _warehouseInputLayer = new WarehouseUIInputLayer();
        _warehouseInputLayer.Initialize();

        _buildModeInputLayer = new BuildModeInputLayer();
        _buildModeInputLayer.Initialize();
        _buildModeInputLayer.BuildModeFollowTarget = _buildModeTarget;

        _nearbyInteractables = new List<InteractableVolume>();
    }

    //////////////////////////////////////////////////
    private void Start()
    {
        _activeTool.ChangeTool(null);

        _dayManager = FindObjectOfType<TimeManager>();
        _uiManager = FindObjectOfType<UIManager>();

        _uiManager.PlayerInventoryUI.DisplayedInventory = _inventory;

        _equipment.equipmentUpdated += OnEquippedArmorChanged;

        if (_startingLoadout)
        {
            _startingLoadout.SpawnItems(_inventory);
        }

        _characterSpriteController.SetSpriteSet(ArmorType.Head, _headSprites);
        _characterSpriteController.SetSpriteSet(ArmorType.Chest, _chestSprites);
        _characterSpriteController.SetSpriteSet(ArmorType.Arms, _armSprites);
        _characterSpriteController.SetSpriteSet(ArmorType.Legs, _legSprites);

        _currencyStore.GainMoney(500);
    }

    //////////////////////////////////////////////////
    private void FixedUpdate()
    {
        _mover.Move(_movement);

        Vector3 adjustedPos = transform.position + new Vector3(_movement.x, _movement.y);
        _lookDirection = GameUtilities.CalculateDirection(transform.position, adjustedPos);

        _lastValidDir = GameUtilities.GetDirectionVector(_lookDirection);

        _animator.SetFloat("LastHorizontal", _movement.x);
        _animator.SetFloat("LastVertical", _movement.y);
    }

    //////////////////////////////////////////////////
    private void Update()
    {
        if (Debug.isDebugBuild)
        {
            DebugUpdate();
        }

        _characterSpriteController.UpdateSprite();


        _activeMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (_activeTool.UsingTool || _playerWeapon.IsAttacking)
        {
            _movement = Vector2.zero;
            _animator.SetFloat("Speed", 0.0f);
        }

        //Only set our aim position when using a ranged weapon within a -45/+45 degree angle of the direction we are facing
        if (!_playerWeapon.IsAttacking && _playerWeapon.HasWeapon && _playerWeapon.IsRanged)
        {
            Vector3 adjustedPos = _activeMousePos;
            Vector3 dirVec = GameUtilities.GetDirectionVector(_lookDirection);
            Vector3 dir = (adjustedPos - transform.position).normalized;

            //Inside our bounds
            if (Vector3.Dot(dirVec, dir) > 0.5f)
            {
                _playerWeapon.SetAimPosition(adjustedPos);
                _lastValidDir = dir;
                _bIsInValidAimingDir = true;
            }
            //Outside our aiming bounds
            else
            {
                Vector3 bp = transform.position;
                bp += _lastValidDir * 5.0f;
                _playerWeapon.SetAimPosition(bp);
                _bIsInValidAimingDir = false;
            }
        }

        _inputLayerManager.UpdateLayers();
    }

    //////////////////////////////////////////////////
    private void DebugUpdate()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            _dayManager.ProgressDay();
        }
    }

    //////////////////////////////////////////////////
    private void OnEquippedArmorChanged()
    {
        _characterSpriteController.ClearOverlaySpriteSets();

        foreach (InventoryItem armor in _equipment.GetAllWornItems())
        {
            if (!armor) continue;

            _characterSpriteController.SetOverlaySpriteSet(armor.armorType, armor.armorSprites);
        }
    }

    //////////////////////////////////////////////////
    public void SetBuildModeActive(bool active)
    {
        _bIsInBuildMode = active;
        _uiManager.SetBuildMenuUIVisibility(active);
        _dayManager.IsTimePaused = active;
        _camera.Follow = active ? _buildModeTarget : transform;
        _camera.m_Lens.OrthographicSize = active ? 9.0f : 5.0f;
        _buildModeController.IsEnabled = active;

        if (active)
        {
            _inputLayerManager.AddLayer(_buildModeInputLayer);
        }
        else
        {
            if (_inputLayerManager.IsLayerInFront(_buildModeInputLayer))
            {
                _inputLayerManager.PopLayer();
            }
        }
    }

    //Called through CurrencyStore OnCurrencyChanged event
    //////////////////////////////////////////////////
    public void OnCurrencyUpdated()
    {
        _uiManager.SetCoinTextAmount(_currencyStore.CurrencyAmount);
    }

    //////////////////////////////////////////////////
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

    //////////////////////////////////////////////////
    public void OnHealthChanged()
    {
        _uiManager?.SetHealthBarFillRatio(_playerHealth.HealthRatio);
    }

    //////////////////////////////////////////////////
    public void SetMovement(Vector2 movement)
    {
        if (!_inputLayerManager.IsLayerInFront(_coreInputLayer))
        {
            _movement = Vector2.zero;
            return;
        }

        _animator.SetFloat("Horizontal", movement.x);
        _animator.SetFloat("Vertical", movement.y);
        _animator.SetFloat("Speed", movement.sqrMagnitude);
        _movement = movement;
    }

    //////////////////////////////////////////////////
    private void UpdatePrompts()
    {
        _uiManager.SetInputPromptVisibility(false);
    }

    //////////////////////////////////////////////////
    public void EquipItem(InventoryItem item)
    {
        _currentItem = item;

        //Reset state for changing item
        _heldItemGO.SetActive(false);
        _activeTool.ChangeTool(null);
        _playerWeapon.EquipWeapon(null);

        if (!item)
        {
            return;
        }

        //Handle state based off our item type
        switch (item.itemType)
        {
            case ItemType.HOLDABLE:
            case ItemType.RESOURCE:
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
                _heldItemGO.SetActive(true);
                _heldItemGO.GetComponent<SpriteRenderer>().sprite = item.icon;

                break;
            default:
                Debug.LogWarning("Non-Supported Item type passed into EquipItem");
                break;
        }
    }

    //////////////////////////////////////////////////
    public void InteractPressed()
    {
        //Farmland
        if (Vector2.Distance(_mousePosAtClick, transform.position) < 1.75f)
        {
            RaycastHit2D hit = Physics2D.Raycast(_mousePosAtClick, Vector2.zero, 10.0f, _farmableLayer);
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

    //////////////////////////////////////////////////
    public void InspectPressed()
    {
        _mousePosAtClick = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (_nearbyInteractables.Count == 0) return;

        foreach(InteractableVolume interactable in _nearbyInteractables)
        {
            if (!interactable) continue;

            RaycastHit2D hit = Physics2D.Raycast(_mousePosAtClick, Vector2.zero, 10.0f, _interactableLayer);
            if (hit.collider != null)
            {
                InteractableVolume iv = hit.transform.GetComponentInChildren<InteractableVolume>();
                if(iv && iv == interactable)
                {
                    IInteractable interaction = hit.transform.GetComponent<IInteractable>();
                    interaction?.OnUse(this);
                    break;
                }
            }
        }
    }

    //////////////////////////////////////////////////
    public void UseEquipped()
    {
        if (_playerWeapon.IsAttacking || _activeTool.UsingTool || !_currentItem)
        {
            return;
        }

        _mousePosAtClick = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        _lookDirection = GameUtilities.CalculateDirection(gameObject.transform.position, _mousePosAtClick);
        Vector2 directionVec = GameUtilities.GetDirectionVector(_lookDirection);

        if (_currentItem.itemType == ItemType.WEAPON)
        {
            //Swords can always swing
            if (!_playerWeapon.IsRanged)
            {
                float angle = Mathf.Atan2(directionVec.y, directionVec.x) * Mathf.Rad2Deg;
                Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                _weaponAttach.eulerAngles = rotation.eulerAngles;

                _animator.SetFloat("CombatHorizontal", directionVec.x);
                _animator.SetFloat("CombatVertical", directionVec.y);
                _animator.SetFloat("LastHorizontal", directionVec.x);
                _animator.SetFloat("LastVertical", directionVec.y);
                _playerWeapon.StartSwing(null, _bIsHeavyAttack);
            }
            //Bows can only rotate/aim when in the acceptable aim direction
            else if(_playerWeapon.IsRanged && _bIsInValidAimingDir)
            {
                _animator.SetFloat("CombatHorizontal", directionVec.x);
                _animator.SetFloat("CombatVertical", directionVec.y);
                _animator.SetFloat("LastHorizontal", directionVec.x);
                _animator.SetFloat("LastVertical", directionVec.y);
                _playerWeapon.StartSwing(null, false);
            }

        }
        else if(_currentItem.itemType == ItemType.TOOL)
        {
            _animator.SetFloat("CombatHorizontal", directionVec.x);
            _animator.SetFloat("CombatVertical", directionVec.y);
            _animator.SetFloat("LastHorizontal", directionVec.x);
            _animator.SetFloat("LastVertical", directionVec.y);

            if (_activeTool.HasTool)
            {
                _activeTool.UseTool(_mousePosAtClick);
                _characterEnergy.UseEnergy(_activeTool.EnergyConsumption);
            }
        }

    }

    //////////////////////////////////////////////////
    public void SetInventoryUIVisibility(bool visibility)
    {
        _uiManager.SetInventoryUIVisibilty(visibility);
        _dayManager.IsTimePaused = visibility;

        if(visibility)
        {
            _inputLayerManager.AddLayer(_inventoryInputLayer);
        }
        else
        {
            if (_inputLayerManager.IsLayerInFront(_inventoryInputLayer))
            {
                _inputLayerManager.PopLayer();
            }
        }
    }

    //////////////////////////////////////////////////
    public void SetChestUIVisibility(bool visibility) 
    {
        _uiManager.SetChestUIVisibility(visibility);

        if (visibility)
        {
            _inputLayerManager.AddLayer(_chestInputLayer);
        }
        else
        {
            if(_inputLayerManager.IsLayerInFront(_chestInputLayer))
            {
                _inputLayerManager.PopLayer();
            }
        }
    }

    //////////////////////////////////////////////////
    public void SetWarehouseUIVisibility(bool visibility, Warehouse warehouse)
    {
        _uiManager.SetWarehouseUIVisibility(visibility, warehouse);

        if(visibility)
        {
            _inputLayerManager.AddLayer(_warehouseInputLayer);
        }
        else
        {
            if(_inputLayerManager.IsLayerInFront(_warehouseInputLayer))
            {
                _inputLayerManager.PopLayer();
            }
        }
    }

    //////////////////////////////////////////////////
    public void UseInteractable()
    {

    }

    //////////////////////////////////////////////////
    public void AddInteractable(InteractableVolume interactable)
    {
        if (!_nearbyInteractables.Contains(interactable))
        {
            _nearbyInteractables.Add(interactable);
        }
    }

    //////////////////////////////////////////////////
    public void RemoveInteractable(InteractableVolume interactable)
    {
        if(_nearbyInteractables.Contains(interactable))
        {
            _nearbyInteractables.Remove(interactable);
        }
    }
}
