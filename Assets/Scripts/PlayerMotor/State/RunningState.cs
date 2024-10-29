using UnityEngine;

public class RunningState : BaseState 
    {

    public override void Construct() {
        _motor._verticalVelocity = 0;
    }

    public override Vector3 ProcessMotion() {
        Vector3 m = Vector3.zero;

        m.x = _motor.SnapToLane();
        m.y = -1.0f;
        m.z = _motor._baseRunSpeed;
          
        return m;
    }

    public override void Transition() {

        if (InputManager.Instance.SwipeLeft)
            _motor.ChangeLane(-1);

        if (InputManager.Instance.SwipeRight)
            _motor.ChangeLane(1);

        if (InputManager.Instance.SwipeUp && _motor._isGrounded)
            _motor.ChangeState(GetComponent<JumpingState>());

        if (!_motor._isGrounded)
            _motor.ChangeState(GetComponent<FallingState>());

        if (InputManager.Instance.SwipeDown)
            _motor.ChangeState(GetComponent<SlidingState>());
    }
}
