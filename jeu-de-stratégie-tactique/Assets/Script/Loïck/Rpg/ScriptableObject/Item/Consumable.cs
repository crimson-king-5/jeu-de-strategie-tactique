using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering.UI;

public enum AddOrDelete
{
    Add, Minus
}
[System.Serializable]
public class Consumable : ItemData<Consumable>
{
    [ShowInInspector] public StatModificator[] stat;
    [BoxGroup("Item")]
    public CharactersEnum.CharacterStatut conStatut;
    [BoxGroup("Item")]
    public AddOrDelete addOrDelete = 0;

    public override Consumable GetItem()
    {
        return this;
    }

    public override ItemType InitType()
    {
        throw new System.NotImplementedException();
    }

    public override void UseItem(CharacterData combatant)
    {
        if (combatant.characterStatut != CharactersEnum.CharacterStatut.Death)
        {
            if (conStatut != CharactersEnum.CharacterStatut.None)
            {
                combatant.characterStatut = conStatut;
            }
            switch (addOrDelete)
            {
                case AddOrDelete.Add:
                    foreach (StatModificator statMod in stat)
                    {
                        int characterStatValue = combatant.GetModificatorToStat(statMod.statType);
                        characterStatValue += statMod.value;
                        //combatant.stat.SetStat(statMod.statType, characterStatValue);
                    }
                    break;
                case AddOrDelete.Minus:
                    foreach (StatModificator statMod in stat)
                    {
                        int characterStatValue = combatant.GetModificatorToStat(statMod.statType);
                        characterStatValue -= statMod.value;
                        //combatant.stat.SetStat(statMod.statType, characterStatValue);
                    }
                    break;
            }
        }
    }
}
