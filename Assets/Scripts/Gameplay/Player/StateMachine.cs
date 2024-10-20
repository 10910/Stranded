using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class StateMachine
{
    public State currentState { get; private set; }
    public StateMachine(State iniState){
        currentState = iniState;
    }

    public void SetState(State newState){
        if(newState != currentState)
        {
            currentState.Exit();
            currentState = newState; 
            newState.Enter();
        }
    }

    public void Update(){
        if (currentState != null) { 
            currentState.Update();
        }
    }
}

