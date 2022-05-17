using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStateMachine
{
    public enum UnitState
    {
        MoveTo, Wait, Attack, Defend
    }

    public UnitState currentState;

    public void SetSate(UnitState newState, BaseUnit unit)
    {
        switch (newState)
        {
            case UnitState.MoveTo:
                MoveTo(unit);
                break;
            case UnitState.Wait:
                Wait(unit);
                break;
            case UnitState.Attack:
                Attack(unit);
                break;
            case UnitState.Defend:
                Defend(unit);
                break;
            default:
                break;
        }
    }
    public void Attack(BaseUnit unitTarget) { 
        currentState = UnitState.Wait;
    }
    public void MoveTo(BaseUnit unit) { 

        currentState = UnitState.Wait; 
    }
    public void Defend(BaseUnit unit) { currentState = UnitState.Wait; }
    public void Wait(BaseUnit unit) { currentState = UnitState.Wait; }

}
