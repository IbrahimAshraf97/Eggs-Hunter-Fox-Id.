using UnityEngine;

public class RespawnState : BaseState
{
    [SerializeField] private float _verticalDistance = 25.0f;
    [SerializeField] private float _immunityTime = 1f;

    private float _startTime;

    public override void Construct() {

        _startTime = Time.time;

        _motor._controller.enabled = false;
        _motor.transform.position = new Vector3(0, _verticalDistance, _motor.transform.position.z);
        _motor._controller.enabled = true;
        _motor._verticalVelocity = 0.0f;
        _motor._currentLane = 0;
        _motor._anim?.SetTrigger("Respawn");

    }

    public override void Destruct() {
        GameManager.Instance.ChangeCamera(GameCamera.Game);
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
        if (_motor._isGrounded && (Time.time - _startTime)> _immunityTime)
            _motor.ChangeState(GetComponent<RunningState>());

        if (InputManager.Instance.SwipeLeft)
            _motor.ChangeLane(-1);

        if (InputManager.Instance.SwipeRight)
            _motor.ChangeLane(1);
    }
}
