using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class ThirdPersonTest : MonoBehaviour
{
    public float moveSpeed;
    public float gravity;
    CharacterController controller;

    [Header("Input")]
    PlayerInputActions playerControls;
    InputAction move, attack, focus;

    Animator playerAnimator;

    // Start is called before the first frame update
    void Awake()
    {
        controller = GetComponent<CharacterController>();
        playerControls = new PlayerInputActions();
        playerAnimator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        move = playerControls.Player.Move;
        move.Enable();

        attack = playerControls.Player.Attack;
        attack.Enable();
        attack.performed += Attack;

        focus = playerControls.Player.Focus;
        focus.Enable();
    }

    private void OnDisable()
    {
        move.Disable();
        attack.Disable();
        attack.performed -= Attack;
        focus.Disable();
    }
    // Update is called once per frame
    void Update()
    {
        Move(transform);    
        //controller.Move();


    }
    
    void Move(Transform playerTransform)
    {
        Camera cam = Camera.main;
        Vector2 inputDirection = move.ReadValue<Vector2>();


        float Horizontal = inputDirection.x * moveSpeed;
        float Vertical = inputDirection.y * moveSpeed;


        //move based on the camera view.
        Vector3 horizontalMovement = cam.transform.right;
        horizontalMovement = new Vector3(horizontalMovement.x, 0, horizontalMovement.z);
        horizontalMovement.Normalize();

        Vector3 verticalMovement = cam.transform.forward;
        verticalMovement = new Vector3(verticalMovement.x, 0, verticalMovement.z);
        verticalMovement.Normalize();

        Vector3 movement = horizontalMovement * Horizontal + verticalMovement * Vertical;
        movement = new Vector3(movement.x, 0, movement.z);

        if(movement != Vector3.zero)
        {
            playerAnimator.SetBool("Moving", true);
        }
        else
        {
            playerAnimator.SetBool("Moving", false);
        }

        controller.Move(movement * moveSpeed * Time.deltaTime + Vector3.up * gravity * Time.deltaTime);
        playerTransform.forward = Vector3.Lerp(playerTransform.forward, movement, 0.01f);
    }

    void Attack(InputAction.CallbackContext context)
    {
        playerAnimator.SetTrigger("Attack");
    }

    void Focus(InputAction.CallbackContext context)
    {
        playerAnimator.SetTrigger("Attack");
    }
}
