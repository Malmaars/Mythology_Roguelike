using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    InputManager inputManager;
    PlayerInputActions controls;

    [Header("Player")]
    public float moveSpeed;
    public float gravity;
    public Transform playerTransform;
    public Animator playerAnimator;
    public CharacterController controller;
    Player playerScript;

    [Header("Camera")]
    public CinemachineVirtualCamera normalCamera;
    public float mouseSensitivity;
    public Transform whatToFollow;
    public Transform follower;
    FollowTarget followTarget;
    public CinemachineVirtualCamera zoomCam;
    public Transform zoomTarget;
    public float zoomDistanceFromPlayer;
    public float heightOffset;
    public float zoomCamSensitivity;
    ForwardTarget forwardTarget;

    [Header("Enemies")]
    EnemyManager enemyManager;

    // Start is called before the first frame update
    void Awake()
    {
        controls = new PlayerInputActions();
        InputDistributor.inputActions = controls;

        //add here the actions we want to use.
        inputManager = new InputManager(
            new InputAction[] {
            controls.Player.Move,
            controls.Player.Attack,
            controls.Player.SpecialAttack,
            controls.Player.Focus,
            controls.Player.MoveCamera
        });
        //inputManager.AddActionToInput(controls.Player.Attack, DebugOnInput);
        forwardTarget = new ForwardTarget(playerTransform, playerTransform, zoomDistanceFromPlayer, heightOffset, zoomCam);
        playerScript = new Player(playerTransform, forwardTarget.thisTransform, moveSpeed, playerAnimator, controller, gravity);
        followTarget = new FollowTarget(mouseSensitivity, whatToFollow, follower);


        enemyManager = new EnemyManager();

        inputManager.AddActionToInput(InputDistributor.inputActions.Player.Focus, forwardTarget.ZoomIn);
        inputManager.AddActionToInput(InputDistributor.inputActions.Player.Focus, playerScript.ZoomIn);
        inputManager.AddActionToInputCancelled(InputDistributor.inputActions.Player.Focus, forwardTarget.ZoomOut);
        inputManager.AddActionToInputCancelled(InputDistributor.inputActions.Player.Focus, playerScript.ZoomOut);
        inputManager.AddActionToInputCancelled(InputDistributor.inputActions.Player.Focus, SwitchToNormalCamera);
    }

    private void OnEnable()
    {
        inputManager.WhenEnabled();
    }

    private void OnDisable()
    {
        inputManager.WhenDisabled();
    }

    private void OnDrawGizmos()
    {
        if (forwardTarget != null)
        {
            forwardTarget.GizmosDraw();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //maybe to reduce repeating this, I could make each class that uses a LogicUpdate part of an IUpdate interface, and put them all in a List
        //so I can just do some for loop to go through all the update functions
        playerScript.LogicUpdate();
        followTarget.LogicUpdate();
        forwardTarget.LogicUpdate();
        forwardTarget.CheckDistanceChange(zoomDistanceFromPlayer);
        forwardTarget.CheckSensitivityTarget(zoomCamSensitivity);
        enemyManager.LogicUpdate();        
    }

    void SwitchToNormalCamera(InputAction.CallbackContext context)
    {
        CameraHandler.SwitchCamera(normalCamera);
    }

    void DebugOnInput(InputAction.CallbackContext context)
    {
        Debug.Log("button pressed");
    }
}
