
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
        if (_stateManager.movement.velocity != 0 || !_stateManager.movement.characterController.isGrounded) {
            _stateManager.SetState(_stateManager.midairState);
        }else{
            Movement movement = _stateManager.movement;
            movement.moveDelta = movement.transform.right * movement.moveDir.x + movement.transform.forward * movement.moveDir.y;
            float speed;
            if (movement.isRunPressed) {
                speed = movement.runSpeed;
            }
            else if (movement.isCrouching) {
                speed = movement.crouchSpeed;
            }
            else {
                speed = movement.walkSpeed;
            }
            movement.moveDelta *= speed * Time.deltaTime;
            movement.moveDelta += Vector3.down * 0.2f;
            movement.characterController.Move(movement.moveDelta);
        }
    }

}