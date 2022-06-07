using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UnitStateMachine
{
    public UnitStateMachine()
    {
        currentState = UnitState.None;
    }
    public enum UnitState
    {
        Wait = 0,
        Attack = 1,
        Defend = 2,
        MoveTo = 3,
        Build = 4,
        EndTurn = 5,
        Dead = 6,None = 7
    }

    public UnitState currentState;
}