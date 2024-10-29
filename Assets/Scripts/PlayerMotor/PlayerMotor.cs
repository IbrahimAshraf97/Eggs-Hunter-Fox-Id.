using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    [HideInInspector] public Vector3 _moveVector;
    [HideInInspector] public float _verticalVelocity;
    [HideInInspector] public bool _isGrounded;
    [HideInInspector] public int _currentLane;

    public float _distanceInBetweenLanes = 3.0f;
    public float _baseRunSpeed = 5.0f;
    public float _baseSideWaySpeed = 10.0f;
    public float _gravity = 14.0f;
    public float _terminalVelocity = 20.0f;

    public CharacterController _controller;
    public Animator _anim;

    private BaseState _state;
    private bool _isPaused;

    // Start is called before the first frame update
    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _anim = GetComponent<Animator>();

        _state = GetComponent<RunningState>();
        _state.Construct();
        _isPaused = true;   
    }

    // Update is called once per frame
    void Update()
    {
        if(!_isPaused)
            UpdateMotor();

        //SnapToLane();
    }

    private void UpdateMotor() {
        // Check if we're grounded
        _isGrounded = _controller.isGrounded;

        // How should we be moving now ? based on  state
        _moveVector = _state.ProcessMotion();

        // are we trying to change the state ?
        _state.Transition();

        // Feed our animator
        _anim?.SetBool("IsGrounded", _isGrounded);
        _anim?.SetFloat("Speed", Mathf.Abs(_moveVector.z));

        // move the player
        _controller.Move(_moveVector * Time.deltaTime);
    }

    public float SnapToLane() {
        float r = 0.0f;

        // If we're not directly on top of a lane 
        if (transform.position.x != (_currentLane * _distanceInBetweenLanes))
        {
            float deltaToDesiredPosition = (_currentLane * _distanceInBetweenLanes) - transform.position.x;

            r = (deltaToDesiredPosition > 0) ? 1 : -1;
            r *= _baseRunSpeed;

            float actualDistance = r * Time.deltaTime;
            if (Mathf.Abs(actualDistance) > Mathf.Abs(deltaToDesiredPosition))
                r = deltaToDesiredPosition * (1 / Time.deltaTime);
        } 
        else 
        {
            r = 0;
        }

        return r;
    }
    public void ChangeLane(int diraction) {
        _currentLane = Mathf.Clamp(_currentLane + diraction, -1, 1);
    }
    public void ChangeState(BaseState s) {
        _state.Destruct();
        _state = s;
        _state.Construct();
    }

    public void ApplyGravity() {
        _verticalVelocity -= _gravity * Time.deltaTime;

        if (_verticalVelocity < -_terminalVelocity)
            _verticalVelocity = -_terminalVelocity;
    }

    public void PausePlayer() {
        _isPaused = true;
    }
    public void ResumePlayer() {
        _isPaused = false;
    }

    public void RespawnPlayer() {
        ChangeState(GetComponent<RespawnState>());
        GameManager.Instance.ChangeCamera(GameCamera.Respawn);
    }
    public void ResetPlayer() {
        _currentLane = 0;
        transform.position = Vector3.zero;
        _anim?.SetTrigger("Idle");
        ChangeState(GetComponent<RunningState>());
        PausePlayer();
    }

    public void OnControllerColliderHit(ControllerColliderHit hit) {
        string hitLayerName = LayerMask.LayerToName(hit.gameObject.layer);
        if (hitLayerName == "Death")
            ChangeState(GetComponent<DeathState>());
    }
}
