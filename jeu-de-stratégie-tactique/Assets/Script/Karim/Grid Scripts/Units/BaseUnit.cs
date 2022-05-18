using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class BaseUnit : MonoBehaviour
{
    public Tile OccupiedTile;
    [HideInInspector] public ScriptableUnit scriptableUnit;
    public int xPos;
    public int yPos;
    public int life { get { return scriptableUnit.unitStats.life; } }
    public int range { get { return scriptableUnit.unitStats.range; } }
    public int atk { get { return scriptableUnit.unitStats.atk; } }
    public int mv { get { return scriptableUnit.unitStats.mv; } }

    public UnitStateMachine unitStateMachine;
    public Direction currentOrientation;

    public void Attack(Direction dir)
    {
        Tile targetTile;
        int targetLife;
        switch (dir)
        {
            case Direction.Up:
                targetTile = BattleGrid.instance.GetTile(xPos, yPos + range);
                if (targetTile.OccupiedUnit)
                {
                    return;
                }
                targetLife = targetTile.OccupiedUnit.scriptableUnit.unitStats.life;
                targetLife -= atk;
                break;
            case Direction.Down:
                targetTile = BattleGrid.instance.GetTile(xPos, yPos - range);
                if (targetTile.OccupiedUnit)
                {
                    return;
                }
                targetLife = targetTile.OccupiedUnit.scriptableUnit.unitStats.life;
                targetLife -= atk;
                break;
            case Direction.Left:
                targetTile = BattleGrid.instance.GetTile(xPos - range, yPos);
                if (targetTile.OccupiedUnit)
                {
                    return;
                }
                targetLife = targetTile.OccupiedUnit.scriptableUnit.unitStats.life;
                targetLife -= atk;
                break;
            case Direction.Right:
                targetTile = BattleGrid.instance.GetTile(xPos + range, yPos);
                if (targetTile.OccupiedUnit)
                {
                    return;
                }
                targetLife = targetTile.OccupiedUnit.scriptableUnit.unitStats.life;
                targetLife -= atk;
                break;
            default:
                break;
        }
    }
    public void MoveTo(int x, int y)
    {
        Tile tile = BattleGrid.instance.GetTile(x, y);
        if (tile != null && tile.OccupiedUnit == null && tile.Walkable)
        {
            transform.position = tile.transform.position;
        }
    }
    public void Defend(BaseUnit unit) { }
    public void Wait(BaseUnit unit) { }
    public enum Direction
    {
        Up, Down, Left, Right
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && UnitManager.Instance.SelectedHero == this && unitStateMachine.currentState != UnitStateMachine.UnitState.EndTurn)
        {
            yPos++;
            if (BattleGrid.instance.OntheGrid(xPos, yPos))
            {
                MoveTo(xPos, yPos);
                UpdatePosition();
            }
            else
            {
                yPos--;
            }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) && UnitManager.Instance.SelectedHero == this && unitStateMachine.currentState != UnitStateMachine.UnitState.EndTurn)
        {
            yPos--;
            if (BattleGrid.instance.OntheGrid(xPos, yPos))
            {
                MoveTo(xPos, yPos);
                UpdatePosition();
            }
            else
            {
                yPos++;
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) && UnitManager.Instance.SelectedHero == this && unitStateMachine.currentState != UnitStateMachine.UnitState.EndTurn)
        {
            xPos--;
            if (BattleGrid.instance.OntheGrid(xPos, yPos))
            {
                MoveTo(xPos, yPos);
                UpdatePosition();
            }
            else
            {
                xPos++;
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) && UnitManager.Instance.SelectedHero == this && unitStateMachine.currentState != UnitStateMachine.UnitState.EndTurn)
        {
            xPos++;
            if (BattleGrid.instance.OntheGrid(xPos, yPos))
            {
                MoveTo(xPos, yPos);
                UpdatePosition();
            }
            else
            {
                xPos--;
            }
        }

    }
    private void UpdatePosition()
    {
        OccupiedTile.OccupiedUnit = null;
        OccupiedTile.CheckIfCanWalk();
        OccupiedTile = null;
        OccupiedTile = BattleGrid.instance.GetTile(xPos, yPos);
    }
}
