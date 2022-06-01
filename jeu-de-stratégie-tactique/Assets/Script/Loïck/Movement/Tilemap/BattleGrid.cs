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
    [SerializeField] private GameObject _currentTilesRef;
    [SerializeField] private Tilemap _tilemap;
    [SerializeField] private List<Vector3Int> _availablePlaces = new List<Vector3Int>();

    //public List<Vector3Int> AvailablePlaces
    //{
    //    get => AvailablePlaces;
    //}

    public Tilemap Tilemap
    {
        get => _tilemap;
    }


    [LabelText("/n")]
    public GridLoader loader;
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
                Vector3Int place = new Vector3Int((int)_tilemap.GetCellCenterWorld(localPlace).x, (int)_tilemap.GetCellCenterWorld(localPlace).y);
                if (_tilemap.HasTile(localPlace))
                {
                    //Tile at "place"
                    _availablePlaces.Add(place);
                }
                else
                {
                    //No tile at "place"
                }
            }
        }
    }

    public Vector3Int SpawnRandomUnit()
    {
        int randomIndex = Random.Range(0, _availablePlaces.Count);
        Vector3Int unitPos = _availablePlaces[randomIndex];
        Player player = _gameManager.PlayerManager.CurrentPlayer;
        if (CheckIfUnitIsHere(player, unitPos.x, unitPos.y))
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

    public BattleGridTile GetTileType(int x, int y)
    {
        if (OntheGrid(x, y))
        {
            return GetTileType(x, y);
        }
        Debug.LogError("Erreur sortie de Grille");
        return null;
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

    public Vector3Int GetTilePosition(int x, int y)
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

    public bool OntheGrid(int x, int y)
    {
        for (int i = 0; i < _availablePlaces.Count; i++)
        {

            if (_availablePlaces[i].x == x && _availablePlaces[i].y == y)
            {
                return true;
            }

        }
        return false;
    }

    public int GetTileRange(Vector3Int unitPos,Vector3Int gridPos)
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

}