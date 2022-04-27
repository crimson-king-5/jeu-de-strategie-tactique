using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "Char_", menuName = "Rpg/Character")]
public class CharacterData : ScriptableObject
{
    [BoxGroup("Fiche")]
    //public CharacterClass charClass;
    [ShowInInspector, BoxGroup("Fiche")]
    public string characterName = "";
    [TextArea()]
    public string story = "";
    [EnumToggleButtons, BoxGroup("Fiche")]
    public CharactersEnum.Race characterRace;
    [ShowInInspector, BoxGroup("Stat"), LabelText("")]
    public CharacterStat stat;

    [BoxGroup("Statut")]
    public CharactersEnum.CharacterStatut characterStatut = CharactersEnum.CharacterStatut.None;
    [ShowInInspector, BoxGroup("Inventory")]
    public Consumable[] consumables = new Consumable[10];
    [ShowInInspector, BoxGroup("Inventory")]
    public Equipement weapon;

    public int GetModificatorToStat(CharactersEnum.Stat currentStat)
    {
        int newStat = 0;  // stat.GetStat(currentStat);
        StatModificator[] statsModificator = null;

        for (int i = 0; i < statsModificator.Length; i++)
        {
            if (statsModificator[i].statType == currentStat)
            {
                newStat += statsModificator[i].value;
                break;
            }
        }

        return newStat;
    }
}
[System.Serializable]
public class CharacterStat
{
    
}
