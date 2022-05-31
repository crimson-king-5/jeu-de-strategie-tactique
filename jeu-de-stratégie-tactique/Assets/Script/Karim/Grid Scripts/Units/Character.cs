using System;
using System.Collections;
using System.Collections.Generic;
using Codice.CM.Common;
using Sirenix.OdinInspector;
using Unity.Netcode;
using UnityEngine;

public class Character : TEAM2.Unit
{
    public Vector3Int OccupiedTile;

    public enum Team
    {
        TEAM1 = 0, TEAM2 = 1
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
    public int life { get { return ScrUnit.unitStats.life; } set { ScrUnit.unitStats.life = value; } }
    public int range { get { return ScrUnit.unitStats.range; } set { ScrUnit.unitStats.range = value; } }
    public int atk { get { return ScrUnit.unitStats.atk; } set { ScrUnit.unitStats.atk = value; } }
    public int mv { get { return ScrUnit.unitStats.mv; } set { ScrUnit.unitStats.mv = value; } }

    public UnitStateMachine unitStateMachine = new UnitStateMachine();

    public  void Init()
    {
        ScrUnit = ScrUnit.GetCloneUnit();
    }

    public void Attack(int xPos, int yPos)
    {
        unitStateMachine.currentState = UnitStateMachine.UnitState.Attack;
        Character OccupiedUnit = _gameManager.PlayerManager.CurrentPlayer.GetCharacter(xPos,yPos);
        BattleGridTile targetBattleGridTile;
        int targetLife;
        targetBattleGridTile = _gameManager.BattleGrid.GetTile(xPos, yPos);
        Vector3 effectPos = targetBattleGridTile.transform.GetPosition();
        GameManager.Instance.InstantiateEffect(effectPos, 0);
        targetLife = OccupiedUnit.life;
        Debug.Log("Unit " + OccupiedUnit.ScrUnit.unitsName + " take " + atk + " damage");
        targetLife -= atk;
        Debug.Log("Unit :" + OccupiedUnit.ScrUnit.unitsName + " have " + targetLife + " Life now !");
        OccupiedUnit.life = targetLife;
        unitStateMachine.currentState = UnitStateMachine.UnitState.EndTurn;
    }

    [ClientRpc]
    public void MoveToClientRpc(int x, int y)
    {
        
        BattleGridTile battleGridTile = _gameManager.BattleGrid.GetTile(x, y);
        StartCoroutine(MoveUnit(battleGridTile));
        OccupiedTile = new Vector3Int((int)battleGridTile.transform.GetPosition().x, (int)battleGridTile.transform.GetPosition().y);
    }

    private IEnumerator MoveUnit(BattleGridTile battleGridTile)
    {
        while (transform.position != battleGridTile.transform.GetPosition())
        {
            transform.position = Vector2.MoveTowards(transform.position, battleGridTile.transform.GetPosition(), 5f * Time.deltaTime);
            yield return new WaitForSeconds(0.001f);
        }
    }

    public bool CanMoveTo(int x, int y)
    {
        bool canMove;
        unitStateMachine.currentState = UnitStateMachine.UnitState.MoveTo;
        BattleGridTile battleGridTile = _gameManager.BattleGrid.GetTile(x, y);
        if (battleGridTile != null && battleGridTile.Walkable)
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
    public void Defend(Character unit) { }
    public void Wait(Character unit) { }


    public override void DoAction()
    {
        base.DoAction();
    }

    public override void Die()
    {
        base.Die();
    }

    private void Update()
    {
        #region TMP
        if (Input.GetKeyDown(KeyCode.UpArrow) && _gameManager.UnitManager.SelectedHero == this && unitStateMachine.currentState != UnitStateMachine.UnitState.EndTurn)
        {
            yPos++;
            if (_gameManager.BattleGrid.OntheGrid(xPos, yPos))
            {
                if (CanMoveTo(xPos, yPos))
                {
                    UpdateWalkable();
                }
                else
                {
                    if (_gameManager.BattleGrid.GetTile(xPos, yPos) != null)
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
        else if (Input.GetKeyDown(KeyCode.DownArrow) && _gameManager.UnitManager.SelectedHero == this && unitStateMachine.currentState != UnitStateMachine.UnitState.EndTurn)
        {
            yPos--;
            if (_gameManager.BattleGrid.OntheGrid(xPos, yPos))
            {
                if (CanMoveTo(xPos, yPos))
                {
                    UpdateWalkable();
                }
                else
                {
                    if (_gameManager.BattleGrid.GetTile(xPos, yPos) != null)
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
        else if (Input.GetKeyDown(KeyCode.LeftArrow) && _gameManager.UnitManager.SelectedHero == this && unitStateMachine.currentState != UnitStateMachine.UnitState.EndTurn)
        {
            xPos--;
            if (_gameManager.BattleGrid.OntheGrid(xPos, yPos))
            {
                if (CanMoveTo(xPos, yPos))
                {
                    UpdateWalkable();
                }
                else
                {
                    if (_gameManager.BattleGrid.GetTile(xPos, yPos) != null)
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
        else if (Input.GetKeyDown(KeyCode.RightArrow) && _gameManager.UnitManager.SelectedHero == this && unitStateMachine.currentState != UnitStateMachine.UnitState.EndTurn)
        {
            xPos++;
            if (_gameManager.BattleGrid.OntheGrid(xPos, yPos))
            {
                if (CanMoveTo(xPos, yPos))
                {
                    UpdateWalkable();
                }
                else
                {
                    if (_gameManager.BattleGrid.GetTile(xPos, yPos) != null)
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


        #endregion
    }
    private void UpdateWalkable()
    {
    }
}
