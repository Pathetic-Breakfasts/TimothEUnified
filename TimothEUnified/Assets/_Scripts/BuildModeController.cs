using Pathfinding;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildModeController : MonoBehaviour
{
    public bool IsEnabled { get => _bEnabled; set => _bEnabled = value; }
    bool _bEnabled = false;

    public bool CanPlace { get => _bCanPlace; }
    bool _bCanPlace = false;

    Tilemap[] _tilemaps;

    [SerializeField] Tilemap _placementTilemap;

    public StructureConfig CurrentConfig { set => _currentConfig = value; }
    StructureConfig _currentConfig = null;

    public Vector3Int CurrentTilePosition { get => _currentTilePosition; }
    Vector3Int _currentTilePosition;


    CurrencyStore _currencyStore;
    WarehouseManager _warehouseManager;

    //////////////////////////////////////////////////
    private void Awake()
    {
        _tilemaps = FindObjectsOfType<Tilemap>();
        _currencyStore = FindObjectOfType<CurrencyStore>();
        _warehouseManager = FindObjectOfType<WarehouseManager>();
    }

    //////////////////////////////////////////////////
    void FixedUpdate()
    {
        if (!_bEnabled || !_currentConfig) return;

        Vector2 mouseWorldSpace = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        _bCanPlace = true;

        for(int x = 0; x < _currentConfig.structureSize.x; ++x)
        {
            for(int y = 0; y < _currentConfig.structureSize.y; ++y)
            {
                Vector3 newSamplePos = new Vector3(mouseWorldSpace.x + x, mouseWorldSpace.y + y, 0.0f);

                GraphNode node = AstarPath.active.GetNearest(newSamplePos).node;
                if (node != null && !node.Walkable)
                {
                    _bCanPlace = false;
                    break;
                }

                RaycastHit2D hit = Physics2D.Raycast(newSamplePos, Vector2.zero, 10.0f);
                if(hit.collider != null)
                {
                    _bCanPlace = false;
                    break;
                }
            }
        }

        _currentTilePosition = _tilemaps[0].WorldToCell(mouseWorldSpace);
    }

    //////////////////////////////////////////////////
    public void PlaceStructure()
    {
        if (!_bCanPlace)
        {
            return; //TODO: Place SFX/VFX here?
        };

        if (!CanAfford(_currentConfig))
        {
            return;
        }

        _currencyStore.SpendMoney(_currentConfig.goldCost);

        if(_currentConfig.resourceCost.Count > 0)
        {
            foreach(ResourceCost cost in _currentConfig.resourceCost)
            {
                _warehouseManager.RemoveResource(cost.item, cost.quantity);
            }
        }

        _placementTilemap.SetTile(_currentTilePosition, _currentConfig.tilemapTile);
    }

    public bool CanAfford(StructureConfig config)
    {
        if (!config) return false;

        if (!_currencyStore.CanAfford(config.goldCost))
        {
            return false;
        }

        if(_currentConfig.resourceCost.Count > 0)
        {
            foreach(ResourceCost cost in  _currentConfig.resourceCost)
            {
                if (!_warehouseManager.HasResource(cost.item, cost.quantity))
                {
                    return false;
                }
            }
        }

        return true;
    }
}
