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
        Attack = 0,
        MoveTo = 1,
        EndTurn = 2,
        Build = 3,
        Dead = 4,None = 5
    }

    public UnitState currentState;
}