using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

public class TilesSelector : OdinEditorWindow
{
    [MenuItem("Cassoulet Tool/Tiles Selector")]
    private static void OpenWindow()
    {
        GetWindow<TilesSelector>().Show();
    }
    public TileList Editor;
}

[System.Serializable]
public class TileList
{
    [AssetList(Path = "Prefabs/Tiles")]
    [PreviewField(70, Sirenix.OdinInspector.ObjectFieldAlignment.Center)]
    public GameObject SomePrefab;
    [TableColumnWidth(160)]
    [Button("Selection Tile", ButtonSizes.Medium), GUIColor(0, 1, 0, 1)]
    public void PickupThePrefab()
    {
        if (GameObject.FindGameObjectWithTag("Grid").GetComponent<BattleGrid>() != null)
        {
            BattleGrid gridInit = GameObject.FindGameObjectWithTag("Grid").GetComponent<BattleGrid>();
            gridInit.currentTilesRef = SomePrefab;
        }
    }
}
