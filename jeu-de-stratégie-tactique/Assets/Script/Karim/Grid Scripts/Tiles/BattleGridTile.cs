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

    public Color baseColor;

    public enum TileType
    {
        None = 0,
        Walkable = 1,
        Spawn = 2,
        Ruin = 3,
        MotherBase = 4
    }

    public override bool StartUp(Vector3Int location, ITilemap tilemap, GameObject go)
    {
        baseColor = color;
        flags = TileFlags.LockTransform;
        return true;
    }



   
}
