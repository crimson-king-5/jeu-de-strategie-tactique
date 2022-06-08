using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using TEAM2;

public class GridBuildingSystem : MonoBehaviour
{
    private GameManager _gameManager;
    public GridLayout GridLayout
    {
        get => _gameManager.BattleGrid.Tilemap;
    }

    public BuildingManager BuildManager {
        get => _gameManager.BuildingManager;
    }
    //public Tilemap MainTilemap;
    //public Tilemap TempTilemap;


    //private static Dictionary<TileType, TileBase> tileBases = new Dictionary<TileType, TileBase>();

    private Building temp;
    //private Vector3 prevPos;
    //private BoundsInt prevArea;
    //public TileBase green;
    //public TileBase red;
    //public TileBase white;

    #region Unity Methods
    private void Update()
    {
        //if (!temp)
        //{
        //    return;
        //}
        //if (Input.GetMouseButton(0))
        //{
        //    if (EventSystem.current.IsPointerOverGameObject(0))
        //    {
        //        return;
        //    }

        //    if (!temp.Placed)
        //    {
        //        Vector2 touchPos = BattleGrid.GetMouseWorldPosition();
        //        Vector3Int cellPos = GridLayout.LocalToCell(touchPos);

        //        if (prevPos != cellPos)
        //        {
        //            temp.transform.localPosition = GridLayout.CellToLocalInterpolated(cellPos + new Vector3(0.5f, .5f, 0f));
        //            prevPos = cellPos;
        //            FollowBuilding();
        //        }
        //    }
        //}
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    if (temp.CanBePlaced())
        //    {
        //        temp.Place();
        //    }
        //}
        //else if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    ClearArea();
        //    Destroy(temp.gameObject);
        //}
    }

    public void Init(GameManager gm)
    {
        _gameManager = gm;
    }

    #endregion
    #region Tilemap Management


    //private static void FillTiles(TileBase[] arr, TileType type)
    //{
    //    for (int i = 0; i < arr.Length; i++)
    //    {
    //        arr[i] = tileBases[type];
    //    }
    //}
    //private static void SetTilesBlock(BoundsInt area, TileType type, Tilemap tilemap)
    //{
    //    int size = area.size.x * area.size.y * area.size.z;
    //    TileBase[] tileArray = new TileBase[size];
    //    FillTiles(tileArray, type);
    //    tilemap.SetTilesBlock(area, tileArray);

    //}
    //private static TileBase[] GetTilesBlock(BoundsInt area, Tilemap tilemap)
    //{
    //    TileBase[] array = new TileBase[area.size.x * area.size.y * area.size.z];
    //    int counter = 0;

    //    foreach (Vector3Int v in area.allPositionsWithin)
    //    {
    //        Vector3Int pos = new Vector3Int(v.x, v.y, 0);
    //        array[counter] = tilemap.GetTile(pos);
    //        counter++;
    //    }
    //    return array;
    //}

    #endregion
    #region Building Placement
    public void IntitializeWithBuilding(ScriptableUnit unit,Vector3Int gridPos)
    {
        temp = new GameObject(unit.unitsName,typeof(Building),typeof(SpriteRenderer),typeof(SwitchLevels)).GetComponent<Building>();
        temp.GetComponent<SpriteRenderer>().sortingOrder = 4;
        temp.GetComponent<SpriteRenderer>().sprite = unit.renderUnit;
        temp.transform.position = gridPos;
        BuildManager.Buildings.Add(temp);
        //FollowBuilding();
    }

    //private void ClearArea()
    //{
    //    TileBase[] toClear = new TileBase[prevArea.size.x * prevArea.size.y * prevArea.size.z];
    //    FillTiles(toClear, TileType.White);
    //    TempTilemap.SetTilesBlock(prevArea, toClear);
    //}

    //private void FollowBuilding()
    //{
    //    ClearArea();

    //    temp.area.position = GridLayout.WorldToCell(temp.gameObject.transform.position);

    //    BoundsInt buildingArea = temp.area;

    //    TileBase[] basesArray = GetTilesBlock(buildingArea, MainTilemap);

    //    int size = basesArray.Length;
    //    for (int i = 0; i < size; i++)
    //    {
    //        basesArray[i] = tileBases[TileType.Green];
    //    }
    //    TileBase[] tileArray = new TileBase[size];

    //    /*for (int i = 0; i < basesArray.Length; i++)
    //    {
    //        if (basesArray[i] == tileBases[TileType.White])
    //        {
    //            tileArray[i] = tileBases[TileType.Green];
    //        }
    //        else
    //        {
    //            FillTiles(tileArray, TileType.Red);
    //            break;
    //        }
    //    }*/



    //    TempTilemap.SetTilesBlock(buildingArea, basesArray);
    //    prevArea = buildingArea;
    //}

    //public bool CanTakeArea(BoundsInt area)
    //{
    //    TileBase[] baseArray = GetTilesBlock(area, MainTilemap);
    //    foreach (var b in baseArray)
    //    {
    //        if (b != tileBases[TileType.White])
    //        {
    //            Debug.Log("peut pas mettre ici");
    //            return false;
    //        }
    //    }
    //    return true;
    //}
    //public void TakeArea(BoundsInt area)
    //{
    //    SetTilesBlock(area, TileType.Empty, TempTilemap);
    //    SetTilesBlock(area, TileType.Green, MainTilemap);
    //}


    #endregion

    //public enum TileType
    //{
    //    Empty,
    //    White,
    //    Green,
    //    Red
    //}
}
