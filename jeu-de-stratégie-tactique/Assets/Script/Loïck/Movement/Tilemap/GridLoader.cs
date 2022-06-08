using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

public class GridLoader : ScriptableObject
{
    public int[,] gridArray;
    public TextMesh[,] debugTextArray;
    public int width;
    public int height;

    public GridLoader(int[,] currentGridArray, TextMesh[,] currentDebugTextArray, int currentHeight,int currentWidth)
    {
        width = currentWidth;
        height = currentHeight;
        gridArray = currentGridArray;
        debugTextArray = currentDebugTextArray;
    }
}
