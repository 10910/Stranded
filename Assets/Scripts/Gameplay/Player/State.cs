using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    protected StateManager _stateManager;
    public abstract void Enter();
    public abstract void Exit();
    public abstract void Update();


    public State(StateManager stateManager) {
        _stateManager = stateManager;
    }

}

//public enum PlayerState
//{
//    Dead,
//    Grounded,
//    Jumping,
//    OnGoggles,
//    OnShooter,
//    OnInfoLoader,
//    Flying,

//}
public enum GameState
{
    StartMenu,
    Playing,
    Pause
}