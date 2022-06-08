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
    //public enum Team
    //{
    //    TEAM1 = 0, TEAM2 = 1
    //}

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

    public int Life { get { return ScrUnit.unitStats.life; } set { ScrUnit.unitStats.life = value; } }
    public int Range { get { return ScrUnit.unitStats.range; } set { ScrUnit.unitStats.range = value; } }
    public int Atk { get { return ScrUnit.unitStats.atk; } set { ScrUnit.unitStats.atk = value; } }
    public int Mv { get { return ScrUnit.unitStats.mv; } set { ScrUnit.unitStats.mv = value; } }

    public UnitStateMachine unitStateMachine = new UnitStateMachine();

    public void Init()
    {
        ScrUnit = ScrUnit.GetCloneUnit();
    }

    public void Attack(Character targetCharacter)
    {
        int targetLife;
        _gameManager.InstantiateEffect(targetCharacter.GetUnitDestinationWorldPosition(targetCharacter.GetCurrentUnitGridlPosition()), 0);
        targetLife = targetCharacter.Life;
        Debug.Log("Unit " + targetCharacter.ScrUnit.unitsName + " take " + Atk + " damage");
        targetLife -= Atk;
        Debug.Log("Unit :" + targetCharacter.ScrUnit.unitsName + " have " + targetLife + " Life now !");
        targetCharacter.Life = targetLife;
    }

    private IEnumerator MoveUnit(Vector3 newUnitPos, float speed)
    {
        while (transform.position != newUnitPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, newUnitPos, speed * Time.deltaTime);
            yield return null;
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
            CharacterMouseEvent();
        }
    }

    void MouseClickMoveTo(Vector2 mouseWorldPosition)
    {
        Vector3Int gridPos = GetSpecificGridPosition(mouseWorldPosition);
        Vector3 charaDestinationWorldPos = GetUnitDestinationWorldPosition(gridPos);
        Vector3Int charaDestinationGridPos = GetUnitDestinationGridPosition(charaDestinationWorldPos);
        if (BattleGrid.Tilemap.HasTile(gridPos))
        {
            StartCoroutine(MoveUnit(charaDestinationWorldPos, 15));
            OccupiedTileGridPosition = charaDestinationGridPos;
            unitStateMachine.currentState = UnitStateMachine.UnitState.EndTurn;
        }
    }

    void CharacterMouseEvent()
    {
        Vector3 mouseWorldPosition = BattleGrid.GetMouseWorldPosition();
        int tileRange = GetTileRange(mouseWorldPosition);
        Character mouseCharacter = null;
        if (PlayerManager.CheckifUnitWasHere(GetUnitDestinationGridPosition(mouseWorldPosition)))
        {
            mouseCharacter = (Character)PlayerManager.GetUnit(GetUnitDestinationGridPosition(mouseWorldPosition));
            if (mouseCharacter != null && mouseCharacter.ScrUnit.faction != ScrUnit.faction && tileRange <= Range)
            {
                unitStateMachine.currentState = UnitStateMachine.UnitState.Attack;
            }
        }
        else if(tileRange <= Mv)
        {
            unitStateMachine.currentState = UnitStateMachine.UnitState.MoveTo;
        }

        switch (unitStateMachine.currentState)
        {
            case UnitStateMachine.UnitState.Attack:
                Attack(mouseCharacter);
        unitStateMachine.currentState = UnitStateMachine.UnitState.EndTurn; 
                break;
            case UnitStateMachine.UnitState.Defend:
                Defend();
        unitStateMachine.currentState = UnitStateMachine.UnitState.EndTurn; 
                break;
            case UnitStateMachine.UnitState.MoveTo:
                MouseClickMoveTo(mouseWorldPosition);
        unitStateMachine.currentState = UnitStateMachine.UnitState.EndTurn; 
                break;
        }
    }
    private void UpdateWalkable()
    {
    }
}
