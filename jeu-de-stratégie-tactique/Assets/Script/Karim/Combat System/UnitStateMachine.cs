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
        Attack,
        MoveTo,
        EndTurn,
        Dead,None
    }

    public UnitState currentState;
}