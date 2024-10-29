using UnityEngine;

public class SlidingState : BaseState
{
    public float _slideDuration = 1.0f;

    //Collider logic
    private Vector3 _initialCenter;
    private float _initialSize;
    private float _slideStart;

    public override void Construct() {
        _motor._anim?.SetTrigger("Slide");

        _slideStart = Time.time;

        _initialSize = _motor._controller.height;
        _initialCenter = _motor._controller.center;

        _motor._controller.height = _initialSize * 0.5f;
        _motor._controller.center = _initialCenter * 0.5f;
    }
    public override void Destruct() {

        _motor._controller.height = _initialSize;
        _motor._controller.center = _initialCenter;
        _motor._anim?.SetTrigger("Running");

    }

    public override void Transition() {

        if (InputManager.Instance.SwipeLeft)
            _motor.ChangeLane(-1);

        if (InputManager.Instance.SwipeRight)
            _motor.ChangeLane(1);

        if (!_motor._isGrounded)
            _motor.ChangeState(GetComponent<FallingState>());

        if (InputManager.Instance.SwipeUp)
            _motor.ChangeState(GetComponent<JumpingState>());

        if (Time.time - _slideStart > _slideDuration)
            _motor.ChangeState(GetComponent<RunningState>());

    }

    public override Vector3 ProcessMotion() {
        Vector3 m = Vector3.zero;

        m.x = _motor.SnapToLane();
        m.y = -1.0f;
        m.z = _motor._baseRunSpeed;

        return m;
    }
}
