using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Sirenix.OdinInspector;
using TEAM2;
using UnityEditor;
using UnityEditor.Animations;
using UnityEditor.Graphs;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.Tilemaps;

public class BattleGrid : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private Tilemap _tilemap;
    [SerializeField] private List<Vector3> _availablePlaces = new List<Vector3>();

    //public List<Vector3Int> AvailablePlaces
    //{
    //    get => AvailablePlaces;
    //}

    public Tilemap Tilemap
    {
        get => _tilemap;
    }

    private UnitManager UnitManager
    {
        get => _gameManager.UnitManager;
    }

    public BattleGridTile.TileType tileType;

    #region Editor Function
    [MenuItem("GameObject/Cassoulet Objects/Grid Editor")]
    public static void InstanceGridEditor()
    {
        GameObject instanceGridEditor = new GameObject("Grid Editor", typeof(BattleGrid));
        instanceGridEditor.tag = "Grid";
    }
    #endregion


    #region Runtime Function
    public void Init(GameManager gm)
    {
        _gameManager = gm;
        for (int n = _tilemap.cellBounds.xMin; n < _tilemap.cellBounds.xMax; n++)
        {
            for (int p = _tilemap.cellBounds.yMin; p < _tilemap.cellBounds.yMax; p++)
            {
                Vector3Int localPlace = (new Vector3Int(n, p, (int)_tilemap.transform.position.y));
                Vector3 place = _tilemap.GetCellCenterWorld(localPlace);
                if (_tilemap.HasTile(localPlace))
                {
                    //Tile at "place"
                    _availablePlaces.Add(place);
                    BattleGridTile currentTile = (BattleGridTile)_tilemap.GetTile(localPlace);
                    if (currentTile.currentTileType == BattleGridTile.TileType.Ruin)
                    {
                        _gameManager.GridBuildingSystem.IntitializeWithBuilding(UnitManager.GetFactionScriptableUnits(Faction.Building)[0], localPlace);
                    }
                }
                else
                {
                    //No tile at "place"
                }
            }
        }
    }

    public Vector3 SpawnRandomUnit()
    {
        int randomIndex = Random.Range(0, _availablePlaces.Count);
        Vector3 unitPos = _availablePlaces[randomIndex];
        Player player = _gameManager.PlayerManager.CurrentPlayer;
        if (CheckIfUnitIsHere(player, (int)unitPos.x, (int)unitPos.y))
        {
            if (randomIndex == _availablePlaces.Count)
            {
                unitPos = _availablePlaces[Random.Range(0, randomIndex--)];
            }
            else
            {
                unitPos = _availablePlaces[Random.Range(randomIndex++, _availablePlaces.Count)];
            }
        }
        return unitPos;
    }

    public BattleGridTile GetTileType(Vector3Int tilePos)
    {
        return (BattleGridTile)_tilemap.GetTile(tilePos);
    }

    public bool CheckIfUnitIsHere(Player player, int x, int y)
    {
        bool ishere = false;
        for (int i = 0; i < player.Units.Count; i++)
        {

            if (player.Units[i].xPos == x && player.Units[i].yPos == y)
            {
                ishere = true;
            }

        }
        return ishere;
    }

    public Character GetPlayerUnit(Player player, int x, int y)
    {
        Character character = null;
        for (int i = 0; i < player.Units.Count; i++)
        {

            if (player.Units[i].xPos == x && player.Units[i].yPos == y)
            {
                character = player.Units[i] as Character;
            }

        }
        return character;
    }

    public Vector3 GetTilePosition(int x, int y)
    {
        for (int i = 0; i < _availablePlaces.Count; i++)
        {

            if (_availablePlaces[i].x == x && _availablePlaces[i].y == y)
            {
                return _availablePlaces[i];
            }

        }
        Debug.LogError("Emplacement introuvable");
        return Vector3Int.zero;
    }



    public int GetTileRange(Vector3Int unitPos, Vector3Int gridPos)
    {
        bool inRange = false;
        int numTiles = 0;
        for (int i = 0; i < _availablePlaces.Count; i++)
        {
            if (_availablePlaces[i] == unitPos || inRange)
            {
                inRange = true;
                numTiles++;
            }
            else if (_availablePlaces[i] == gridPos)
            {
                break;
            }
        }

        return numTiles;
    }

    #endregion
    public static Vector2 GetMouseWorldPosition()
    {
        Vector2 vec = GetMouseWorldPositionWithZ(Camera.main, Input.mousePosition);
        return vec;
    }
    public static Vector2 GetMouseWorldPositionWithZ(Camera worldCamera, Vector2 screenPosition)
    {
        Vector2 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
        return worldPosition;
    }

    void OnGUI()
    {
        Vector3 mousPos = GetMouseWorldPosition();
        Vector3Int intMousPos = new Vector3Int((int)mousPos.x, (int)mousPos.y);
        Vector3 centerMousePos = _tilemap.GetCellCenterWorld(intMousPos);
        GUI.Label(new Rect(10f, 10, 1000, 1000), "Mouse position : " + mousPos.x + " " + mousPos.y + " \n Center Mouse Position :" + centerMousePos.x + " " + centerMousePos.y);
    }
}