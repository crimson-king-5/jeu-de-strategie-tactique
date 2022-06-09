using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TEAM2
{
    [CreateAssetMenu(fileName = "Tile", menuName = "Tile/Faction Tile")]
    public class FactionTile : BattleGridTile
    {
        public Faction faction;
    }
}
