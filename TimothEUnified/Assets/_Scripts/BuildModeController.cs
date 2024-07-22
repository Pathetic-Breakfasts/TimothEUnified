using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class BuildModeController : MonoBehaviour
{
    
    bool _bEnabled = false;

    Tilemap[] _tilemaps;

    [SerializeField] Tilemap _placementTilemap;

    [SerializeField] StructureConfig _config;

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
        if (!_bEnabled) return;

        //TODO: Hook this into the current structure config
        Vector2 size = new Vector2(10.0f, 9.0f);

        Vector2 mouseWorldSpace = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        bool canPlace = true;
        foreach (Tilemap tilemap in _tilemaps)
        {
            Vector3Int originCellPosition = tilemap.WorldToCell(mouseWorldSpace);

            List<Vector3Int> cellPositions = new List<Vector3Int>();
            for(int x = 0; x < size.x; x++)
            {
                for(int y = 0; y < size.y; y++)
                {
                    Vector3Int pos = new Vector3Int(originCellPosition.x + x, originCellPosition.y + y, originCellPosition.z);
                    cellPositions.Add(pos);
                }
            }

            for(int i = 0; i < cellPositions.Count; i++)
            {
                Vector3 pos = tilemap.CellToWorld(cellPositions[i]);
                RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, 10.0f);
                if(hit.collider !=  null)
                {
                    Debug.Log(hit.collider.name);
                    canPlace = false;
                    break;
                }
            }
        }

        BuildModeUI ui = FindObjectOfType<BuildModeUI>();
        Vector3Int tilePos = _tilemaps[0].WorldToCell(mouseWorldSpace);
        ui.SetBoxPosition(tilePos, size, canPlace);
        
        if(Input.GetMouseButtonDown(0))
        {
            if(canPlace)
            {
                _placementTilemap.SetTile(tilePos, _config.tilemapTile);
            }
        }

    }
}
