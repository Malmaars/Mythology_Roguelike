using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class ForwardTarget
{
    public Transform thisTransform;

    Transform zoomBodyTransform;
    Vector3 initialZoomFollowPos;

    Transform objectToFollow;
    Transform objectToOrbit;

    CinemachineVirtualCamera zoomCam;

    //could compare the position of forwardTarget to be on the radius of a sphere coming from the player
    //this float is the radius of that Sphere
    public float distanceFromObject;
    float heightOffset;

    float zoomCameraSensitivity;
    public bool zoomedIn = false;

    public ForwardTarget(Transform _follow, Transform _orbit, float _distance, float _hOffset, CinemachineVirtualCamera _zoomCam)
    {
        objectToFollow = _follow;
        objectToOrbit = _orbit;
        distanceFromObject = _distance;
        heightOffset = _hOffset;
        zoomCam = _zoomCam;

        thisTransform = new GameObject().transform;
        zoomBodyTransform = new GameObject().transform;
        zoomCam.LookAt = thisTransform;
        zoomBodyTransform = zoomCam.Follow;
        initialZoomFollowPos = zoomBodyTransform.localPosition;
    }
    // Update is called once per frame
    public void LogicUpdate()
    {
        //if we're not zoomed in, follow the player
        if (zoomedIn)
            Orbit();

        else
            FollowTarget();
        
        //if we áre zoomed in, orbit around the object when the mouse moves
    }

    void FollowTarget()
    {
        Vector3 newFw = Camera.main.transform.forward;
        newFw = new Vector3(newFw.x, 0, newFw.z);
        thisTransform.position = objectToFollow.position + newFw * distanceFromObject + objectToFollow.up * heightOffset;
    }

    void Orbit()
    {
        //i should lock the up and down movement so it can't go too far
        float maxHeight = 4.5f;
        float minHeight = 4.5f;

        Quaternion previousRot = thisTransform.rotation;
        Vector3 previousPos = thisTransform.position;

        Vector2 movingCamera = InputDistributor.inputActions.Player.MoveCamera.ReadValue<Vector2>();

        //I can check the player's forward looking direction and base the new direciton off that
        //I can also base it off the camera, which seems more flexible to me

        //or get the direction from the orbitObject to the forwardtarget, and edit that direction using mouse input < this

        Vector3 currentDir = (thisTransform.position - objectToOrbit.position).normalized;

        Vector3 newDir = new Vector3(currentDir.x + movingCamera.y * zoomCameraSensitivity, currentDir.y + movingCamera.x * zoomCameraSensitivity, currentDir.z).normalized;

        //Debug.Log(newDir);

        thisTransform.position = objectToOrbit.position;
        thisTransform.forward = currentDir;
        thisTransform.rotation = Quaternion.Euler(thisTransform.rotation.eulerAngles.x - movingCamera.y * zoomCameraSensitivity, thisTransform.rotation.eulerAngles.y + movingCamera.x * zoomCameraSensitivity, thisTransform.rotation.eulerAngles.z);

        Vector3 newPos = thisTransform.position + thisTransform.forward.normalized * distanceFromObject;
        thisTransform.position = newPos;

        if (newPos.y - objectToOrbit.position.y >= maxHeight || objectToOrbit.position.y - newPos.y > minHeight)
        {
            thisTransform.position = previousPos;
            thisTransform.rotation = previousRot;
        }
        

        //zoomCam.m_Follow = zoomBodyTransform;
        Vector3 zoomFollowOffset = (objectToFollow.position - thisTransform.position).normalized;
        zoomFollowOffset = new Vector3(0, zoomFollowOffset.y * 2, 0);

        zoomBodyTransform.localPosition = initialZoomFollowPos + zoomFollowOffset;

    }

    public void GizmosDraw()
    {
        Debug.Log("Drawing Gizmos");
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(objectToOrbit.position, distanceFromObject);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(thisTransform.position, 1f);
    }


    public void CheckDistanceChange(float _newDistance)
    {
        if (_newDistance != distanceFromObject)
            distanceFromObject = _newDistance;
    }

    public void CheckSensitivityTarget(float _sens)
    {
        if(_sens != zoomCameraSensitivity)
        {
            zoomCameraSensitivity = _sens;
        }
    }

    public void ZoomIn(InputAction.CallbackContext context)
    {
        zoomedIn = true;
        CameraHandler.SwitchCamera(zoomCam);
    }

    public void ZoomOut(InputAction.CallbackContext context)
    {
        zoomedIn = false;
    }
}
