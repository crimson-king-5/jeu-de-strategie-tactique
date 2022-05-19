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
        Wait,
        Attack,
        Defend,
        MoveTo,
        EndTurn,
        Dead,None
    }

    public UnitState currentState;
}