using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Order
{
    OrderType _orderType;


    public OrderType type
    {
        get { return _orderType; }
    }

    public Order(OrderType type)
    {
        _orderType = type;
    }

    public Order() { }

}

public class MoveOrder : Order
{

}

public class BuildOrder : Order
{

}

public class DefendOrder : Order
{

}