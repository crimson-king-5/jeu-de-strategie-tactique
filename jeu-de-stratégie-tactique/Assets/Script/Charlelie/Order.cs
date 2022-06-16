using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TEAM2;

public class Order : MonoBehaviour
{
    OrderType _orderType;
    protected Player player;
    protected Character chara;

    public OrderType type
    {
        get { return _orderType; }
    }

    public Order() { Set(); }

    public Order(OrderType type)
    {
        _orderType = type;
        GameManager.Instance.PlayerManager.CurrentPlayer.CurrentOrder = this;
    }


    public virtual void ApplyOrder() { }

    public virtual void Set() { }

}

public class MoveOrder : Order
{
    Vector3Int _currTile;
    Vector3Int _nextTile;

    public override void Set()
    {
        chara = GameManager.Instance.UnitManager.SelectedHero;
        _nextTile = TileSelector.instance.SelectedTileVec;
    }


    public override void ApplyOrder()
    {
        base.ApplyOrder();
    }


    public static void SetMove()
    {
        if (!TileSelector.instance.SelectTile)
        {
            TileSelector.instance.SelectTile = true;
            TileSelector.instance.currType = OrderType.WALK;
        }
    }

}

public class BuildOrder : Order
{

    public override void Set()
    {

    }
    public override void ApplyOrder()
    {
        base.ApplyOrder();
    }

    public static void SetBuild()
    {
        UIManager.Instance.CreateOrder(OrderType.BUILD);
    }
}

public class DefendOrder : Order
{

    public override void Set()
    {
        chara = GameManager.Instance.UnitManager.SelectedHero;
    }
    public override void ApplyOrder()
    {
        base.ApplyOrder();
    }

    public static void SetDefend()
    {
        GameManager.Instance.StartCoroutine(Select());
    }

    static IEnumerator Select()
    {
        yield return new WaitUntil(() => GameManager.Instance.UnitManager.SelectedHero != null);
        UIManager.Instance.CreateOrder(OrderType.DEFEND);
    }
}

public class AttackOrder : Order
{
    List<Character> charaToAttack;

    public override void Set()
    {

    }
    public override void ApplyOrder()
    {
        base.ApplyOrder();
    }

    public static void SetAttack()
    {
        UIManager.Instance.CreateOrder(OrderType.ATTACK);
    }
}