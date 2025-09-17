using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    private InputManager inputManager => GameManager.Instance.inputManager;
    private CharacterController characterController => GetComponent<CharacterController>();

    [SerializeField] private Transform cameraRoot;
    public Transform CameraRoot => cameraRoot;

    [Header("Enable/Disable Controls & Features")]
    public bool moveEnabled = true;
    public bool lookEnabled = true;

    [SerializeField] private bool jumpEnabled = true;
    [SerializeField] private bool sprintEnabled = true;

    [Header("Look Settings")]
    public Vector2 lookInput;
    // camera look
    public float horizontalLookSensitivity = 1;
    public float verticalLookSensitivity = 1;

    [SerializeField] float upperLookLimit = 60;
    [SerializeField] float lowerLookLimit = -60;

    [SerializeField] public bool invertLookY { get; private set; } = false;

    [Header("Movement Settings")]
    public Vector2 moveInput;
    [SerializeField] private float currentSpeed;
    [SerializeField] private float walkSpeed = 4f;
    [SerializeField] private float sprintSpeed = 5f;
    [SerializeField] private float crouchSpeed = 3f;
    [SerializeField] private float speedTransitionDuration = 0.25f;

    private bool sprintInput = false;
    private bool crouchInput = false;

    void Awake()
    {
        
    }

    private void Update()
    {
        HandlePlayerMovement();
    }

    private void LateUpdate()
    {
        HandlePlayerLook();
    }

    public void HandlePlayerMovement()
    {
        if (!moveEnabled) return;

        // get input direction
        Vector3 moveInputDirection = new Vector3(moveInput.x, 0, moveInput.y);
        Vector3 worldMoveDirection = transform.TransformDirection(moveInputDirection);

        float targetSpeed;

        if(sprintInput == true) {
            targetSpeed = sprintSpeed;
        }
        else {
            targetSpeed = walkSpeed;
        }

        // handle sprint acelleration
        float lerpSpeed = 1f - Mathf.Pow(0.01f, Time.deltaTime / speedTransitionDuration);
        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, lerpSpeed);

        // handle horizontal mvmt
        Vector3 horizontalMovement = worldMoveDirection * currentSpeed;

        // handle jumping & gravity


        // combine horizontal & vertical movement
        Vector3 movement = horizontalMovement;

        characterController.Move(movement * Time.deltaTime * currentSpeed) ;
    }

    public void HandlePlayerLook()
    {
        if (!lookEnabled) return;

        float lookX = lookInput.x * horizontalLookSensitivity * Time.deltaTime;
        float lookY = lookInput.y * verticalLookSensitivity * Time.deltaTime;

        // rotate left-right
        transform.Rotate(Vector3.up * lookX);

        // tilting on x axis up-down
        Vector3 currentAngle = cameraRoot.localEulerAngles;
        float newRotationX = currentAngle.x - lookY;

        // clamping look limit
        newRotationX = (newRotationX > 180) ? newRotationX - 360 : newRotationX;
        newRotationX = Mathf.Clamp(newRotationX, lowerLookLimit, upperLookLimit);

        cameraRoot.localEulerAngles = new Vector3(newRotationX, 0, 0);
    }

    void SetMoveInput(Vector2 inputVector)
    {
        moveInput = new Vector2(inputVector.x, inputVector.y ); 
    }

    void SetLookInput(Vector2 inputVector)
    {
        lookInput = new Vector2(inputVector.x, inputVector.y);
        Debug.Log($"Look Input: {lookInput}");
    }

    #region Jump event
    void JumpHandler(InputAction.CallbackContext context)
    {
        if (jumpEnabled && context.started)
            Debug.Log("Jump started");

        if (context.performed)
            Debug.Log("Jump held");

        if (context.canceled)
            Debug.Log("Jump cancelled");
    }
    #endregion

    #region Sprint event

    void SprintHandler(InputAction.CallbackContext context)
    {
        if (sprintEnabled == false) return;

        if (context.started) 
        {
            sprintInput = true;
            currentSpeed = walkSpeed;
            Debug.Log("Sprinting");
        }                
        if (context.canceled)
        {
            sprintInput = false;
            Debug.Log("Sprint cancelled");
        }
    }
    #endregion

    #region Crouch event
    void CrouchHandler(InputAction.CallbackContext context)
    {
        // change character height (and speed?)
        if (context.started)
            Debug.Log("Crouch cancelled");

        if (context.performed)
            Debug.Log("Crouch held");

        if (context.canceled)
            Debug.Log("Crouch cancelled");
    }
    #endregion


    void OnEnable()
    {
        inputManager.MoveInputEvent += SetMoveInput;
        inputManager.LookInputEvent += SetLookInput;

        inputManager.SprintInputEvent += SprintHandler; 
        inputManager.CrouchInputEvent += CrouchHandler;
        inputManager.JumpInputEvent += JumpHandler;
    }

    void OnDestroy()
    {
        inputManager.MoveInputEvent -= SetMoveInput;
        inputManager.LookInputEvent -= SetLookInput;

        inputManager.SprintInputEvent -= SprintHandler; 
        inputManager.CrouchInputEvent -= CrouchHandler;
        inputManager.JumpInputEvent -= JumpHandler;
    }
}
