using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "Tile", menuName = "Tile/BattleGrid")]
public class BattleGridTile : Tile
{
    public TileType currentTileType = TileType.None;
    [Range(0,99)]public int mvRequire = 0;
    public bool Walkable => currentTileType == TileType.Walkable;

    public enum TileType
    {
        None = 0,
        Walkable = 1,
        Spawn = 2,
        Ruin = 3,
        MotherBase = 4
    }
}
