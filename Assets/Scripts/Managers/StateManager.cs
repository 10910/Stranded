using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class StateManager : MonoBehaviour
{
    public PlayerInput input;
    public Movement movement;
    public Goggles goggles;
    public InfoLoader infoLoader;
    public Shooter shooter;
    public string stateName;

    private State _currentState;

    public GroundedState groundedState;
    public MidairState midairState;
    public GogglesState gogglesState;

    void Start()
    {
        groundedState = new GroundedState(this);
        midairState = new MidairState(this);
        _currentState = groundedState;
        _currentState.Enter();
    }

    // Update is called once per frame
    void Update()
    {
        //stateName = _currentState.ToString();
        _currentState.Update();
    }

    public void SetState(State newState){
        if (newState != _currentState) { 
            _currentState.Exit();
            _currentState = newState;
            _currentState.Enter();
        }
    }

    public void FixedUpdate() {
    }
}


