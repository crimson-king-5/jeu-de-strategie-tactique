using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Codice.CM.Common;
using Sirenix.OdinInspector;
using Unity.Netcode;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

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

    public int life { get { return ScrUnit.unitStats.life; } set { ScrUnit.unitStats.life = value; } }
    public int range { get { return ScrUnit.unitStats.range; } set { ScrUnit.unitStats.range = value; } }
    public int atk { get { return ScrUnit.unitStats.atk; } set { ScrUnit.unitStats.atk = value; } }
    public int mv { get { return ScrUnit.unitStats.mv; } set { ScrUnit.unitStats.mv = value; } }

    public UnitStateMachine unitStateMachine = new UnitStateMachine();

    public void Init()
    {
        ScrUnit = ScrUnit.GetCloneUnit();
    }

    public void Attack()
    {
        unitStateMachine.currentState = UnitStateMachine.UnitState.Attack;
        Character OccupiedUnit = _gameManager.PlayerManager.CurrentPlayer.GetCharacter(xPos, yPos);
        BattleGridTile targetBattleGridTile;
        int targetLife;
        targetBattleGridTile = BattleGrid.GetTileType(xPos, yPos);
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

        Vector3Int battleGridTile = BattleGrid.GetTilePosition(x, y);
        StartCoroutine(MoveUnit(battleGridTile,25f));
        OccupiedTile = new Vector3Int(battleGridTile.x, battleGridTile.y);
    }

    private IEnumerator MoveUnit(Vector3Int newUnitPos,float speed)
    {
        while (transform.position != newUnitPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, newUnitPos, speed * Time.deltaTime);
            yield return new WaitForSeconds(0f);
        }
    }

    public void Defend() { }
    public void Wait() { }


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
        //if (Input.GetKeyDown(KeyCode.UpArrow) && _gameManager.UnitManager.SelectedHero == this && unitStateMachine.currentState != UnitStateMachine.UnitState.EndTurn)
        //{
        //    yPos++;
        //    if (BattleGrid.OntheGrid(xPos, yPos))
        //    {
        //        if (CanMoveTo(xPos, yPos))
        //        {
        //            UpdateWalkable();
        //        }
        //        else
        //        {
        //            if (BattleGrid.GetTileType(xPos, yPos) != null)
        //            {
        //                Attack(xPos, yPos);
        //            }

        //            yPos--;
        //        }
        //    }
        //    else
        //    {
        //        yPos--;
        //    }
        //}
        //else if (Input.GetKeyDown(KeyCode.DownArrow) && _gameManager.UnitManager.SelectedHero == this && unitStateMachine.currentState != UnitStateMachine.UnitState.EndTurn)
        //{
        //    yPos--;
        //    if (BattleGrid.OntheGrid(xPos, yPos))
        //    {
        //        if (CanMoveTo(xPos, yPos))
        //        {
        //            UpdateWalkable();
        //        }
        //        else
        //        {
        //            if (BattleGrid.GetTileType(xPos, yPos) != null)
        //            {
        //                Attack(xPos, yPos);
        //            }

        //            yPos++;
        //        }
        //    }
        //    else
        //    {
        //        yPos++;
        //    }
        //}
        //else if (Input.GetKeyDown(KeyCode.LeftArrow) && _gameManager.UnitManager.SelectedHero == this && unitStateMachine.currentState != UnitStateMachine.UnitState.EndTurn)
        //{
        //    xPos--;
        //    if (BattleGrid.OntheGrid(xPos, yPos))
        //    {
        //        if (CanMoveTo(xPos, yPos))
        //        {
        //            UpdateWalkable();
        //        }
        //        else
        //        {
        //            if (BattleGrid.GetTileType(xPos, yPos) != null)
        //            {
        //                Attack(xPos, yPos);
        //            }

        //            xPos++;
        //        }
        //    }
        //    else
        //    {
        //        xPos++;
        //    }
        //}
        //else if (Input.GetKeyDown(KeyCode.RightArrow) && _gameManager.UnitManager.SelectedHero == this && unitStateMachine.currentState != UnitStateMachine.UnitState.EndTurn)
        //{
        //    xPos++;
        //    if (BattleGrid.OntheGrid(xPos, yPos))
        //    {
        //        if (CanMoveTo(xPos, yPos))
        //        {
        //            UpdateWalkable();
        //        }
        //        else
        //        {
        //            if (BattleGrid.GetTileType(xPos, yPos) != null)
        //            {
        //                Attack(xPos, yPos);
        //            }

        //            xPos--;
        //        }
        //    }
        //    else
        //    {
        //        xPos--;
        //    }
        //}


        #endregion

        if (Input.GetMouseButtonDown(0) && _gameManager.UnitManager.SelectedHero == this)
        {
            MouseClickPosition();
        }
    }

    void MouseClickPosition()
    {
        Vector2 mousePos = BattleGrid.GetMouseWorldPosition();
        Vector3Int gridPos = BattleGrid.Tilemap.WorldToCell(mousePos);
        Vector3Int unitPos = new Vector3Int((int)transform.position.x, (int)transform.position.y);
        int moveRange = (int)Vector3Int.Distance(gridPos, unitPos);
        if (BattleGrid.Tilemap.HasTile(gridPos)&& moveRange <= mv)
        {
            StartCoroutine(MoveUnit(gridPos,15));
            OccupiedTile = new Vector3Int(gridPos.x, gridPos.y);
            unitStateMachine.currentState = UnitStateMachine.UnitState.EndTurn;
        }
        else
        {
            Debug.LogError("out of range !");
        }
    }

    void MouseEvent()
    {
        switch (unitStateMachine.currentState)
        {
            case UnitStateMachine.UnitState.Attack:
                Attack();
                break;
            case UnitStateMachine.UnitState.Defend:
                Defend();
                break;
            case UnitStateMachine.UnitState.MoveTo:
                MouseClickPosition();
                break;
        }
    }
    private void UpdateWalkable()
    {
    }
}
