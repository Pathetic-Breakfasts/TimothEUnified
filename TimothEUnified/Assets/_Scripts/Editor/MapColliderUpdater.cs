using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;

namespace TimothE.Utility.Editors
{
    //////////////////////////////////////////////////
    [CustomEditor(typeof(PolygonCollider2D))]
    public class MapColliderUpdater : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            PolygonCollider2D polygonCollider2D = (PolygonCollider2D)target;
            if (polygonCollider2D == null || !polygonCollider2D.gameObject.CompareTag(GameTagManager._mapColliderTag))
            {
                return;
            }

            if (GUILayout.Button("Calculate Bounds (Background Tilemap)"))
            {
                GameObject map = GameObject.Find("Tilemap_Bg");
                if (map != null)
                {
                    Tilemap tilemap = map.GetComponent<Tilemap>();

                    tilemap.CompressBounds();
                    Bounds b = tilemap.localBounds;

                    //Offsets the bounds by one tile inwards
                    Vector3 cellSize = tilemap.cellSize;

                    Vector2[] points = new Vector2[4];
                    points[0] = new Vector2(b.min.x + cellSize.x, b.max.y - cellSize.y);
                    points[1] = new Vector2(b.max.x - cellSize.x, b.max.y - cellSize.y);
                    points[2] = new Vector2(b.max.x - cellSize.x, b.min.y + cellSize.y);
                    points[3] = new Vector2(b.min.x + cellSize.x, b.min.y + cellSize.y);

                    polygonCollider2D.points = points;
                }
                else
                {
                    Debug.Log("Cannot find Tilemap_Bg");
                }
            }
        }
    }
}
