using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Inventory
{
    public List<Equipement> itemsEquipement = new List<Equipement>();
    public List<Consumable> itemConsumable = new List<Consumable>();
    private ScriptableObject currentItem;
    public int indexNavigation;

    public ScriptableObject GetCurrentItem()
    {
        ItemClassType.GetItemClass(currentItem);
        switch (ItemClassType.GetItemClass(currentItem))
        {
            case ItemData<ScriptableObject>.ItemType.CONSOMABLE:
                return itemConsumable[indexNavigation];
            case ItemData<ScriptableObject>.ItemType.EQUIPEMENT:
                return itemsEquipement[indexNavigation];
        }
        Debug.LogError("Item non reconnu");
        return null;
    }
}
