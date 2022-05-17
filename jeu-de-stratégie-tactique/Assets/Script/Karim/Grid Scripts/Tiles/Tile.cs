using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{

    [SerializeField] private bool _isWalkable;
    public TileType currentTileType = TileType.None;

    public BaseUnit OccupiedUnit;
    public bool Walkable => _isWalkable && OccupiedUnit == null && currentTileType != TileType.None;

    void Start()
    {

    }

    public void CheckIfCanWalk()
    {
        switch (currentTileType)
        {
            case TileType.Walkable:
                _isWalkable = true;
                break;
            case TileType.None:
                _isWalkable = false;
                break;
        }
    }

    private void OnMouseEnter()
    {
        MenuManager.Instance.ShowTileInfo(this);
    }

    private void OnMouseExit()
    {
        MenuManager.Instance.ShowTileInfo(null);
    }

    private void OnMouseDown()
    {
        if (GameManager.Instance.gameState != GameManager.GameState.HerosTurn) return;
        
        if(OccupiedUnit != null)
        {
            if (OccupiedUnit.scriptableUnit.faction == Faction.Hero) UnitManager.Instance.SetSelectedHero((BaseUnit)OccupiedUnit);
            else
            {
                if(UnitManager.Instance.SelectedHero != null)
                {
                    var enemy = (BaseEnemy)OccupiedUnit;
                    Destroy(enemy.gameObject);
                    UnitManager.Instance.SetSelectedHero(null);
                }
            }
        }else
        {
            if(UnitManager.Instance.SelectedHero != null)
            {
                SetUnit(UnitManager.Instance.SelectedHero);
                UnitManager.Instance.SetSelectedHero(null);
            }
        }
    
    }
    public void SetUnit(BaseUnit unit)
    {
        if (unit.OccupiedTile != null) unit.OccupiedTile.OccupiedUnit = null;
        unit.transform.position = transform.position;
        OccupiedUnit = unit;
        unit.OccupiedTile = this;
    }

    public enum TileType
    {
        None,Walkable
    }
}
