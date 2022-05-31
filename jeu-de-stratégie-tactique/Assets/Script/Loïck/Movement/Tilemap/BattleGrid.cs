using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using Random = UnityEngine.Random;

public class BattleGrid : MonoBehaviour
{
    public static BattleGrid instance;
    private Vector2 originPosition;
    private int[,] gridArray;
    private TextMesh[,] debugTextArray;
    private List<GameObject> tiles;
    private float cellSize = 1;
    private int fontSize;
    private bool canCreateGrid = true;
    public GridLoader loader;
    public int width = 4;
    public int height = 4;
    public Tile.TileType tileType;
    [HideInEditorMode] public GameObject currentTilesRef;
    [ReadOnly]
    private List<GameObject> tilesRender;

    #region Grid Init
    #region Editor Function
    [MenuItem("GameObject/Cassoulet Objects/Grid Editor")]
    static void InstanceGridEditor()
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


    [HorizontalGroup("Split/Right")]
    [Button("Delete Battle Grid", ButtonSizes.Large), GUIColor(1, 0, 0, 1)]
    public void DeleteBattleGrid()
    {
        UpdateGridList();
        if (tiles != null)
        {
            for (int i = 0; i < tiles.Count; i++)
            {
                DestroyImmediate(tiles[i]);
            }
            tiles.Clear();
            canCreateGrid = true;
        }

        if (tilesRender != null)
        {
            tilesRender.Clear();
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

    void Start()
    {
        instance = this;
        BuildBattleGrid();
        if (tiles != null)
        {
            canCreateGrid = false;
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (tileType < Enum.GetValues(typeof(Tile.TileType)).Cast<Tile.TileType>().Last())
            {
                tileType++;
            }
            SetValue(GetMouseWorldPosition(), (int)tileType);
            SetObjectToGrid(GetMouseWorldPosition(), currentTilesRef);
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (tileType > 0)
            {
                tileType--;
            }
            SetValue(GetMouseWorldPosition(), (int)tileType);
            DeleteObjectToGrid(GetMouseWorldPosition());
        }
    }
    #endregion


    #region Runtime Function
    public List<Tile> GetValidTiles()
    {
        List<Tile> tiles = new List<Tile>();
        for (int i = 0; i < transform.childCount; i++)
        {
            Tile currentiles = transform.GetChild(i).GetComponent(typeof(Tile)) as Tile;
            if (currentiles != null && currentiles.Walkable)
                tiles.Add(currentiles);
        }

        return tiles;
    }

    private void UpdateGridList()
    {
        tiles = AllGridChild();
        if (loader != null)
        {
            gridArray = loader.gridArray;
            debugTextArray = loader.debugTextArray;
            height = loader.height;
            width = loader.width;
        }
        else if (gridArray != null)
        {
            for (int x = 0; x < gridArray.GetLength(0); x++)
            {
                for (int y = 0; y < gridArray.GetLength(1); y++)
                {
                    debugTextArray[x, y] = transform.GetChild(y + x).GetComponent<TextMesh>();
                }
            }
        }
    }

    public Tile SpawnRandomUnit()
    {
        int index = Random.Range(0, GetValidTiles().Count);
        return GetValidTiles()[index];
    }

    public Tile GetTile(int x, int y)
    {
        if (OntheGrid(x, y))
        {
            if (debugTextArray[x, y] != null)
            {
                return debugTextArray[x, y].GetComponent<Tile>();

            }
        }
        Debug.LogError("Erreur sortie de Grille");
        return null;
    }


    public bool OntheGrid(int x, int y)
    {
        bool onGrid = x >= 0 && y >= 0 && x <= width - 1 && y <= height - 1;

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
                debugTextArray[x, y] = CreateText(null, gridArray[x, y].ToString(), GetWorldPosition(x, y) + new Vector2(cellSize, cellSize) * .5f, fontSize);
                Tile debugTiles = debugTextArray[x, y].GetComponent<Tile>();
                debugTiles.tileXPos = x;
                debugTiles.tileYPos = y;
                debugTextArray[x, y].transform.SetParent(transform);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 1f);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 1f);
            }
        }
        Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 1f);
        Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 1f);
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
            Tile currentTile = debugTextArray[x, y].gameObject.GetComponent<Tile>();
            if (currentTile.OccupiedUnit == null)
            {
                currentTile.currentTileType = tileType;
                currentTile.CheckIfCanWalk();
                gridArray[x, y] = value;
                debugTextArray[x, y].text = gridArray[x, y].ToString();
            }
            else
            {
                UnitManager.Instance.SelectedHero = currentTile.OccupiedUnit;
            }
        }
    }

    public void SetValue(Vector2 worldPosition, int value)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        SetValue(x, y, value);
    }

    public void SetObjectToGrid(Vector2 worldPosition, GameObject obj)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            bool isdoublon = false;
            if (tilesRender != null)
            {
                for (int i = 0; i < tilesRender.Count; i++)
                {
                    if (tilesRender[i].transform.position == debugTextArray[x, y].transform.position)
                    {
                        isdoublon = true;
                    }
                }
            }
            else
            {
                tilesRender = new List<GameObject>();
            }
            if (!isdoublon)
            {
                GameObject instanceObj = Instantiate(obj);
                instanceObj.transform.position = debugTextArray[x, y].transform.position;
                instanceObj.transform.SetParent(transform);
                tilesRender.Add(instanceObj);
            }
        }
    }

    public void DeleteObjectToGrid(Vector2 worldPosition)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            if (this.tilesRender != null)
            {
                for (int i = 0; i < tilesRender.Count; i++)
                {
                    if (tilesRender[i].transform.position == debugTextArray[x, y].transform.position)
                    {
                        Destroy(tilesRender[i]);
                        tilesRender.Remove(tilesRender[i]);
                    }
                }
            }
        }
    }

    public List<GameObject> AllGridChild()
    {
        List<GameObject> allChild = new List<GameObject>();
        for (int i = 0; i < transform.childCount; i++)
        {
            allChild.Add(transform.GetChild(i).gameObject);
        }

        return allChild;
    }


    private void GetXY(Vector2 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
        y = Mathf.FloorToInt((worldPosition - originPosition).y / cellSize);
    }

    private Vector2 GetWorldPosition(int x, int y)
    {
        return new Vector2(x, y) * cellSize + originPosition;
    }

    public static TextMesh CreateText(Transform parent, string text, Vector2 localPosition, int fontSize)
    {
        GameObject textObj = new GameObject("World_Text", typeof(TextMesh), typeof(Tile));
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
