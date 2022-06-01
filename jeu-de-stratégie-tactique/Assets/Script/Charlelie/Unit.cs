using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TEAM2
{
    public class Unit : MonoBehaviour
    {
        protected Vector2Int _position;
        protected BattleGrid grid;

        public int MaxLife
        {
            get;
            private set;
        }

        int _currentLife;

        virtual public void Init(Vector2Int position)
        {
            _position = position;
        }

        virtual public void DoAction()
        {
            
        }

        virtual public void TakeDamage()
        {

        }
        virtual public void Die()
        {

        }

    }
}
