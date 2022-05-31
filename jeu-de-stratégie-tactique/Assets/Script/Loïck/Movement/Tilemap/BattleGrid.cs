using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Sirenix.OdinInspector;
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
    [SerializeField] private Grid _grid;
    [SerializeField] private Tilemap _tilemap;

    [LabelText("/n")]
    private Vector2 originPosition;
    private int[,] gridArray;
    private TextMesh[,] debugTextArray;
    private List<TileBase> tiles;
    private float cellSize = 1;
    private int fontSize;
    private bool canCreateGrid = true;
    public GridLoader loader;
    public int width = 4;
    public int height = 4;
    public BattleGridTile.TileType tileType;
    [ReadOnly]
    private List<GameObject> tilesRender;

    #region Grid Init
    #region Editor Function
    [MenuItem("GameObject/Cassoulet Objects/Grid Editor")]
    public static void InstanceGridEditor()
    {
        GameObject instanceGridEditor = new GameObject("Grid Editor", typeof(BattleGrid));
        instanceGridEditor.tag = "Grid";
    }

    [HorizontalGroup("Split")]
    [Button("Build Battle Grid", ButtonSizes.Large), GUIColor(0, 1, 0)]
    public void BuildBattleGrid()
    {
        if (canCreateGrid)
        {
            SetBattleGrid();
            canCreateGrid = false;
            //if (loader == null)
            //{
            //    gridArray = new int[width, height];
            //    debugTextArray = new TextMesh[width, height];
            //}
            //UpdateGridList();
        }
    }

    [Button("Save Map", ButtonSizes.Large), GUIColor(1, 0, 1)]
    public void SaveTilemap()
    {
        GameObject saveMap = new GameObject("New Map");
        GameObject parentG = Instantiate(gameObject, saveMap.transform);
        parentG.name = "Grid";
        parentG.transform.SetParent(saveMap.transform);
        string localPath = "Assets/Prefabs/Grid Map/" + saveMap.name + ".prefab";
        GridLoader saveTilemap = new GridLoader(gridArray, debugTextArray, height, width);
        PrefabUtility.SaveAsPrefabAsset(saveMap, localPath);
        AssetDatabase.CreateAsset(saveTilemap, "Assets/Database/Map/NewMap" + ".asset");
        AssetDatabase.SaveAssets();
        Destroy(saveMap);
    }
    #endregion

    #region Unity Function



    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (tileType < Enum.GetValues(typeof(BattleGridTile.TileType)).Cast<BattleGridTile.TileType>().Last())
            {
                tileType++;
            }
            SetValue(GetMouseWorldPosition(), (int)tileType);
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (tileType > 0)
            {
                tileType--;
            }
        }
    }
    #endregion


    #region Runtime Function
    public void Init(GameManager gm)
    {
        _gameManager = gm;
        BuildBattleGrid();
        tiles = _tilemap.GetTilesBlock(_tilemap.cellBounds).ToList();
    }


    private void UpdateGridList()
    {

    }

    public Vector2Int SpawnRandomUnit()
    {
        int index = Random.Range(0, tiles.Count);
        return _tilemap.GetTile((BattleGridTile)tiles[index].);
    }

    public BattleGridTile GetTile(int x, int y)
    {
        if (OntheGrid(x,y))
        {
            return GetTile(x, y);
        }
        Debug.LogError("Erreur sortie de Grille");
        return null;
    }


    public bool OntheGrid(int x, int y)
    {
        bool onGrid = _tilemap.GetTile(new Vector3Int(x,y)) != null;

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

    public void SetBattleGrid()
    {
        gridArray = new int[width, height];
        debugTextArray = new TextMesh[width, height];
        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                debugTextArray[x, y] = CreateText(null, gridArray[x, y].ToString(), _grid.CellToWorld( new Vector3Int(x, y)), fontSize);
                debugTextArray[x, y].transform.SetParent(transform);
            }
        }
    }

    public int GetValue(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            return gridArray[x, y];
        }
        return 0;
    }

    public void SetValue(int x, int y, int value)
    {
        if (gridArray == null || debugTextArray == null)
        {
            //gridArray = loader.gridArray;
            //debugTextArray = loader.debugTextArray;
            //height = loader.height;
            //width = loader.width;
            //gridArray = new int[width, height];
            //debugTextArray = new TextMesh[width, height];
            //for (int a = 0; a < gridArray.GetLength(0); a++)
            //{
            //    for (int b = 0; b < gridArray.GetLength(1); b++)
            //    {
            //        TextMesh debugTextmesh =  transform.GetChild(a + b).transform.GetComponent<TextMesh>();
            //        if (debugTextmesh != null)
            //        {
            //            debugTextArray[a, b] = debugTextmesh;
            //        }
            //    }
            //}
        }
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            BattleGridTile currentBattleGridTile = debugTextArray[x, y].gameObject.GetComponent<BattleGridTile>();
            Character OccupiedUnit = _gameManager.PlayerManager.CurrentPlayer.GetCharacter(x, y);
            if (OccupiedUnit == null)
            {
                currentBattleGridTile.currentTileType = tileType;
                currentBattleGridTile.CheckIfCanWalk();
                gridArray[x, y] = value;
                debugTextArray[x, y].text = gridArray[x, y].ToString();
            }
            else
            {
                _gameManager.UnitManager.SelectedHero = OccupiedUnit;
            }
        }
    }

    public void SetValue(Vector2 worldPosition, int value)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        SetValue(x, y, value);
    }

    private void GetXY(Vector2 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
        y = Mathf.FloorToInt((worldPosition - originPosition).y / cellSize);
    }

    private void GetXY(Vector2 worldPosition, out float x, out float y)
    {
        x = Mathf.Round((worldPosition - originPosition).x / cellSize);
        y = Mathf.Round((worldPosition - originPosition).y / cellSize);
    }

    private Vector2 GetWorldPosition(float x, float y)
    {
        return new Vector2(x, y) * cellSize + originPosition;
    }

    public static TextMesh CreateText(Transform parent, string text, Vector2 localPosition, int fontSize)
    {
        GameObject textObj = new GameObject("World_Text", typeof(TextMesh), typeof(BattleGridTile));
        textObj.layer = 6;
        Transform localTransform = textObj.transform;
        localTransform.SetParent(parent, false);
        localTransform.localPosition = localPosition;
        TextMesh textMesh = textObj.GetComponent<TextMesh>();
        textMesh.fontSize = fontSize;
        textMesh.text = text;
        textMesh.color = Color.blue;
        textMesh.anchor = TextAnchor.MiddleCenter;
        textMesh.alignment = TextAlignment.Left;
        textMesh.GetComponent<MeshRenderer>();
        return textMesh;
    }

}
