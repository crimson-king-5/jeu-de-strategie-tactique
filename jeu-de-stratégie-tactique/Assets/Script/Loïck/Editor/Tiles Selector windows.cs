using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

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
        BattleGrid gridInit = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().BattleGrid;
        gridInit.currentTilesRef = SomePrefab;
    }
}
