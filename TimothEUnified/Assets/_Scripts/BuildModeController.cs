using Pathfinding;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildModeController : MonoBehaviour
{
    bool _bEnabled = false;

    Tilemap[] _tilemaps;

    [SerializeField] Tilemap _placementTilemap;

    public StructureConfig CurrentConfig { set => _currentConfig = value; }
    StructureConfig _currentConfig = null;

    //////////////////////////////////////////////////
    private void Awake()
    {
        _tilemaps = FindObjectsOfType<Tilemap>();
    }

    //////////////////////////////////////////////////
    public void IsEnabled(bool enabled)
    {
        _bEnabled = enabled;
    }

    //////////////////////////////////////////////////
    void Update()
    {
        if (!_bEnabled || !_currentConfig) return;

        Vector2 mouseWorldSpace = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        bool canPlace = true;

        for(int x = 0; x < _currentConfig.structureSize.x; ++x)
        {
            for(int y = 0; y < _currentConfig.structureSize.y; ++y)
            {
                Vector3 newSamplePos = new Vector3(mouseWorldSpace.x + x, mouseWorldSpace.y + y, 0.0f);

                GraphNode node = AstarPath.active.GetNearest(newSamplePos).node;
                if (node != null && !node.Walkable)
                {
                    canPlace = false;
                    break;
                }

                RaycastHit2D hit = Physics2D.Raycast(newSamplePos, Vector2.zero, 10.0f);
                if(hit.collider != null)
                {
                    canPlace = false;
                    break;
                }
            }
        }

        BuildModeUI ui = FindObjectOfType<BuildModeUI>();
        Vector3Int tilePos = _tilemaps[0].WorldToCell(mouseWorldSpace);
        ui.SetBoxPosition(tilePos, _currentConfig.structureSize, canPlace);
        
        if(Input.GetMouseButtonDown(0))
        {
            if(canPlace)
            {
                _placementTilemap.SetTile(tilePos, _currentConfig.tilemapTile);
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            ui.SelectedConfig = null;
        }
    }
}
