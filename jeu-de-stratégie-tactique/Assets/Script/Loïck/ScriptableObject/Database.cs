using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace TEAM2
{
    [CreateAssetMenu(menuName = "Database", fileName = "Database")]
    public class Database : ScriptableObject
    {
        public List<ScriptableUnit> dataBase;
        public IEnumerable<ScriptableBuilding> ScriptableBuildings => dataBase.Where(i => i is ScriptableBuilding).Cast<ScriptableBuilding>();
        /*public IEnumerable<ScriptableUnit> ScriptableMage => dataBase.Where(i => i.unitUnitClass == UnitClass.Mage);
        public IEnumerable<ScriptableUnit> ScriptableTank => dataBase.Where(i => i.unitUnitClass == UnitClass.Tank);
        public IEnumerable<ScriptableUnit> ScriptableWarrior => dataBase.Where(i => i.unitUnitClass == UnitClass.Warrior);*/

#if UNITY_EDITOR
        [Button("Initialize Database")]
        void InitScriptableObject()
        {
            dataBase = Resources.LoadAll<ScriptableUnit>("Units").ToList();
        }
# endif

    }

}
