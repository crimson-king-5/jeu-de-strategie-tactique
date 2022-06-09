using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "Tile", menuName = "Tile/BattleGrid")]
public class BattleGridTile : Tile
{
    public TileType currentTileType = TileType.None;
    [Range(0,99)]public int mvRequire = 0;
    public bool Walkable => currentTileType != TileType.None && currentTileType != TileType.Ruin;

    public void SetUnit(Character unit)
    {
        //if (unit.OccupiedTileGridPosition != null) unit.OccupiedTileGridPosition.OccupiedUnit = null;
        //unit.transform.position = transform.position;
        //OccupiedUnit = unit;
        //unit.OccupiedTileGridPosition = this;
    }

    public enum TileType
    {
        None = 0,
        Walkable = 1,
        Spawn = 2,
        Ruin = 3,
        MotherBase = 4
    }
}
