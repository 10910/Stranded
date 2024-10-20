
using UnityEngine;

public class MidairState : State
{
    Movement movement;
    public MidairState(StateManager stateManager) : base(stateManager) {
        movement = stateManager.movement;
    }

    public override void Enter() {
        
    }

    public override void Exit() {
        movement.velocity = 0;
        movement.JumpVelocity = Vector2.zero;
    }

    public override void Update() {
        if (_stateManager.movement.characterController.isGrounded && movement.velocity < 0) {
            _stateManager.SetState(_stateManager.groundedState);
        }
        else {
            Vector3 _moveDelta = Vector3.zero;
            if (movement.JumpVelocity != Vector2.zero) {
                Vector3 horizontalDelta = new Vector3(movement.JumpVelocity.x, 0, movement.JumpVelocity.y) * Time.deltaTime;
                _moveDelta += horizontalDelta;
            }

            if (movement.isTopBlocked()) {
                movement.velocity = 0;
            }
            
            movement.velocity -= Time.deltaTime * movement.gravity;
            _moveDelta += Vector3.up * movement.velocity * Time.deltaTime;
            movement.characterController.Move(_moveDelta);
        }
    }
}