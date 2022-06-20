using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;
using TEAM2;

public class Cell
{
    bool selected;

    BattleGridTile _tile;

    Vector3Int position;
    Vector3 posCenter;
    Vector3Int size;
    Tilemap _tilemap;
    BattleGrid battleGrid;
    bool clicked;
    Color currColor;

    float prevRange = 0;

    public Unit Contains { get; set; } 

    public Vector3Int Position { get => position; }

    public Vector3 PosCenter { get => posCenter; }

    static public event System.Action<Cell> OnClickCell;

    public Cell(Vector3Int pos, Tilemap tilemap, BattleGridTile tile)
    {
        _tilemap = tilemap;
        _tile = tile;

        position = pos;
        posCenter = _tilemap.GetCellCenterWorld(position);

        battleGrid = GameManager.Instance.BattleGrid;
        battleGrid.OnSetNeighbors += SetNeighbors;

        currColor = tile.baseColor;
    }

    class Neighbors
    {
        internal Cell top;
        internal Cell bottom;
        internal Cell left;
        internal Cell right;
        internal Cell tr;
        internal Cell tl;
        internal Cell br;
        internal Cell bl;
        internal Cell curr; 
        internal Cell[] a;

        public Neighbors(Dictionary<Vector3Int, Cell> dict, Vector3Int pos)
        {
            a = new Cell[] {tl, top, tr,
                                      left, right,
                                      bl, bottom, br};

            curr = dict[pos];

            int index = 0;
            for (int x = -1; x < 2; x++)
            {
                for (int y = -1; y < 2; y++)
                {
                    if (!(x == 0 && y == 0))
                    {
                        Vector3Int nPos = new Vector3Int(x, y, 0);
                        Cell nCell;
                        if (dict.TryGetValue(pos + nPos, out nCell))
                        {
                            a[index] = nCell;
                        }
                        index++;
                    } 
                }
            }
        }

        public void Illum()
        {
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] != null) a[i]._tilemap.SetColor(a[i].position, Color.magenta);
            }
        }

        public void Desillum()
        {
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] != null) a[i]._tilemap.SetColor(a[i].position, a[i]._tile.baseColor);
            }
        }

        public List<Cell> IllumWithRange(float range)
        {
            Dictionary<Vector3Int, Cell> dict = GameManager.Instance.BattleGrid.CellDict;
            List<Cell> cells = new List<Cell>();
            for (int i = 0; i < dict.Count; i++)
            {
                float dist = Vector3Int.Distance(dict.ElementAt(i).Value.position, curr.position);
                if (dist <= range && (dict.ElementAt(i).Value.position != curr.position))
                {
                    cells.Add(dict.ElementAt(i).Value);
                    dict.ElementAt(i).Value._tilemap.SetColor(dict.ElementAt(i).Value.position, Color.magenta);
                    dict.ElementAt(i).Value.currColor = Color.magenta;
                }
            }
            return cells;
        }

        public void DesillumWithRange(float range)
        {
            Dictionary<Vector3Int, Cell> dict = GameManager.Instance.BattleGrid.CellDict;
            for (int i = 0; i < dict.Count; i++)
            {
                float dist = Vector3Int.Distance(dict.ElementAt(i).Value.position, curr.position);
                if (dist <= range && (dict.ElementAt(i).Value.position != curr.position)) 
                {
                    dict.ElementAt(i).Value._tilemap.SetColor(dict.ElementAt(i).Value.position, dict.ElementAt(i).Value._tile.baseColor);
                    dict.ElementAt(i).Value.currColor = dict.ElementAt(i).Value._tile.baseColor;
                }
            }
        }

        public List<Unit> CheckAroundAll(Unit currUnit)
        {
            List<Unit> list = new List<Unit>();
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i].Contains != null && a[i].Contains.Faction != currUnit.Faction) list.Add(a[i].Contains);
            }
            return list;
        }

    }

    Neighbors nbs;

    public enum TileType
    {
        None = 0,
        Walkable = 1,
        Spawn = 2,
        Ruin = 3,
        MotherBase = 4
    }

    void SetNeighbors(Dictionary<Vector3Int, Cell> dict)
    {
        nbs = new Neighbors(dict, position);
    }
   
    public void ShowWalkableCells(float range)
    {
        nbs.IllumWithRange(range);
    }

    public List<Unit> CheckNeighbours(Unit currUnit)
    {
        return nbs.CheckAroundAll(currUnit);
    }

    public void HideWalkableCells(float range)
    {
        nbs.DesillumWithRange(range);
    }

    public bool CanWalkOnCell(float range)
    {
        if (battleGrid.MouseOverCell == null) return false;
        float dist = Vector3Int.Distance(position, battleGrid.MouseOverCell.position);
        if (dist <= range)
        {
            _tilemap.SetColor(battleGrid.MouseOverCell.position, Color.white);
            return true;
        } else return false;

    }

    public void ResetColor()
    {
        _tilemap.SetColor(position, _tile.baseColor);
        currColor = _tile.baseColor;
    }

    public void SetColor(Color col)
    {
        _tilemap.SetColor(position, col);
        currColor = col;
    }

    public void OnMouseEnter()
    {
        //Debug.Log("Mouse enter: " + name + ",  " + currentTileType + ", " + position);
        //_tilemap.SetColor(position, Color.white);
        //currColor = Color.white;
    }

    public void OnMouseOver()
    {
        //Debug.Log("Mouse over: " + name + ",  " + currentTileType + ", " + position);
    }

    public void OnMouseLeave()
    {
        //Debug.Log("Mouse leave: " + name + ",  " + currentTileType + ", " + position);
        _tilemap.SetColor(position, currColor);
        //currColor = _tile.baseColor;
        if (clicked) OnMouseClickUp();
    }

    public void OnMouseClickDown()
    {
        //Debug.Log("Mouse click down: " + position);
        //nbs.Illum();
        //nbs.IllumWithRange(GameManager.Instance.range);
        if (Contains != null) Contains.OnClick();
        OnClickCell.Invoke(this);
        clicked = true;
    }

    public void OnMouseDown()
    {
        /*
        if (GameManager.Instance.range != prevRange)
        {
            prevRange = GameManager.Instance.range;
            nbs.DesillumWithRange(GameManager.Instance.range);
            nbs.IllumWithRange(GameManager.Instance.range);
        }
        */
    }
    public void OnMouseClickUp()
    {
        //Debug.Log("Mouse click up: " + name + ",  " + currentTileType + ", " + position);
        //nbs.Desillum();
        //nbs.DesillumWithRange(GameManager.Instance.range);
        clicked = false;
    }
}