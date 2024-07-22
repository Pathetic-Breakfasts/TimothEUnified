using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class BuildModeController : MonoBehaviour
{
    
    bool _bEnabled = false;

    Tilemap[] _tilemaps;

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
            for(int x = originCellPosition.x; x < size.x; x++)
            {
                for(int y = originCellPosition.y; y < size.y; y++)
                {
                    Vector3Int pos = new Vector3Int(x, y, originCellPosition.z);
                    cellPositions.Add(pos);
                }
            }

            for(int i = 0; i < cellPositions.Count; i++)
            {
                Vector3 pos = tilemap.CellToWorld(cellPositions[i]);
                RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, 10.0f);
                if(hit.collider !=  null)
                {
                    canPlace = false;
                    break;
                }
            }
        }

        BuildModeUI ui = FindObjectOfType<BuildModeUI>(); 
        ui.SetBoxPosition(_tilemaps[0].WorldToCell(mouseWorldSpace), size, canPlace);

        Debug.Log("Can Place: " + canPlace);

    }
}
