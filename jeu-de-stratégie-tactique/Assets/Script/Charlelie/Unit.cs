using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TEAM2
{
    public class Unit : MonoBehaviour
    {
        protected GameManager _gameManager;
        protected ScriptableUnit _scrUnit;
        public int xPos;
        public int yPos;

        public BattleGrid BattleGrid
        {
            get => _gameManager.BattleGrid;
        }

        public ScriptableUnit ScrUnit
        {
            get => _scrUnit;
            set => _scrUnit = value;
        }

        public int MaxLife
        {
            get;
            private set;
        }

        int _currentLife;
        
        public void Init(GameManager gm)
        {
            _gameManager = gm;
            _scrUnit.GetCloneUnit();
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

    public enum UnitType
    {
        Building,Character
    }
}
