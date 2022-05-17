using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStateMachine 
{
    public enum UnitState
    {
        MoveTo,Wait,Attack,Defend
    }

    public UnitState currentState;

  public void SetSate(UnitState newState) {
        switch (newState)
        {
            case UnitState.MoveTo:
                MoveTo();
                break;
            case UnitState.Wait:
                Wait();
                break;
            case UnitState.Attack:
                Attack();
                break;
            case UnitState.Defend:
                Defend();
                break;
            default:
                break;
        }
    }
    public void Attack() { currentState = UnitState.Wait; }
    public void MoveTo() { currentState = UnitState.Wait; }
    public void Defend(){ currentState = UnitState.Wait; }
    public void Wait(){ currentState = UnitState.Wait; }

}
