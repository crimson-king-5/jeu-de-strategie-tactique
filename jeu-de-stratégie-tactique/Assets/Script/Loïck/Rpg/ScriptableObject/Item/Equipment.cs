using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public enum EquipType
{
    DevilFruit,
    Sword,
    Gun
}

[System.Serializable]
public class Equipement : ItemData<Equipement>
{
    [EnumToggleButtons,BoxGroup("Item")]
    public EquipType equipType = 0;
    [ShowInInspector] public StatModificator[] stat;
    [Range(1,99),ShowInInspector]
    private int range = 1;
    [Range(1, 99), ShowInInspector] private int attack;

    public override Equipement GetItem()
    {
        return this;
    }

    public int GetAttack()
    {
        return attack;
    }

    public override ItemType InitType()
    {
        throw new System.NotImplementedException();
    }

    public override void UseItem(CharacterData combatant)
    {
        throw new System.NotImplementedException();
    }
}
