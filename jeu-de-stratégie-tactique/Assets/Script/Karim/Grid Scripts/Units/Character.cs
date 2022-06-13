using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
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

    public float Life { get { return ScrUnit.unitStats.life; } set { ScrUnit.unitStats.life = value; } }
    public int Range { get { return ScrUnit.unitStats.range; } }
    public float Atk { get { return ScrUnit.unitStats.atk; } }
    public float ClassBonus { get => ScrUnit.classBonus; }
    public int Mv { get => ScrUnit.unitStats.mv; set => ScrUnit.unitStats.mv = value; }
    public float Armor { get => ScrUnit.unitStats.armor; set => ScrUnit.unitStats.armor = value; }
    public UnitClass UnitClass { get => ScrUnit.unitUnitClass; }

    private bool hasMoved = false;
    private bool hasAttack = false;


    public void Attack(Character targetCharacter)
    {
        float bonus = 1;
        switch (targetCharacter.UnitClass)
        {
            case UnitClass.Tank:
                if (UnitClass == UnitClass.Mage)
                {
                    bonus = Atk * ClassBonus;
                }
                else if (UnitClass == UnitClass.Warrior)
                {
                    bonus = Atk / ClassBonus;
                }
                break;
            case UnitClass.Mage:
                if (UnitClass == UnitClass.Warrior)
                {
                    bonus = Atk * ClassBonus;
                }
                else if (UnitClass == UnitClass.Tank)
                {
                    bonus = Atk / ClassBonus;
                }
                break;
            case UnitClass.Warrior:
                if (UnitClass == UnitClass.Tank)
                {
                    bonus = Atk * ClassBonus;
                }
                if (UnitClass == UnitClass.Mage)
                {
                    bonus = Atk / ClassBonus;
                }
                break;
        }
        float damage = bonus * Atk;
        float targetLife = targetCharacter.Life;
        float targetArmor = targetCharacter.Armor;
        Debug.Log("Unit " + targetCharacter.ScrUnit.unitsName + " take " + (damage - targetArmor) + " damage");
        targetLife -= damage - targetArmor;
        Debug.Log("Unit :" + targetCharacter.ScrUnit.unitsName + " have " + targetLife + " Life now !");
        targetCharacter.Life = targetLife;
        _gameManager.InstantiateEffect(targetCharacter.GetUnitDestinationWorldPosition(targetCharacter.GetCurrentUnitGridlPosition()), 0);
        targetCharacter.CheckifUnitDie();
    }

    private IEnumerator MoveUnit(Vector3 newUnitPos, float speed)
    {
        while (transform.position != newUnitPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, newUnitPos, speed * Time.deltaTime);
            yield return null;
        }
        OccupiedTileGridPosition = GetSpecificGridPosition(newUnitPos);
    }

    public void Rest()
    {
        SpriteRenderer unitRenderer = GetComponent<SpriteRenderer>();
        unitRenderer.color = Color.gray;
        hasMoved = false;
        hasAttack = false;
        unitStateMachine.currentState = UnitStateMachine.UnitState.EndTurn;
    }

    public override void DoAction()
    {
        base.DoAction();
    }

    public  void CheckifUnitDie()
    {
        if (Life <= 0)
        {
            unitStateMachine.currentState = UnitStateMachine.UnitState.Dead;
            gameObject.SetActive(false);
        }
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

        if (_gameManager.UnitManager.SelectedHero == this)
        {
            if (Input.GetMouseButtonDown(0))
            {
                CharacterMouseEvent();
            }

            if (Input.GetMouseButtonDown(1))
            {
                Rest();
            }
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
        }
    }

    void CharacterMouseEvent()
    {
        if (unitStateMachine.currentState != UnitStateMachine.UnitState.EndTurn)
        {
            Vector3 mouseWorldPosition = BattleGrid.GetMouseWorldPosition();
            Vector3Int gridPos = GetSpecificGridPosition(mouseWorldPosition);
            if (BattleGrid.Tilemap.HasTile(gridPos))
            {
                int tileRange = GetTileRange(mouseWorldPosition);
                BattleGridTile gridTile = (BattleGridTile)BattleGrid.Tilemap.GetTile(gridPos);
                Character mouseCharacter = null;
                if (PlayerManager.CheckifUnitWasHere(GetUnitDestinationGridPosition(mouseWorldPosition)))
                {
                    mouseCharacter = (Character)PlayerManager.GetUnit(GetUnitDestinationGridPosition(mouseWorldPosition));
                    if (mouseCharacter != null && mouseCharacter.ScrUnit.faction != ScrUnit.faction && tileRange <= Range)
                    {
                        unitStateMachine.currentState = UnitStateMachine.UnitState.Attack;
                    }
                }
                else if (tileRange <= Mv && gridTile.Walkable)
                {
                    unitStateMachine.currentState = UnitStateMachine.UnitState.MoveTo;
                }
                switch (unitStateMachine.currentState)
                {
                    case UnitStateMachine.UnitState.Attack:
                        if (!hasAttack)
                        {
                            Attack(mouseCharacter);
                            hasAttack = true;
                            if (hasMoved)
                            {
                                Rest();
                            }
                            if (mouseCharacter.Range <= mouseCharacter.GetTileRange(transform.position))
                            {
                                Attack(this);
                            }
                        }
                        break;
                    case UnitStateMachine.UnitState.MoveTo:
                        if (!hasMoved)
                        {
                            MouseClickMoveTo(mouseWorldPosition);
                            hasMoved = true;
                            if (hasAttack)
                            {
                                Rest();
                            }
                        }
                        break;
                }
            }
        }
    }
}
