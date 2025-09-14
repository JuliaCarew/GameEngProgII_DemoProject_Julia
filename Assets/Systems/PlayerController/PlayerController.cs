using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PlayerController : MonoBehaviour
{
    //public GameManager gameManager;
    private InputManager inputManager;

    // input variables
    public Vector2 moveInput;
    public Vector2 lookInput;

    // movement variables
    public float currentSpeed;
    public float walkSpeed = 5f;
    public float sprintSpeed = 10f;

    // bools to trigger sprint/crouch states
    private bool isSprinting = false;
    private bool isCrouching = false;

    void Awake()
    {
        inputManager = GameManager.Instance.inputManager;
    }


    void SetMoveInput(Vector2 inputVector)
    {
        moveInput = new Vector2(inputVector.x, inputVector.y ); // later * walkspeed or sprintSpeed
        Debug.Log($"Move Input: {moveInput}");
    }

    void SetLookInput(Vector2 inputVector)
    {
        lookInput = new Vector2(inputVector.x, inputVector.y);
        Debug.Log($"Look Input: {lookInput}");
    }

    void SetSprintInput()
    {
        if (isSprinting) {
            currentSpeed = walkSpeed;
            Debug.Log("Not Sprinting");
        }
        else {
            currentSpeed = sprintSpeed;
            Debug.Log("Sprinting");
        }
    }

    void SetCrouchInput()
    {
        // change character height (and speed?)
        if (isCrouching) isSprinting = false;
        Debug.Log("Crouching");
    }   

    void OnEnable()
    {
        inputManager.MoveInputEvent += SetMoveInput;
        inputManager.LookInputEvent += SetLookInput;

        // sprint
        inputManager.SprintStartedInputEvent += SetSprintInput; 
        inputManager.SprintPerformedInputEvent += SetSprintInput; 
        inputManager.SprintCanceledInputEvent += SetSprintInput;
        
        // crouch
        inputManager.CrouchStartedInputEvent += SetCrouchInput;
        inputManager.CrouchPerformedInputEvent += SetCrouchInput;
        inputManager.CrouchCanceledInputEvent += SetCrouchInput;

        // jump
        inputManager.JumpStartedInputEvent += () => Debug.Log("Jump Started");
        inputManager.JumpPerformedInputEvent += () => Debug.Log("Jump Performed");
        inputManager.JumpCanceledInputEvent += () => Debug.Log("Jump Canceled");
    }

    void OnDestroy()
    {
        inputManager.MoveInputEvent -= SetMoveInput;
        inputManager.LookInputEvent -= SetLookInput;

        // sprint
        inputManager.SprintStartedInputEvent -= SetSprintInput; 
        inputManager.SprintPerformedInputEvent -= SetSprintInput; 
        inputManager.SprintCanceledInputEvent -= SetSprintInput;
        
        // crouch
        inputManager.CrouchStartedInputEvent -= SetCrouchInput;
        inputManager.CrouchPerformedInputEvent -= SetCrouchInput;
        inputManager.CrouchCanceledInputEvent -= SetCrouchInput;

        // jump
        inputManager.JumpStartedInputEvent -= () => Debug.Log("Jump Started");
        inputManager.JumpPerformedInputEvent -= () => Debug.Log("Jump Performed");
        inputManager.JumpCanceledInputEvent -= () => Debug.Log("Jump Canceled");
    }
}
