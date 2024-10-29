using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingState : BaseState
{

    public float _jumpForce = 7f;

    public override void Construct() {
        _motor._verticalVelocity = _jumpForce;
        _motor._anim?.SetTrigger("Jump");

    }
    public override Vector3 ProcessMotion() {

        // Apply gravity
        _motor.ApplyGravity();

        // Create our return vector
        Vector3 m = Vector3.zero;

        m.x = _motor.SnapToLane();
        m.y = _motor._verticalVelocity;
        m.z = _motor._baseRunSpeed;

        return m;
    }
    public override void Transition() {

        if (InputManager.Instance.SwipeLeft)
            _motor.ChangeLane(-1);

        if (InputManager.Instance.SwipeRight)
            _motor.ChangeLane(1);
 
        if (_motor._verticalVelocity < 0)
            _motor.ChangeState(GetComponent<FallingState>());
    }
}
