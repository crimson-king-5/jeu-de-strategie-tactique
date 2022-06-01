using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;


public abstract class ItemData<T> : ScriptableObject
{
    [BoxGroup("Item"), PreviewField(100f)] public Sprite sprite;
    [BoxGroup("Item")] public string itemName;
    [BoxGroup("Item")] private ItemType type;
    [BoxGroup("Item"), TextArea()] public string description;
    [BoxGroup("Item")] public int buy = 0;
    [BoxGroup("Item")]
    private int quantity
    {
        get
        {
            return quantity;
        }
        set
        {
            if (quantity > quantityMax)
            {
                quantity = quantityMax;
            }
            else if (quantity < 0)
            {
                quantity = 0;
            }
        }
    }
    [BoxGroup("Item")] public int quantityMax;

    public int cost
    {
        get { return buy % 10; }
    }

    public enum ItemType
    {
        CONSOMABLE, EQUIPEMENT
    }

    public abstract T GetItem();

    public abstract ItemType InitType();

    public abstract void UseItem(CharacterData combatant);

    public int GetItemQuantity()
    {
        return quantity;
    }
}


public static class ItemClassType
{
    public static ItemData<ScriptableObject>.ItemType GetItemClass(ScriptableObject currentItem)
    {
        Equipement itemEquipement = currentItem as Equipement;
        Consumable itemConsomable = currentItem as Consumable;
        if (itemEquipement != null)
        {
            return ItemData<ScriptableObject>.ItemType.EQUIPEMENT;
        }
        else
        {
            return ItemData<ScriptableObject>.ItemType.CONSOMABLE;
        }
    }

}