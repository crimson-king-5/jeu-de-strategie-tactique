using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TEAM2
{
    [CreateAssetMenu(fileName = "New Build ", menuName = "Scriptable Build")]
    public class ScriptableBuilding : ScriptableUnit
    {
        public UprgadeList upgrades;
        public BuildType currentBuildType;
        void Reset()
        {
            faction = Faction.Building;
            unitUnitClass = UnitClass.Neutral;
            classBonus = 0;
        }
    }

    [System.Serializable]
    public class Upgrade
    {
        [Range(1, 10)] public float gainBonus;
        public Sprite upgradeRender;
    }

    [System.Serializable]
    public class UprgadeList
    {
        public List<Upgrade> upgrades;
    }
}
