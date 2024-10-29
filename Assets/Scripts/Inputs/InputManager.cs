using System;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

public class InputManager : MonoBehaviour {
    // There should be only one InputManager in the scene
    private static InputManager instance;
    public static InputManager Instance { get { return instance; } }

    //Action Schemes
    private RunnerInputAction _actionScheme;

    // Configuration
    [SerializeField] private float sqrSwipDeadZone = 50.0f;

    #region public properties
    public bool Tap {get { return tap; } }
    public Vector2 TouchPosition {get { return touchPosition; } }
    public bool SwipeLeft { get { return swipeLeft; } }
    public bool SwipeRight { get { return swipeRight; } }
    public bool SwipeUp { get { return swipeUp; } }
    public bool SwipeDown { get { return swipeDown; } }
    #endregion

    #region private
    private bool tap;
    private Vector2 touchPosition;
    private Vector2 startDrag;
    private bool swipeLeft;
    private bool swipeRight;
    private bool swipeUp;
    private bool swipeDown;

    #endregion

    private void Awake() {
        instance = this;
        DontDestroyOnLoad(gameObject);
        SetupControl();
    }


    private void LateUpdate() {
        ResetInputs();
    }
    private void ResetInputs() {
        tap = swipeLeft = swipeRight = swipeUp = swipeDown = false;
    }

    private void SetupControl() { 
        _actionScheme = new RunnerInputAction();
        // Register different actions
        _actionScheme.GamePlay.Tap.performed += ctx => OnTap(ctx);
        _actionScheme.GamePlay.TouchPosition.performed += ctx => OnPosition(ctx);
        _actionScheme.GamePlay.StartDrag.performed += ctx => OnStartDrag(ctx);
        _actionScheme.GamePlay.EndDrag.performed += ctx => OnEndtDrag(ctx);

    }

    private void OnEndtDrag(UnityEngine.InputSystem.InputAction.CallbackContext ctx) {
        
        Vector2 delta = touchPosition - startDrag;
        float sqrDistance = delta.sqrMagnitude;

        // Confirmed swipe
        if (sqrDistance > sqrSwipDeadZone) {

            float x = Mathf.Abs(delta.x);
            float y = Mathf.Abs(delta.y);

            if (x > y) // Left or Right
            {
                if (delta.x > 0)
                    swipeRight = true;
                else
                    swipeLeft = true;
            }
            else  // Up or Down
            { 
                if(delta.y > 0 )
                    swipeUp = true;
                else
                    swipeDown=true;
            }
        }

    }

    private void OnStartDrag(UnityEngine.InputSystem.InputAction.CallbackContext ctx) {
        startDrag = touchPosition;
    }

    private void OnPosition(UnityEngine.InputSystem.InputAction.CallbackContext ctx) {
        touchPosition = ctx.ReadValue<Vector2>();
    }

    private void OnTap(UnityEngine.InputSystem.InputAction.CallbackContext ctx) {
        tap = true;
    }

    public void OnEnable() {
        _actionScheme.Enable();
    }

    public void OnDisable() {
        _actionScheme.Disable();
    }
}

/*
 *      DYENAMIC UPDATE
 *      InputManager that processes the inputs
 *      PlayerMotor uses these inputs to move
 * 
 *      LATE UPDATE 
 *      InputManager resets these inputs
 */
