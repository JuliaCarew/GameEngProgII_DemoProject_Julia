using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

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
    public event Action<InputAction.CallbackContext> JumpInputEvent;
    
    // sprint
    public event Action<InputAction.CallbackContext> SprintInputEvent;

    // crouch
    public event Action<InputAction.CallbackContext> CrouchInputEvent;


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
        JumpInputEvent?.Invoke(context);
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        SprintInputEvent?.Invoke(context);
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        CrouchInputEvent?.Invoke(context);
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
