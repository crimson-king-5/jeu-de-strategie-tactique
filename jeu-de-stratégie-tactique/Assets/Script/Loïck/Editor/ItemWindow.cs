using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ItemWindow : OdinMenuEditorWindow
{
    [MenuItem("Cassoulet Tool/Create Item Editor")]
    private static void OpenWindow()
    {
        GetWindow<ItemWindow>().Show();
    }

    protected override OdinMenuTree BuildMenuTree()
    {
        CreateEquipement equipement = new CreateEquipement();
        CreateConsumable consumable = new CreateConsumable();
        var tree = new OdinMenuTree();

        tree.Add("New Consumable", consumable);
        tree.Add("New Equipement", equipement);
        tree.AddAllAssetsAtPath("Equipement", "Assets/Database/Item/Equipement", typeof(Equipement));
        tree.AddAllAssetsAtPath("Consumable", "Assets/Database/Item/Consumable", typeof(Consumable));

        return tree;
    }
    protected override void OnBeginDrawEditors()
    {
        OdinMenuTreeSelection selected = this.MenuTree.Selection;

        SirenixEditorGUI.BeginHorizontalToolbar();
        {
            GUILayout.FlexibleSpace();

            if (SirenixEditorGUI.ToolbarButton("Supprimer Consomable"))
            {
                Consumable asset = selected.SelectedValue as Consumable;
                string path = AssetDatabase.GetAssetPath(asset);
                AssetDatabase.DeleteAsset(path);
                AssetDatabase.SaveAssets();
            }
            if (SirenixEditorGUI.ToolbarButton("Supprimer Equipement"))
            {
                Equipement asset = selected.SelectedValue as Equipement;
                string path = AssetDatabase.GetAssetPath(asset);
                AssetDatabase.DeleteAsset(path);
                AssetDatabase.SaveAssets();
            }
        }
        SirenixEditorGUI.EndHorizontalToolbar();
    }
    public class CreateConsumable
    {
        public CreateConsumable()
        {
            consumable = ScriptableObject.CreateInstance<Consumable>();
            consumable.itemName = "New Consumable";
        }

        [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
        public Consumable consumable;

        [Button("Create New Consumable")]
        private void CreateNewConsumable()
        {
            AssetDatabase.CreateAsset(consumable, "Assets/Database/Item/Consumable/" + consumable.itemName + ".asset");
            AssetDatabase.SaveAssets();

            //Instance le ScriptableObject
            consumable = ScriptableObject.CreateInstance<Consumable>();
            consumable.itemName = "New Consumable";
        }
    }
    public class CreateEquipement
    {
        public CreateEquipement()
        {
            equipement = ScriptableObject.CreateInstance<Equipement>();
            equipement.itemName = "New Equipement";
        }

        [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
        public Equipement equipement;

        [Button("Create Equipement")]
        private void CreateNewEquipement()
        {
            if (equipement != null)
            {
                AssetDatabase.CreateAsset(equipement, "Assets/Database/Item/Equipement/" + equipement.itemName + ".asset");
                AssetDatabase.SaveAssets();

                //Instance le ScriptableObject
                equipement = ScriptableObject.CreateInstance<Equipement>();
                equipement.itemName = "New Equipement";
            }
        }
    }
}
