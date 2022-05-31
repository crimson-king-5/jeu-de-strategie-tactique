using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TEAM2
{
    public class Building : Unit
    {
        public ScriptableUnit scrUnit;

        public override void Init(Vector2Int position)
        {
            base.Init(position);
            scrUnit = scrUnit.GetCloneUnit();
            //Do Anim
        }

        public override void DoAction()
        {
            base.DoAction();
        }

        void Build()
        {

        }

        void GiveResources()
        {

        }


        public override void Die()
        {
            base.Die();
        }
    }
}
