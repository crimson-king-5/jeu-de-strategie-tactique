using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Sirenix.OdinInspector;
using TEAM2;
using Unity.Netcode;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class Character : Unit
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

    public Builder Builder { get => GetComponent<Builder>(); }
    public float Life { get { return ScrUnit.unitStats.life; } set { ScrUnit.unitStats.life = value; } }
    public int Range { get { return ScrUnit.unitStats.range; } }
    public float Atk { get { return ScrUnit.unitStats.atk; } }
    public float ClassBonus { get => ScrUnit.classBonus; }
    public int Mv { get => ScrUnit.unitStats.mv; set => ScrUnit.unitStats.mv = value; }
    public float Armor { get => ScrUnit.unitStats.armor; set => ScrUnit.unitStats.armor = value; }
    public UnitClass UnitClass { get => ScrUnit.unitUnitClass; }
    public UIManager UIManager { get => _gameManager.UIManager; }
    public bool HasBuild { set => hasBuild = value; }
    public bool HasMoved { get => hasMoved; }
    public bool HasBeenUsed { get; set; }
    public bool AwaitMoveOrder { get; set; }
    public bool AwaitAttackOrder { get; set; }
    public Cell CellOn { get; set; }

    private bool hasMoved = false;
    private bool hasAttack = false;
    private bool hasBuild = false;
    private bool canWalkOnCell = false;
    private Cell moveCell;
    private Cell nextPosCell;
    private List<Unit> nbsUnits;
    private List<Cell> ruins;
    private Character toAttack;

    public override void Init(GameManager gm, UnitType unitType)
    {
        base.Init(gm, unitType);
        Cell.OnClickCell += OnClickCell;
    }

    private void OnDestroy()
    {
        Cell.OnClickCell -= OnClickCell;
    }

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
        UIManager.InvokeInformation("Unit " + targetCharacter.ScrUnit.unitsName + " take " + (damage - targetArmor) + " damage");
        targetLife -= damage - targetArmor;
        UIManager.InvokeInformation("Unit :" + targetCharacter.ScrUnit.unitsName + " have " + targetLife + " Life now !");
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
        CellOn.ResetColor();
        CellOn.Contains = null;
        CellOn = null;
        if (moveCell != null)
        {
            transform.position = moveCell.PosCenter;
            CellOn = moveCell;
        }
        CellOn.Contains = this;
        if (toAttack) Attack(toAttack);
        _gameManager.UnitManager.DeselectUnit();
        HasBeenUsed = true;
        Rest();
    }

    public void CheckifUnitDie()
    {
        if (Life <= 0)
        {
            unitStateMachine.currentState = UnitStateMachine.UnitState.Dead;
            gameObject.SetActive(false);
            PlayerManager.GetPlayerPerFaction(_scrUnit.faction).Units.Remove(this);
            UIManager.InvokeUpdateUI();
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
                //CharacterMouseEvent();

            }

            if (AwaitMoveOrder)
                canWalkOnCell = CellOn.CanWalkOnCell(PlayerManager.MoveRange);

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

    public override void OnClick()
    {
        base.OnClick();
        if (HasBeenUsed) return;
        if (_scrUnit.faction != PlayerManager.CurrentPlayer.PlayerFaction) return;
        if (_scrUnit.faction == PlayerManager.CurrentPlayer.PlayerFaction &&
                unitStateMachine.currentState != UnitStateMachine.UnitState.EndTurn && _gameManager.UnitManager.CanSelectUnit)
            _gameManager.UnitManager.SelectUnit(this);

        if (!AwaitAttackOrder)
        {
            if (_gameManager.UnitManager.SelectedHero == this) CellOn.ShowWalkableCells(PlayerManager.MoveRange);
            else Debug.Log(_gameManager.UnitManager.SelectedHero);
            AwaitMoveOrder = true;
        }
        
        canWalkOnCell = false;
    }

    public override void OnSelect()
    {
        base.OnSelect();
        if (ScrUnit.isBuilder) BuilderRuinsAround(CellOn);
    }

    public override bool OnDeselect()
    {
        //base.OnDeselect();
        if (!AwaitAttackOrder) CellOn.HideWalkableCells(PlayerManager.MoveRange);
        AwaitMoveOrder = false;
        canWalkOnCell = false; 
        if (AwaitAttackOrder) return false;
        if (moveCell != null)
        {
            moveCell.ResetColor();
            moveCell = null;
            nextPosCell = null;
        }
        return true;
    }

    void BuilderRuinsAround(Cell cell)
    {
        ruins = cell.CheckForRuin();
        if (ruins.Count > 0)
            for (int i = 0; i < ruins.Count; i++)
            {
                ruins[i].SetColor(Color.magenta);
            }
    }

    void OnClickCell(Cell cell)
    {
        if (!AwaitAttackOrder && (cell.Position == CellOn.Position || _gameManager.UnitManager.SelectedHero != this)) return;
        Cell ruin = ruins.Find(x => x == cell);
        if (ruin != null) Debug.Log("YOU CLICKED ON AN AVAILABLE RUIN");
        if (canWalkOnCell && AwaitMoveOrder)
        {
            moveCell = cell;
            AwaitMoveOrder = false;
            CellOn.HideWalkableCells(PlayerManager.MoveRange);
            cell.SetColor(Color.gray);
            nextPosCell = cell;
            BuilderRuinsAround(nextPosCell);
            nbsUnits = nextPosCell.CheckNeighbours(this);
            if (nbsUnits.Count > 0)
            {
                foreach (Unit u in nbsUnits) u.GetComponent<SpriteRenderer>().color = Color.magenta;
                AwaitAttackOrder = true;
                AwaitMoveOrder = false;
                _gameManager.UnitManager.CanSelectUnit = false;
            }
            return;
        } else if (AwaitAttackOrder)
        {
            #region ATTACK
            Debug.Log("Search Unit...");
            Unit search = nbsUnits.Find(x => x == cell.Contains);
            if (search)
            {
                Debug.Log("Unit found: " + search.ScrUnit.name + "  " +  (search as Character).CellOn.Position);
                foreach (Unit u in nbsUnits) 
                { 
                    if (u != search)
                        u.GetComponent<SpriteRenderer>().color = Color.white;
                }
                toAttack = (Character)search;
            } else Debug.Log("Unit not found");
            #endregion
        }
    }

    void CharacterMouseEvent()
    {
        
        /*
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
                else if (gridTile.currentTileType == BattleGridTile.TileType.Ruin && ScrUnit.isBuilder && tileRange <= Range)
                {
                    unitStateMachine.currentState = UnitStateMachine.UnitState.Build;
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
                    case UnitStateMachine.UnitState.Build:
                        if (!hasBuild)
                        {
                            UIManager.InvokeBuildUI(UIManager.UnitBuildUI);
                        }
                        break;
                }
            }
        }
        */
    }
        
}
