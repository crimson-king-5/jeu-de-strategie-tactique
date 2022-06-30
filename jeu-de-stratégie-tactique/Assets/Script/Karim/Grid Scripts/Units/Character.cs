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
    private bool isSelected = false;
    private Cell moveCell;
    private Cell nextPosCell;
    private Cell tmpCell;
    private LineRenderer lr;

    private List<Unit> nbsUnits = new List<Unit>();
    private List<Cell> ruins = new List<Cell>();
    private List<HistoricData> historic = new List<HistoricData>();
    private Character toAttack;
    private List<Character> setupAttChars = new List<Character>();
    private Character enemyClicked;

    public override void Init(GameManager gm, UnitType unitType)
    {
        base.Init(gm, unitType);
        Cell.OnClickCell += OnClickCell;
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

        if (_gameManager.UnitManager.SelectedHero == this && !isSelected)
        {
            isSelected = true;
            CellOn.ShowWalkableCells(PlayerManager.MoveRange);
        }

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
        isSelected = false;
        return true;
    }

    public void Attack(Character targetCharacter, int baseDamage)
    {
        float bonus = 1;
        /*
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
        */
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
        Debug.Log(facing);
        switch (facing)
        {
            case Facing.NORTH:
                transform.rotation = UnityEngine.Quaternion.Euler(0, 0, 0);
                break;

            case Facing.SOUTH:
                transform.rotation = UnityEngine.Quaternion.Euler(0, 0, 180);
                break;

            case Facing.EAST:
                transform.rotation = UnityEngine.Quaternion.Euler(0, 0, -90);
                break;

            case Facing.WEST:
                transform.rotation = UnityEngine.Quaternion.Euler(0, 0, 90);
                break;
        }
        Mv -= CellOn.Tile.mvRequire;
        Debug.Log(Mv);
        ChooseAttack(true);
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
        switch (facing)
        {
            case Facing.NORTH:
                transform.eulerAngles = Vector3.up;
                break;

            case Facing.SOUTH:
                transform.eulerAngles = Vector3.down;
                break;

            case Facing.EAST:
                transform.eulerAngles = Vector3.left;
                break;

            case Facing.WEST:
                transform.eulerAngles = Vector3.right;
                break;
        }
        historic.RemoveAt(historic.Count - 1);
        CheckNeighbors();
    }

    void ChooseAttack(bool isSetup)
    {
        if (isSetup)
        {
            switch (ScrUnit.unitUnitClass)
            {
                case UnitClass.ASSASSIN:
                    AssassinAttackSetup();
                    break;

                case UnitClass.GARDIEN:
                    GardienAttackSetup();
                    break;

                case UnitClass.ELEMENTAIRE:
                    ElementaireAttackSetup();
                    break;

                case UnitClass.TEMPETE:
                    TempeteAttackSetup();
                    break;

                case UnitClass.FAUCHEUR:
                    FaucheurAttackSetup();
                    break;

            }
        }
        else
        {
            switch (ScrUnit.unitUnitClass)
            {
                case UnitClass.ASSASSIN:
                    AssassinAttack();
                    break;

                case UnitClass.GARDIEN:
                    GardienAttack();
                    break;

                case UnitClass.ELEMENTAIRE:
                    ElementaireAttack();
                    break;

                case UnitClass.TEMPETE:
                    TempeteAttack();
                    break;

                case UnitClass.FAUCHEUR:
                    FaucheurAttack();
                    break;

            }
        }
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

    void SetupChars()
    {
        foreach (Character c in setupAttChars)
        {
            c.GetComponent<SpriteRenderer>().color = Color.magenta;
        }
    }

    void ResetSetup()
    {
        foreach (Character c in setupAttChars)
        {
            c.GetComponent<SpriteRenderer>().color = Color.white;
        }
        setupAttChars.Clear();
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
        StartCell.HideWalkableCells(PlayerManager.MoveRange);
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
    public void OnBuild()
    {
        Rest();
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
            Unit search = setupAttChars.Find(x => x == cell.Contains);
            if (search)
            {
                Debug.Log("Unit found: " + search.ScrUnit.name + "  " + (search as Character).CellOn.Position);
                ChooseAttack(false);//Attack((Character)search);
            }
            else Debug.Log("Unit not found");

            Cell ruin = ruins.Find(x => x == cell);
            if (ruin != null && Builder) UIManager.InvokeBuildUI(ruin);
        }
        return;

    }

    void AssassinAttackSetup()
    {
        Cell c = CellOn;
        for (int i = 0; i < 3; i++)
        {
            switch (facing)
            {
                case Facing.NORTH:
                    c = CellOn._Neighbors.top;
                    break;

                case Facing.SOUTH:
                    c = CellOn._Neighbors.bottom;
                    break;

                case Facing.EAST:
                    c = CellOn._Neighbors.right;
                    break;

                case Facing.WEST:
                    c = CellOn._Neighbors.left;
                    break;
            }
            if (c.Contains != null && (c.Contains as Character) && c.Contains.ScrUnit.faction != ScrUnit.faction) setupAttChars.Add(c.Contains as Character);
        }
        SetupChars();
    }


    void AssassinAttack()
    {
        Character chara = setupAttChars[0];
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
        Attack(chara, 2);
    }

    void GardienAttackSetup()
    {
        Cell c = CellOn;
        for (int i = 0; i < 3; i++)
        {
            switch (facing)
            {
                case Facing.NORTH:
                    c = CellOn._Neighbors.top;
                    break;

                case Facing.SOUTH:
                    c = CellOn._Neighbors.bottom;
                    break;

                case Facing.EAST:
                    c = CellOn._Neighbors.right;
                    break;

                case Facing.WEST:
                    c = CellOn._Neighbors.left;
                    break;
            }
            if (c.Contains != null && (c.Contains as Character) && c.Contains.ScrUnit.faction != ScrUnit.faction) setupAttChars.Add(c.Contains as Character);
        }
        SetupChars();
    }

    void GardienAttack()
    {
        Character chara = setupAttChars[0];
        switch (facing)
        {
            case Facing.NORTH:
                chara.MoveTileForced(CellOn._Neighbors.top);
                break;

            case Facing.SOUTH:
                chara.MoveTileForced(CellOn._Neighbors.bottom);
                break;

            case Facing.EAST:
                chara.MoveTileForced(CellOn._Neighbors.right);
                break;

            case Facing.WEST:
                chara.MoveTileForced(CellOn._Neighbors.left);
                break;
        }
        Attack(chara, 2);
    }

    void ElementaireAttackSetup()
    {
        setupAttChars = CellOn.CheckNeighboursWithRange(this, 6);
        SetupChars();
    }

    void ElementaireAttack()
    {
        Attack(enemyClicked, 2);//List<Unit> units = CellOn.CheckNeighboursWithRange(this, 6);
    }

    void TempeteAttackSetup()
    {
        setupAttChars = CellOn.CheckNeighboursWithRange(this, 5);
        SetupChars();
    }

    void TempeteAttack()
    {
        List<Character> tmp = new List<Character>();
        if (enemyClicked.CellOn._Neighbors.top.Contains != null && (enemyClicked.CellOn._Neighbors.top.Contains as Character).ScrUnit.faction != ScrUnit.faction) tmp.Add(enemyClicked.CellOn._Neighbors.top.Contains as Character);
        if (enemyClicked.CellOn._Neighbors.bottom.Contains != null && (enemyClicked.CellOn._Neighbors.bottom.Contains as Character).ScrUnit.faction != ScrUnit.faction) tmp.Add(enemyClicked.CellOn._Neighbors.bottom.Contains as Character);
        if (enemyClicked.CellOn._Neighbors.left.Contains != null && (enemyClicked.CellOn._Neighbors.left.Contains as Character).ScrUnit.faction != ScrUnit.faction) tmp.Add(enemyClicked.CellOn._Neighbors.left.Contains as Character);
        if (enemyClicked.CellOn._Neighbors.right.Contains != null && (enemyClicked.CellOn._Neighbors.right.Contains as Character).ScrUnit.faction != ScrUnit.faction) tmp.Add(enemyClicked.CellOn._Neighbors.right.Contains as Character);
        Attack(enemyClicked, 1);
        for (int i = 0; i < tmp.Count; i++)
        {
            Attack(tmp[i], 1);
        }
    }

    void FaucheurAttackSetup()
    {
        Cell one = null;
        Cell two = null;
        Cell three = null;
        switch (facing)
        {
            case Facing.NORTH:
                one = CellOn._Neighbors.tl;
                two = CellOn._Neighbors.top;
                three = CellOn._Neighbors.tr;
                break;

            case Facing.SOUTH:
                one = CellOn._Neighbors.bl;
                two = CellOn._Neighbors.bottom;
                three = CellOn._Neighbors.br;
                break;

            case Facing.EAST:
                one = CellOn._Neighbors.tr;
                two = CellOn._Neighbors.right;
                three = CellOn._Neighbors.br;
                break;

            case Facing.WEST:
                one = CellOn._Neighbors.tl;
                two = CellOn._Neighbors.left;
                three = CellOn._Neighbors.bl;
                break;
        }
        if (one.Contains != null && (one.Contains as Character).ScrUnit.faction != ScrUnit.faction) setupAttChars.Add(one.Contains as Character);
        if (two.Contains != null && (two.Contains as Character).ScrUnit.faction != ScrUnit.faction) setupAttChars.Add(two.Contains as Character);
        if (three.Contains != null && (three.Contains as Character).ScrUnit.faction != ScrUnit.faction) setupAttChars.Add(three.Contains as Character);
    }

    void FaucheurAttack()
    {
        if (setupAttChars.Count >= 2)
        {
            for (int i = 0; i < setupAttChars.Count; i++)
            {
                Attack(setupAttChars[i], 1);
            }
        }
        else if (setupAttChars.Count == 1)
        {
            Character c = setupAttChars[0];
            switch (c.facing)
            {
                case Facing.NORTH:
                    c.MoveTileForced(c.CellOn._Neighbors.bottom);
                    break;

                case Facing.SOUTH:
                    c.MoveTileForced(c.CellOn._Neighbors.top);
                    break;

                case Facing.EAST:
                    c.MoveTileForced(c.CellOn._Neighbors.left);
                    break;

                case Facing.WEST:
                    c.MoveTileForced(c.CellOn._Neighbors.right);
                    break;
            }
            Attack(c, 3);
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
