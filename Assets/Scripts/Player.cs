using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player
{
    float moveSpeed;
    float gravity;
    Animator playerAnimator;
    CharacterController controller;

    Transform playerTransform;
    Transform zoomedLookAt;

    public bool zoomedIn;
    public Player(Transform _playerTransform, Transform _zoomedLookAt ,float _moveSpeed, Animator _playerAnimator, CharacterController _controller, float _gravity)
    {
        moveSpeed = _moveSpeed;
        playerAnimator = _playerAnimator;
        controller = _controller;
        gravity = _gravity;
        playerTransform = _playerTransform;
        zoomedLookAt = _zoomedLookAt;
    }
    public void Initialize()
    {

    }

    public void LogicUpdate()
    {
        if (!zoomedIn)
            Move(playerTransform);

        else
            Strafe(playerTransform);
    }

    void Move(Transform playerTransform)
    {
        Camera cam = Camera.main;
        Vector2 inputDirection = InputDistributor.inputActions.Player.Move.ReadValue<Vector2>();


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

        if (movement != Vector3.zero)
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

    void Strafe(Transform playerTransform)
    {
        //to strafe:
        //player always looks forward, perhaps to an empty gameobject (target) in front of it
        //The target can then be moved by using the camera, the target will orbit around the player
        Vector3 newForwardVector = zoomedLookAt.position - playerTransform.position;
        playerTransform.forward = new Vector3(newForwardVector.x, 0, newForwardVector.z);
    }
    public void ZoomIn(InputAction.CallbackContext context)
    {
        zoomedIn = true;
    }

    public void ZoomOut(InputAction.CallbackContext context)
    {
        zoomedIn = false;
    }
}
