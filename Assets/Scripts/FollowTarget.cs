using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FollowTarget
{
    float mouseSensitivity;
    Transform whatToFollow, follower;

    public FollowTarget(float _mouseSens, Transform _whatToFollow, Transform _follower)
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        mouseSensitivity = _mouseSens;
        whatToFollow = _whatToFollow;
        follower = _follower;
    }

    // Update is called once per frame
    public void LogicUpdate()
    {
        Vector2 movingCamera = InputDistributor.inputActions.Player.MoveCamera.ReadValue<Vector2>();
        //Debug.Log(movingCamera);
        follower.position = whatToFollow.position;

        follower.rotation = Quaternion.Euler(follower.rotation.eulerAngles.x, follower.rotation.eulerAngles.y + movingCamera.x * mouseSensitivity * Time.deltaTime, follower.rotation.eulerAngles.z);
    }
}
