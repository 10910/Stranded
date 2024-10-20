
public class MidairState : State
{
    public MidairState(StateManager stateManager) : base(stateManager) {
    }

    public override void Enter() {
        
    }

    public override void Exit() {
    }

    public override void Update() {
        if(_stateManager.movement.characterController.isGrounded){
            _stateManager.SetState(_stateManager.groundedState);
        }
    }
}