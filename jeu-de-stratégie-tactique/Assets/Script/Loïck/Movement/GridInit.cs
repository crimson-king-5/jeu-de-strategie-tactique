using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

public class GridInit : MonoBehaviour
{
    public float cellSize = 1;
    public int fontsize = 1;
    [ShowInInspector] private BattleGrid grid;
    public int width = 4;
    public int height = 4;
    public GridType gridType;
    private List<Transform> gridTransforms;
    private List<GameObject> tilesObjects = new List<GameObject>();
    public GameObject currentTilesRef;
    private GameObject parentGrid;
    private GameObject parentOfTiles;

    void Start()
    {
        for (int i = 0; i < 2; i++)
        {
            DeleteBattleGrid();
            BuildBattleGrid();
        }
    }

    [HorizontalGroup("Split")]
    [Button("Build Battle Grid", ButtonSizes.Large), GUIColor(0, 1, 0)]
    public void BuildBattleGrid()
    {
        if (grid == null && GameObject.FindGameObjectWithTag("Grid") == null)
        {
            parentGrid = new GameObject("Grid");
            parentGrid.tag = "Grid";
            grid = parentGrid.AddComponent<BattleGrid>();
            grid = grid.SetBattleGrid(width, height, cellSize, fontsize, transform.position, ref parentGrid, grid);
        }
        else
        {
            grid = GameObject.FindGameObjectWithTag("Grid").GetComponent<BattleGrid>();
        }
    }
    [HorizontalGroup("Split/Right")]
    [Button("Delete Battle Grid", ButtonSizes.Large), GUIColor(1, 0, 0, 1)]
    public void DeleteBattleGrid()
    {
        if (grid != null && GameObject.FindGameObjectWithTag("Grid") != null)
        {
            DestroyImmediate(grid.gameObject);
            DestroyImmediate(parentGrid);
            DestroyImmediate(parentOfTiles);
            parentOfTiles = null;
        }
        else if (GameObject.FindGameObjectWithTag("Grid") != null)
        {
            grid = GameObject.FindGameObjectWithTag("Grid").GetComponent<BattleGrid>();
        }
    }

    [Button("Save Map", ButtonSizes.Large), GUIColor(1, 0, 1)]
    public void SaveTilemap()
    {
        if (grid != null && GameObject.FindGameObjectWithTag("Grid") != null)
        {
            GameObject saveMap = new GameObject("new Map");
            GameObject parentG =  Instantiate(parentGrid, saveMap.transform);
            GameObject parentT = Instantiate(parentOfTiles, saveMap.transform);
            parentT.name = "Parent Tiles";
            parentG.name = "Parent Grid";
            string localPath = "Assets/Prefabs/Grid Map/" + saveMap.name + ".prefab";

            PrefabUtility.SaveAsPrefabAsset(saveMap,localPath);
            Destroy(saveMap);
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (gridType < Enum.GetValues(typeof(GridType)).Cast<GridType>().Last())
            {
                gridType++;
            }

            if (grid == null)
            {
                DeleteBattleGrid();
                BuildBattleGrid();
            }

            if (grid != null)
            {
                grid.SetValue(GetMouseWorldPosition(), (int)gridType);
                grid.SetObjectToGrid(GetMouseWorldPosition(), currentTilesRef, tilesObjects, ref parentOfTiles);
            }
            // parentOfTiles.transform.SetParent(parentGrid.transform);
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (gridType > 0)
            {
                gridType--;
            }

            if (grid == null)
            {
                DeleteBattleGrid();
                BuildBattleGrid();
            }
            else
            {
                grid.SetValue(GetMouseWorldPosition(), (int)gridType);
                grid.DeleteObjectToGrid(GetMouseWorldPosition(), tilesObjects);
            }
        }
    }

    public enum GridType
    {
        Wakable, None
    }

    public static Vector2 GetMouseWorldPosition()
    {
        Vector2 vec = GetMouseWorldPositionWithZ(Camera.main, Input.mousePosition);
        return vec;
    }

    public static Vector2 GetMouseWorldPositionWithZ(Camera worldCamera, Vector2 screenPosition)
    {
        Vector2 worldPosiion = worldCamera.ScreenToWorldPoint(screenPosition);
        return worldPosiion;
    }
}
