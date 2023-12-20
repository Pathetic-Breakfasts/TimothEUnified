using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameDevTV.Inventories;
using Unity.VisualScripting;
using GameDevTV.UI.Inventories;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Weapon _playerWeapon;
    ActiveTool _activeTool;

    [SerializeField] Transform _weaponAttach;

    [SerializeField] LayerMask _interactableLayer;

    [SerializeField] InventoryLoadout _startingLoadout;

    [SerializeField] GameObject _heldItemGO;


    [SerializeField] BodyPieceSpriteCollection _headSprites;
    [SerializeField] BodyPieceSpriteCollection _chestSprites;
    [SerializeField] BodyPieceSpriteCollection _armSprites;
    [SerializeField] BodyPieceSpriteCollection _legSprites;

    [SerializeField] InventoryItem _defaultHelmet;
    [SerializeField] InventoryItem _defaultChest;
    [SerializeField] InventoryItem _defaultArms;
    [SerializeField] InventoryItem _defaultLegs;

    CharacterEnergy _characterEnergy;
    Inventory _inventory;
    Mover _mover;
    DayManager _dayManager;
    Animator _animator;
    Health _playerHealth;

    InputLayerManager _inputLayerManager;

    ChestInputLayer _chestInputLayer;
    InventoryInputLayer _inventoryInputLayer;
    CurrencyStore _currencyStore;

    CharacterSpriteController _characterSpriteController;

    public UIManager GameUIManager { get => _uiManager; }
    UIManager _uiManager;


    Vector2 _movement;
    Vector2 _mousePosAtClick;
    Vector2 _mousePos;

    Vector3 _lastValidDir;
    bool _bIsInValidAimingDir = false;

    InventoryItem _currentItem;

    InteractDirection _lookDirection;
    public InteractDirection LookDirection { get => _lookDirection; }

    public Bed CurrentBed { get => _bed; set { _bed = value; UpdatePrompts(); } }
    Bed _bed;

    public DoorController NearbyDoor { get => _doorController; set { _doorController = value; UpdatePrompts(); } }
    DoorController _doorController;

    public Chest NearbyChest { get => _nearbyChest; set { _nearbyChest = value; UpdatePrompts(); } }
    Chest _nearbyChest;
    public bool IsHeavyAttack { get => _isHeavyAttack; set => _isHeavyAttack = value; }
    bool _isHeavyAttack = false;

    private void Awake()
    {
        _characterSpriteController = GetComponent<CharacterSpriteController>();
        _characterEnergy = GetComponent<CharacterEnergy>();
        _animator = GetComponent<Animator>();
        _activeTool = GetComponent<ActiveTool>();
        _mover = GetComponent<Mover>();
        _playerHealth = GetComponent<Health>();
        _inventory = GetComponent<Inventory>();
        _inputLayerManager = GetComponent<InputLayerManager>();
        _currencyStore = GetComponent<CurrencyStore>();

        _inventoryInputLayer = new InventoryInputLayer();
        _inventoryInputLayer.Initialize();

        _chestInputLayer = new ChestInputLayer();
        _chestInputLayer.Initialize();
    }

    private void Start()
    {
        _activeTool.ChangeTool(null);

        _dayManager = FindObjectOfType<DayManager>();
        _uiManager = FindObjectOfType<UIManager>();

        _uiManager.PlayerInventoryUI.DisplayedInventory = _inventory;

        if (_startingLoadout)
        {
            _startingLoadout.SpawnItems(_inventory);
        }

        _characterSpriteController.SetSpriteSet(ArmorType.Head, _headSprites);
        _characterSpriteController.SetSpriteSet(ArmorType.Chest, _chestSprites);
        _characterSpriteController.SetSpriteSet(ArmorType.Arms, _armSprites);
        _characterSpriteController.SetSpriteSet(ArmorType.Legs, _legSprites);

        if (_defaultHelmet)
        {
            _characterSpriteController.SetOverlaySpriteSet(ArmorType.Head, _defaultHelmet.armorSprites);
        }

        _characterSpriteController.SetOverlaySpriteSet(ArmorType.Chest, _defaultChest.armorSprites);
        _characterSpriteController.SetOverlaySpriteSet(ArmorType.Arms, _defaultArms.armorSprites);
        _characterSpriteController.SetOverlaySpriteSet(ArmorType.Legs, _defaultLegs.armorSprites);

        _currencyStore.GainMoney(500);
    }

    private void FixedUpdate()
    {
        if (_movement == Vector2.zero) return;

        _mover.Move(_movement);

        Vector3 adjustedPos = transform.position + new Vector3(_movement.x, _movement.y);
        _lookDirection = GameUtilities.CalculateDirection(transform.position, adjustedPos);

        _lastValidDir = GameUtilities.GetDirectionVector(_lookDirection);

        _animator.SetFloat("LastHorizontal", _movement.x);
        _animator.SetFloat("LastVertical", _movement.y);
    }

    private void Update()
    {
        if (Debug.isDebugBuild)
        {
            DebugUpdate();
        }

        _characterSpriteController.UpdateSprite();


        _mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (_activeTool.UsingTool || _playerWeapon.IsAttacking)
        {
            _movement = Vector2.zero;
            _animator.SetFloat("Speed", 0.0f);
        }

        //Only set our aim position when using a ranged weapon within a -45/+45 degree angle of the direction we are facing
        if (!_playerWeapon.IsAttacking && _playerWeapon.HasWeapon && _playerWeapon.IsRanged)
        {
            Vector3 adjustedPos = _mousePos;
            Vector3 dirVec = GameUtilities.GetDirectionVector(_lookDirection);
            Vector3 dir = (adjustedPos - transform.position).normalized;

            //Inside our bounds
            if(Vector3.Dot(dirVec, dir) > 0.5f)
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

    }

    private void DebugUpdate()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            _dayManager.ProgressDay();
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            _characterSpriteController.SetOverlaySpriteSet(ArmorType.Head, new BodyPieceSpriteCollection(4));
        }
        if (Input.GetKey(KeyCode.H))
        {
            _characterSpriteController.SetOverlaySpriteSet(ArmorType.Head, _defaultHelmet.armorSprites);
        }
    }

    //Called through CurrencyStore OnCurrencyChanged event
    public void OnCurrencyUpdated()
    {
        _uiManager.SetCoinTextAmount(_currencyStore.CurrencyAmount);
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
                _playerWeapon.StartSwing(null, _isHeavyAttack);
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

    public void SetInventoryUIVisibility(bool visibility)
    {
        _uiManager.SetInventoryUIVisibilty(visibility);

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
        else if(_nearbyChest)
        {
            _uiManager.ChestInventoryUI.DisplayedInventory = _nearbyChest.ChestInventory;
            _uiManager.PlayerChestInventoryUI.DisplayedInventory = _inventory;
            SetChestUIVisibility(true);
        }
    }
}
