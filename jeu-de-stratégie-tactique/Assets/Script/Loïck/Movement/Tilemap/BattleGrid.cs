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
    List<Vector3Int> availablePlaces = new List<Vector3Int>();

    [LabelText("/n")]
    public GridLoader loader;
    public BattleGridTile.TileType tileType;

    #region Grid Init
    #region Editor Function
    [MenuItem("GameObject/Cassoulet Objects/Grid Editor")]
    public static void InstanceGridEditor()
    {
        GameObject instanceGridEditor = new GameObject("Grid Editor", typeof(BattleGrid));
        instanceGridEditor.tag = "Grid";
    }

    //[Button("Save Map", ButtonSizes.Large), GUIColor(1, 0, 1)]
    //public void SaveTilemap()
    //{
    //    GameObject saveMap = new GameObject("New Map");
    //    GameObject parentG = Instantiate(gameObject, saveMap.transform);
    //    parentG.name = "Grid";
    //    parentG.transform.SetParent(saveMap.transform);
    //    string localPath = "Assets/Prefabs/Grid Map/" + saveMap.name + ".prefab";
    //    GridLoader saveTilemap = new GridLoader(gridArray, debugTextArray, height, width);
    //    PrefabUtility.SaveAsPrefabAsset(saveMap, localPath);
    //    AssetDatabase.CreateAsset(saveTilemap, "Assets/Database/Map/NewMap" + ".asset");
    //    AssetDatabase.SaveAssets();
    //    Destroy(saveMap);
    //}
    #endregion

    #region Unity Function

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
                Vector3Int place = new Vector3Int((int)_tilemap.CellToWorld(localPlace).x, (int)_tilemap.CellToWorld(localPlace).y);
                if (_tilemap.HasTile(localPlace))
                {
                    //Tile at "place"
                    availablePlaces.Add(place);
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
        int randomIndex = Random.Range(0, availablePlaces.Count);
        Vector3Int unitPos = availablePlaces[randomIndex];
        Player player = _gameManager.PlayerManager.CurrentPlayer;
        if (CheckIfUnitIsHere(player, unitPos.x, unitPos.y))
        {
            if (randomIndex == availablePlaces.Count)
            {
                unitPos = availablePlaces[Random.Range(0, randomIndex--)];
            }
            else
            {
                unitPos = availablePlaces[Random.Range(randomIndex++, availablePlaces.Count)];
            }
        }
        return unitPos;
    }

    public BattleGridTile GetTile(int x, int y)
    {
        if (OntheGrid(x, y))
        {
            return GetTile(x, y);
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

    public bool OntheGrid(int x, int y)
    {
        Vector3Int gridPos = new Vector3Int(x, y);
        bool onGrid = _tilemap.HasTile(gridPos);
        return onGrid;
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
    #endregion
}