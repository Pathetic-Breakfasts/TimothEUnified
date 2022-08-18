using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Windows;

[CreateAssetMenu]
public class CustomRuleTile : RuleTile<CustomRuleTile.Neighbor>
{
    public bool desert;
    public bool tilled;

    public class Neighbor : RuleTile.TilingRule.Neighbor
    {
        public const int SameTerrain = 3;
        public const int DifferentTerrain = 4;
    }

    public override bool RuleMatch(int neighbor, TileBase tile)
    {
        var customRule = tile as CustomRuleTile;
        switch (neighbor)
        {
            //case Neighbor.SameTerrain:
            //    return customRule && this.desert == customRule.desert;
            //case Neighbor.DifferentTerrain:
            //    return customRule && this.desert != customRule.desert;



            case 2: return tile == null || !(tile is CustomRuleTile);
            case Neighbor.SameTerrain: return tile is CustomRuleTile;
        }

        return base.RuleMatch(neighbor, tile);
    }
}
