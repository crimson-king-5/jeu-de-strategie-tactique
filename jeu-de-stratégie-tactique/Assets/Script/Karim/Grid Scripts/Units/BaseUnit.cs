using System;
using System.Collections;
using System.Collections.Generic;
using Codice.CM.Common;
using Sirenix.OdinInspector;
using Unity.Netcode;
using UnityEngine;

public class BaseUnit : MonoBehaviour
{
    public Tile OccupiedTile; 
    public ScriptableUnit scriptableUnit;

    public enum Team
    {
        TEAM1 = 0,TEAM2 = 1
    }

    //public Team currentTeam
    //{
    //    get
    //    {
    //        if (NetworkManager.IsClient)
    //        {
    //            return Team.TEAM2;
    //        }
    //        return Team.TEAM1;
    //    }
    //}

    public int xPos;
    public int yPos;
    public int life { get { return scriptableUnit.unitStats.life; }set { scriptableUnit.unitStats.life = value; }}
    public int range { get { return scriptableUnit.unitStats.range; } set { scriptableUnit.unitStats.range = value; } }
    public int atk { get { return scriptableUnit.unitStats.atk; } set { scriptableUnit.unitStats.atk = value; } }
    public int mv { get { return scriptableUnit.unitStats.mv; } set { scriptableUnit.unitStats.mv = value; } }

    public UnitStateMachine unitStateMachine = new UnitStateMachine();

    public void Attack(int xPos, int yPos)
    {
        unitStateMachine.currentState = UnitStateMachine.UnitState.Attack;
        Tile targetTile;
        int targetLife;
        targetTile = BattleGrid.instance.GetTile(xPos, yPos);
        targetLife = targetTile.OccupiedUnit.scriptableUnit.unitStats.life;
        Debug.Log("Unit " + targetTile.OccupiedUnit.scriptableUnit.unitsName + " take " + atk + " damage");
        targetLife -= atk;
        Debug.Log("Unit :" + targetTile.OccupiedUnit.scriptableUnit.unitsName + " have " + targetLife + " Life now !");
        targetTile.OccupiedUnit.life = targetLife;
        unitStateMachine.currentState = UnitStateMachine.UnitState.EndTurn;
    }

    [ClientRpc]
    public void MoveToClientRpc(int x, int y)
    {
        Tile tile = BattleGrid.instance.GetTile(x, y);
        transform.position = tile.transform.position;
        OccupiedTile.OccupiedUnit = null;
        OccupiedTile = tile;
        OccupiedTile.OccupiedUnit = this;
    }

    public bool CanMoveTo(int x, int y)
    {
        bool canMove;
        unitStateMachine.currentState = UnitStateMachine.UnitState.MoveTo;
        Tile tile = BattleGrid.instance.GetTile(x, y);
        if (tile != null && tile.Walkable)
        {
            MoveToClientRpc(x, y);
            canMove = true;
        }
        else
        {
            canMove = false;
        }
        unitStateMachine.currentState = UnitStateMachine.UnitState.EndTurn;
        return canMove;
    }
    public void Defend(BaseUnit unit) { }
    public void Wait(BaseUnit unit) { }

    void Start()
    {
        scriptableUnit = scriptableUnit.GetCloneUnit();
    }

    private void Update()
    {
        //if (IsLocalPlayer)
        //{
            if (Input.GetKeyDown(KeyCode.DownArrow) && UnitManager.Instance.SelectedHero == this && unitStateMachine.currentState != UnitStateMachine.UnitState.EndTurn)
            {
                yPos++;
                if (BattleGrid.instance.OntheGrid(xPos, yPos))
                {
                    if (CanMoveTo(xPos, yPos))
                    {
                        UpdateWalkable();
                    }
                    else
                    {
                        if (BattleGrid.instance.GetTile(xPos, yPos).OccupiedUnit != null)
                        {
                            Attack(xPos, yPos);
                        }

                        yPos--;
                    }
                }
                else
                {
                    yPos--;
                }
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow) && UnitManager.Instance.SelectedHero == this &&
                     unitStateMachine.currentState != UnitStateMachine.UnitState.EndTurn)
            {
                yPos--;
                if (BattleGrid.instance.OntheGrid(xPos, yPos))
                {
                    if (CanMoveTo(xPos, yPos))
                    {
                        UpdateWalkable();
                    }
                    else
                    {
                        if (BattleGrid.instance.GetTile(xPos, yPos).OccupiedUnit != null)
                        {
                            Attack(xPos, yPos);
                        }

                        yPos++;
                    }
                }
                else
                {
                    yPos++;
                }
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow) && UnitManager.Instance.SelectedHero == this &&
                     unitStateMachine.currentState != UnitStateMachine.UnitState.EndTurn)
            {
                xPos--;
                if (BattleGrid.instance.OntheGrid(xPos, yPos))
                {
                    if (CanMoveTo(xPos, yPos))
                    {
                        UpdateWalkable();
                    }
                    else
                    {
                        if (BattleGrid.instance.GetTile(xPos, yPos).OccupiedUnit != null)
                        {
                            Attack(xPos, yPos);
                        }

                        xPos++;
                    }
                }
                else
                {
                    xPos++;
                }
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) && UnitManager.Instance.SelectedHero == this &&
                     unitStateMachine.currentState != UnitStateMachine.UnitState.EndTurn)
            {
                xPos++;
                if (BattleGrid.instance.OntheGrid(xPos, yPos))
                {
                    if (CanMoveTo(xPos, yPos))
                    {
                        UpdateWalkable();
                    }
                    else
                    {
                        if (BattleGrid.instance.GetTile(xPos, yPos).OccupiedUnit != null)
                        {
                            Attack(xPos, yPos);
                        }

                        xPos--;
                    }
                }
                else
                {
                    xPos--;
                }
            }
        //}
    }
    private void UpdateWalkable()
    {
        OccupiedTile.CheckIfCanWalk();
    }
}
