using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[System.Serializable]
public class StatModificator
{
    [Range(0, 99)] public int value = 0;
    [LabelText("Is Buff or Debuff ?")] public Modificator isbuff;
    [EnumToggleButtons] public CharactersEnum.Stat statType;

    public enum Modificator
    {
        Buff,
        Debuff
    }
}