using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public class GridInit : MonoBehaviour
{
    public float cellSize = 1;
    public int fontsize = 1;
    private BattleGrid grid;
    public int width = 4;
    public int height = 4;
    public GridType gridType;
    private List<Transform> gridTransforms;
    private List<GameObject> tilesObjects = new List<GameObject>();
    public GameObject currentTilesRef;
    private GameObject parentGrid;
    private GameObject parentOfTiles;

    [HorizontalGroup("Split")]
    [Button("Build Battle Grid", ButtonSizes.Large), GUIColor(0, 1, 0)]
    public void BuildBattleGrid()
    {
        if (grid == null)
        {
            parentGrid = new GameObject("Grid");
            grid = parentGrid.AddComponent<BattleGrid>();
            grid =  grid.SetBattleGrid(width, height, cellSize, fontsize, transform.position, ref parentGrid,grid);
        }
    }

    void Start()
    {
        BuildBattleGrid();
    }
    [HorizontalGroup("Split/Right")]
    [Button("Delete Battle Grid", ButtonSizes.Large), GUIColor(1, 0, 0, 1)]
    public void DeleteBattleGrid()
    {
        DestroyImmediate(grid);
        DestroyImmediate(parentGrid);
        DestroyImmediate(parentOfTiles);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (gridType < Enum.GetValues(typeof(GridType)).Cast<GridType>().Last())
            {
                gridType++;
            }
            grid.SetValue(GetMouseWorldPosition(), (int)gridType);
            grid.SetObjectToGrid(GetMouseWorldPosition(), currentTilesRef, tilesObjects, ref parentOfTiles);
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (gridType > 0)
            {
                gridType--;
            }
            grid.SetValue(GetMouseWorldPosition(), (int)gridType);
            grid.DeleteObjectToGrid(GetMouseWorldPosition(), tilesObjects);
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
