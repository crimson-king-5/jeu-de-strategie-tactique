using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

public class TilesSelectorwindows : OdinMenuEditorWindow
{
    [MenuItem("Cassoulet Tool/Tiles Selector")]
    protected override OdinMenuTree BuildMenuTree()
    {

        var tree = new OdinMenuTree(); ;
        tree.AddAllAssetsAtPath("", "Assets/Prefabs/Tiles", typeof(GameObject));

        return tree;
    }
}
