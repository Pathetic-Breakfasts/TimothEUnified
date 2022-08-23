using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public struct InfluenceTileData
{
    public Vector2 _tilePosition;
    public float _tileValue;
}

public class TileGrid : MonoBehaviour
{
    Tilemap _tilemap;

    List<InfluenceTileData> _influenceMap;

    // Start is called before the first frame update
    void Start()
    {
        Tilemap[] tms = FindObjectsOfType<Tilemap>();
        foreach (Tilemap tm in tms)
        {
            //Finds the correct tilemap 
            if (tm.gameObject.name == "Tilemap_Bg")
            {
                _tilemap = tm;
                break;
            }
        }

        if (_tilemap == null)
        {
            Debug.LogWarning("Tilemap_Bg was not found");
        }

        _influenceMap = new List<InfluenceTileData>();

        foreach (var pos in _tilemap.cellBounds.allPositionsWithin) 
        {
            if (_tilemap.HasTile(pos))
            {
                InfluenceTileData data;
                data._tilePosition = new Vector2(pos.x, pos.y);
                data._tileValue = 0.0f;

                _influenceMap.Add(data);

                continue;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (_influenceMap == null) return;

        Gizmos.color = Color.gray;

        Vector3 tileSize = new Vector3(0.5f, 0.5f, 0.5f);

        foreach (InfluenceTileData data in _influenceMap)
        {

            Vector3 tilePos = new Vector3();
            tilePos.x = data._tilePosition.x + 0.5f;
            tilePos.y = data._tilePosition.y + 0.5f;
            tilePos.z = 0.0f;

            Gizmos.DrawWireCube(tilePos, tileSize);
        }
        
    }


}
