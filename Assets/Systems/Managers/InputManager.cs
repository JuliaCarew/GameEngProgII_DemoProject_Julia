using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour, Inputs.IPlayerActions
{
    private Inputs inputs;
    void Awake()
    {
        try
        {
            inputs = new Inputs();
            inputs.Player.SetCallbacks(this);
            inputs.Player.Enable();
        }
        catch (Exception exception)
        {
            Debug.LogError($"Error initializing InputManager: {exception.Message}");
        }
    }

    #region Input Events

    // Events that are triggered when input activity is detected
    public event Action<Vector2> MoveInputEvent;
    public event Action<Vector2> LookInputEvent;

    // jumping
    public event Action JumpStartedInputEvent;
    public event Action JumpPerformedInputEvent;
    public event Action JumpCanceledInputEvent;

    // sprint
    public event Action SprintStartedInputEvent;
    public event Action SprintPerformedInputEvent;
    public event Action SprintCanceledInputEvent;

    // crouch
    public event Action CrouchStartedInputEvent;
    public event Action CrouchPerformedInputEvent;
    public event Action CrouchCanceledInputEvent;

    #endregion

    #region Input Callbacks

    // handle input action callbacks abd dispatches input data to listeners
    public void OnMove(InputAction.CallbackContext context)
    {
        MoveInputEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        LookInputEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started) JumpStartedInputEvent?.Invoke();
        else if (context.performed) JumpPerformedInputEvent?.Invoke();
        else if (context.canceled) JumpCanceledInputEvent?.Invoke();
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.started) SprintStartedInputEvent?.Invoke();
        else if (context.performed) SprintPerformedInputEvent?.Invoke();
        else if (context.canceled) SprintCanceledInputEvent?.Invoke();
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        if (context.started) CrouchStartedInputEvent?.Invoke();
        else if (context.performed) CrouchPerformedInputEvent?.Invoke();
        else if (context.canceled) CrouchCanceledInputEvent?.Invoke();
    }

    #endregion

    void OnEnable()
    {
        if (inputs != null)
            inputs.Player.Enable();
    }

    void OnDestroy()
    {
        if (inputs != null)
            inputs.Player.Disable();
    }
}
