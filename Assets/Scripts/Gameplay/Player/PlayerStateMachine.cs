using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateMachine : StateMachine
{
    PlayerInput input;
    public PlayerStateMachine(State iniState, PlayerInput playerInput) : base(iniState){
        
    }
}