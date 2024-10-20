
using UnityEngine.InputSystem;
using UnityEngine;

public class GroundedState : State
{
    public GroundedState(StateManager manager) : base(manager) { 
    }

    public override void Enter()
    {
        //_manager.input.actions.FindActionMap("Player").Enable();
    }

    public override void Exit()
    {
    }

    public override void Update()
    {
        if (_stateManager.movement.isJumpPressed || !_stateManager.movement.characterController.isGrounded) {
            _stateManager.SetState(_stateManager.midairState);
        }
    }

}