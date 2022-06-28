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
    public struct HistoricData
    {
        public Cell cell;
        public Facing facing;

        public HistoricData(Cell c, Facing f)
        {
            cell = c;
            facing = f;
        }
    }


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
    public bool AwaitMoveOrder { get; set; }
    public bool AwaitAttackOrder { get; set; }

    

    private bool hasMoved = false;
    private bool hasAttack = false;
    private bool hasBuild = false;
    private bool canWalkOnCell = false;
    private Cell moveCell;
    private Cell nextPosCell;
    private Cell tmpCell;
    private LineRenderer lr;
    
    private List<Unit> nbsUnits = new List<Unit>();
    private List<Cell> ruins = new List<Cell>();
    private List<HistoricData> historic = new List<HistoricData>();
    private Character toAttack;

    public override void Init(GameManager gm, UnitType unitType)
    {
        base.Init(gm, unitType);
        Cell.OnClickCell += OnClickCell;
        _gameManager.SoundManager.PlaySound(ScrUnit.appearAudioClip);
    }

    private void OnDestroy()
    {
        Cell.OnClickCell -= OnClickCell;
    }

    private void Update()
    {
        if (_gameManager.UnitManager.SelectedHero == this)
        {
            canWalkOnCell = Mv > 0 ? CellOn.CanWalkOnCell() : false;

            if (Input.GetMouseButtonDown(1))
            {
                RewindHisto();
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                Rest();
            }
        }
    }


    public override void OnClick()
    {
        base.OnClick();

        if (_gameManager.UnitManager.SelectedHero == this) CellOn.ShowWalkableCells(PlayerManager.MoveRange);

        canWalkOnCell = false;
    }

    public override void OnSelect()
    {
        //base.OnSelect();
        //Debug.Log(Mv);
        lr = gameObject.AddComponent<LineRenderer>();
        lr.material = _scrUnit.lineRendererMat;
        lr.widthMultiplier = 0.25f;
        lr.positionCount = 1;
        lr.SetPosition(lr.positionCount - 1, CellOn.PosCenter);
        StartCell = CellOn;
        if (ScrUnit.isBuilder) BuilderRuinsAround(CellOn);
    }

    public override bool OnDeselect()
    {
        base.OnDeselect();
        Destroy(lr);
        StartCell.HideWalkableCells(PlayerManager.MoveRange);
        GetComponent<SpriteRenderer>().color = Color.white;
        return true;
    }

    public void Attack(Character targetCharacter)
    {
        switch (_scrUnit.unitUnitClass)
        {
            case UnitClass.Mage:
                OneAttack(targetCharacter);
                break;

            default:
                OneAttack(targetCharacter);
                break;
        }
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

        EndTurn();
    }

    void MoveTile(Cell cell)
    {
        ResetLists();
        historic.Add(new HistoricData(CellOn, facing));
        facing = CheckCellRelativPos(cell);
        tmpCell = cell;
        transform.position = cell.PosCenter;
        CellOn.Contains = null;
        CellOn = cell;
        CellOn.Contains = this;
        lr.positionCount++;
        lr.SetPosition(lr.positionCount - 1, CellOn.PosCenter);
        Mv -= CellOn.Tile.mvRequire;
        Debug.Log(Mv);
        CheckNeighbors();
        CheckRuins(cell);
    }

    void MoveTileForced(Cell cell)
    {
        if (cell.Contains != null) return;
        transform.position = cell.PosCenter;
        CellOn.Contains = null;
        CellOn = cell;
        CellOn.Contains = this;
    }

    void RewindHisto()
    {
        ResetLists();
        if (historic.Count <= 0) return;
        lr.positionCount--;
        HistoricData hs = historic[historic.Count - 1];
        Mv += CellOn.Tile.mvRequire;
        transform.position = hs.cell.PosCenter;
        facing = hs.facing;
        CellOn.Contains = null;
        CellOn = hs.cell;
        CellOn.Contains = this;
        tmpCell = hs.cell;
        historic.RemoveAt(historic.Count - 1);
        CheckNeighbors();
    }

    bool CheckNeighbors()
    {
        nbsUnits = CellOn.CheckNeighbours(this);
        if (nbsUnits.Count > 0)
        {
            foreach (Unit u in nbsUnits) u.GetComponent<SpriteRenderer>().color = Color.magenta;
            return true;
        }
        else return false;
    }

    bool CheckRuins(Cell cell)
    {
        BuilderRuinsAround(CellOn);
        return true;
    }

    Facing CheckCellRelativPos(Cell nextCell)
    {
        Vector2Int v = (Vector2Int)nextCell.Position - (Vector2Int)CellOn.Position;
        if (v == Vector2Int.up) return Facing.NORTH;
        else if (v == Vector2Int.down) return Facing.SOUTH;
        else if (v == Vector2Int.right) return Facing.EAST;
        else if (v == Vector2Int.left) return Facing.WEST;
        else return Facing.NORTH;
    }

    void ResetLists()
    {
        if (ruins.Count > 0)
        {
            foreach (Cell c in ruins) c.ResetColor();
        }
        ruins.Clear();

        if (nbsUnits.Count > 0)
        {
            foreach (Unit u in nbsUnits) u.GetComponent<SpriteRenderer>().color = Color.white;
        }
        nbsUnits.Clear();
    }


    override public void EndTurn()
    {
        ResetLists();
        CellOn.HideWalkableCells(PlayerManager.MoveRange);
        Rest();
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
        if ((cell.Position == CellOn.Position || _gameManager.UnitManager.SelectedHero != this)) return;
        if (canWalkOnCell) MoveTile(cell);
        else 
        {
            Unit search = nbsUnits.Find(x => x == cell.Contains);
            if (search)
            {
                Debug.Log("Unit found: " + search.ScrUnit.name + "  " + (search as Character).CellOn.Position);
                Attack((Character)search);
            }
            else Debug.Log("Unit not found");

            Cell ruin = ruins.Find(x => x == cell);
            if (ruin != null) UIManager.InvokeBuildUI(ruin);
        }
        return;
        
    }

    void OneAttack(Character chara)
    {
        Vector3Int vec = CellOn.Position;
        Vector3Int vec2 = chara.CellOn.Position;
        switch (vec2 - vec)
        {
            case var value when value == Vector3Int.up:
                chara.MoveTileForced(chara.CellOn._Neighbors.top);
                MoveTileForced(CellOn._Neighbors.bottom);
                break;

            case var value when value == Vector3Int.down:
                chara.MoveTileForced(chara.CellOn._Neighbors.bottom);
                MoveTileForced(CellOn._Neighbors.top);
                break;

            case var value when value == Vector3Int.left:
                chara.MoveTileForced(chara.CellOn._Neighbors.left);
                MoveTileForced(CellOn._Neighbors.right);
                break;

            case var value when value == Vector3Int.right:
                chara.MoveTileForced(chara.CellOn._Neighbors.right);
                MoveTileForced(CellOn._Neighbors.left);
                break;
        }
    }



    #region UnusedButKeeping

    //private void Update()
    //{
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
    //}

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

    #endregion

}
