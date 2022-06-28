using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TEAM2
{
    [CreateAssetMenu(fileName = "New Build ", menuName = "Scriptable Build")]
    public class ScriptableBuilding : ScriptableUnit
    {
        public List<ScriptableUnit> charactersUnlocked = new List<ScriptableUnit>();
        public int gainResource = 1;
        public int armorBonus = 0;
        public bool canAddUnits;
        void Reset()
        {
            faction = Faction.Building;
            unitUnitClass = UnitClass.Neutral;
            classBonus = 0;
        }
    }
}
