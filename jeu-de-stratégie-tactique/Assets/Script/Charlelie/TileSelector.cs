using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TEAM2;
using UnityEngine.Tilemaps;

public class TileSelector : MonoBehaviour
{
    [SerializeField] BattleGrid grid;
    BattleGridTile selectedTile;
    Vector3Int selectedTileVec;
    BattleGridTile mouseOverTile;
    Vector3Int mouseOverTileVec;
    public static TileSelector instance;
    public bool SelectTile { get; set; }

    public OrderType currType; 

    public BattleGridTile SelectedTile
    {
        get => selectedTile;
    }

    public Vector3Int SelectedTileVec
    {
        get => selectedTileVec;
    }

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {

    }

    public void ResetTile()
    {
        if (selectedTile == null) return;
        grid.Tilemap.SetColor(selectedTileVec, mouseOverTile.baseColor);
        selectedTile = null;
        mouseOverTile = null;
        selectedTileVec = Vector3Int.zero;
        mouseOverTileVec = Vector3Int.zero;
    }


    void Update()
    {
        if (SelectTile)
        {
            Vector3 mousePos = BattleGrid.GetMouseWorldPosition();
            Vector3Int intMousPos = new Vector3Int((int)mousePos.x, (int)mousePos.y);
            BattleGridTile currentTile = (BattleGridTile)grid.Tilemap.GetTile(intMousPos);

            if (currentTile)
            {
                if (mouseOverTile == null)
                {
                    mouseOverTileVec = intMousPos;
                    mouseOverTile = currentTile;
                    grid.Tilemap.SetColor(mouseOverTileVec, Color.blue);
                    mouseOverTile.OnMouseEnter();
                }
                else if (currentTile != mouseOverTile)
                {
                    mouseOverTile.OnMouseLeave();
                    grid.Tilemap.SetColor(mouseOverTileVec, mouseOverTile.baseColor);
                    mouseOverTile = currentTile;
                    mouseOverTile.OnMouseEnter();
                    mouseOverTileVec = intMousPos;
                    grid.Tilemap.SetColor(mouseOverTileVec, Color.blue);
                }
                else mouseOverTile.OnMouseOver();
            }
            if (Input.GetMouseButtonDown(0))
            {
                selectedTile = mouseOverTile;
                SelectTile = false;
                selectedTileVec = mouseOverTileVec;
                UIManager.Instance.CreateOrder(currType);
            }
        }
    }
   
}