using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "Tile", menuName = "Tile/BattleGrid")]
public class BattleGridTile : Tile
{
    [SerializeField] private bool _isWalkable;
    public TileType currentTileType = TileType.None;
    [Range(0,99)]public int mvRequire = 0;
    public bool Walkable => _isWalkable && currentTileType != TileType.None;

    public void CheckIfCanWalk()
    {
        switch (currentTileType)
        {
            case TileType.Walkable:
                _isWalkable = true;
                break;
            case TileType.None:
                _isWalkable = false;
                break;
        }
    }

    public void SetUnit(Character unit)
    {
        //if (unit.OccupiedTileGridPosition != null) unit.OccupiedTileGridPosition.OccupiedUnit = null;
        //unit.transform.position = transform.position;
        //OccupiedUnit = unit;
        //unit.OccupiedTileGridPosition = this;
    }

    public enum TileType
    {
        None, Walkable,Spawn
    }
}
