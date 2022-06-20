using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TEAM2
{
    public class Unit : MonoBehaviour
    {
        protected GameManager _gameManager;
        [SerializeField]protected ScriptableUnit _scrUnit;
        UnitType _unitType;
        private Vector3Int _occupiedTileGridPosition;

        public int xPos;
        public int yPos;

        public UnitStateMachine unitStateMachine = new UnitStateMachine();

        public Faction Faction
        {
            get => _scrUnit.faction;
        }

        public UnitType UnitType
        {
            get => _unitType;
        }
        public UIManager UIManager
        {
            get => _gameManager.UIManager;
            set => _gameManager.UIManager = value;
        }

        public Vector3Int OccupiedTileGridPosition
        {
            get => _occupiedTileGridPosition;
            set => _occupiedTileGridPosition = value;
        }

        public BattleGrid BattleGrid
        {
            get => _gameManager.BattleGrid;
        }

        public PlayerManager PlayerManager
        {
            get => _gameManager.PlayerManager;
        }

        public ScriptableUnit ScrUnit
        {
            get => _scrUnit;
            set => _scrUnit = value;
        }

        public virtual void Init(GameManager gm,UnitType unitType)
        {
            _gameManager = gm;
           _scrUnit = _scrUnit.GetCloneUnit();
           OccupiedTileGridPosition = GetCurrentUnitGridlPosition();
           _unitType = unitType;
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

        virtual public void OnClick()
        {
            if (_gameManager.UnitManager.SelectedHero != null && _gameManager.UnitManager.SelectedHero != this) 
                if (_gameManager.UnitManager.SelectedHero.OnDeselect()) _gameManager.UnitManager.SelectedHero = this;
        }

        virtual public bool OnDeselect()
        {
            return true;
        }

        public int GetTileRange(Vector3 newPos)
        {
            Vector3Int GridposDestination = GetUnitDestinationGridPosition(newPos);
            Vector3Int moveRange = GetCurrentUnitGridlPosition() - GridposDestination;
            return Mathf.Abs(moveRange.x) + Mathf.Abs(moveRange.y) + BattleGrid.GetTileType(GridposDestination).mvRequire;
        }

        public Vector3Int GetSpecificGridPosition(Vector3 newPos)
        {
            return BattleGrid.Tilemap.WorldToCell(newPos);
        }

        public Vector3Int GetUnitDestinationGridPosition(Vector3 charaDestinationWorldPos)
        {
            return BattleGrid.Tilemap.WorldToCell(charaDestinationWorldPos);
        }

        public Vector3Int GetCurrentUnitGridlPosition()
        {
            return BattleGrid.Tilemap.WorldToCell(transform.position);
        }


        public Vector3 GetUnitDestinationWorldPosition(Vector3Int gridPos)
        {
            return BattleGrid.Tilemap.GetCellCenterWorld(gridPos);
        }
    }

    public enum UnitType
    {
        Building = 0,
        Character = 1
    }
}
