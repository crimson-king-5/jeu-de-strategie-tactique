using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UnitStateMachine
{
    public enum UnitState
    {
        Wait,
        Attack,
        Defend,
        EndTurn
    }

    public UnitState currentState;

    public void SetSate(UnitState newState)
    {
        switch (newState)
        {
            case UnitState.Wait:
                currentState = UnitState.EndTurn;
                break;
            case UnitState.Attack:
                currentState = UnitState.EndTurn;
                break;
            case UnitState.Defend:
                currentState = UnitState.EndTurn;
                break;
            default:
                break;
        }

    }
}