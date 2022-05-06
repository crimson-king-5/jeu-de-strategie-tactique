using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleGrid : MonoBehaviour
{
    private int width;
    private int height;
    private float cellSize;
    private Vector2 originPosition;
    private int[,] gridArray;
    private TextMesh[,] debugTextArray;
    public BattleGrid(int width, int height, float cellSize, int fontSize, Vector2 originPosition, ref GameObject gridParent)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        int xOrginPos = Mathf.FloorToInt(originPosition.x);
        int yOrginPos = Mathf.FloorToInt(originPosition.y);
        originPosition = new Vector2(xOrginPos, yOrginPos);
        this.originPosition = originPosition;
        gridArray = new int[width, height];
        debugTextArray = new TextMesh[width, height];
        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                debugTextArray[x, y] = CreateText(null, gridArray[x, y].ToString(), GetWorldPosition(x, y) + new Vector2(cellSize, cellSize) * .5f, fontSize);
                debugTextArray[x, y].transform.SetParent(gridParent.transform);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 1f);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 1f);
            }
        }
        Debug.DrawLine(GetWorldPosition(0, this.height), GetWorldPosition(this.width, this.height), Color.white, 1f);
        Debug.DrawLine(GetWorldPosition(this.width, 0), GetWorldPosition(this.width, this.height), Color.white, 1f);
    }

    public BattleGrid SetBattleGrid(int width, int height, float cellSize, int fontSize, Vector2 originPosition, ref GameObject gridParent,BattleGrid grid)
    {
        grid.width = width;
        grid.height = height;
        grid.cellSize = cellSize;
        int xOrginPos = Mathf.FloorToInt(originPosition.x);
        int yOrginPos = Mathf.FloorToInt(originPosition.y);
        originPosition = new Vector2(xOrginPos, yOrginPos);
        grid.originPosition = originPosition;
        gridArray = new int[width, height];
        debugTextArray = new TextMesh[width, height];
        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                debugTextArray[x, y] = CreateText(null, gridArray[x, y].ToString(), GetWorldPosition(x, y) + new Vector2(cellSize, cellSize) * .5f, fontSize);
                debugTextArray[x, y].transform.SetParent(gridParent.transform);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 1f);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 1f);
            }
        }
        Debug.DrawLine(GetWorldPosition(0, grid.height), GetWorldPosition(grid.width, grid.height), Color.white, 1f);
        Debug.DrawLine(GetWorldPosition(grid.width, 0), GetWorldPosition(grid.width, grid.height), Color.white, 1f);
        return grid;
    }

    public int GetValue(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            return gridArray[x, y];
        }
        else
        {
            return 0;
        }
    }

    public void SetValue(int x, int y, int value)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            gridArray[x, y] = value;
            debugTextArray[x, y].text = gridArray[x, y].ToString();
        }
    }

    public void SetValue(Vector2 worldPosition, int value)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        SetValue(x, y, value);
    }

    public void SetObjectToGrid(Vector2 worldPosition, GameObject obj, List<GameObject> objects, ref GameObject tilesParent)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            for (int i = 0; i < objects.Count; i++)
            {
                if (objects[i].transform.position == debugTextArray[x, y].transform.position)
                {
                    DestroyImmediate(objects[i]);
                    objects.Remove(objects[i]);
                }
            }
            GameObject instanceObj = Instantiate(obj);
            if (tilesParent == null)
            {
                tilesParent = new GameObject("Grid Objects");
                instanceObj.transform.SetParent(tilesParent.transform);
            }
            else
            {
                instanceObj.transform.SetParent(tilesParent.transform);
            }
            instanceObj.transform.position = debugTextArray[x, y].transform.position;
            objects.Add(instanceObj);
        }
    }

    public void DeleteObjectToGrid(Vector2 worldPosition, List<GameObject> objects)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            for (int i = 0; i < objects.Count; i++)
            {
                if (objects[i].transform.position == debugTextArray[x, y].transform.position)
                {
                    DestroyImmediate(objects[i]);
                    objects.Remove(objects[i]);
                }
            }
        }
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
        GameObject textObj = new GameObject("World_Text", typeof(TextMesh));
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
