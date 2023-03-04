using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public static class CameraHandler
{
    static CinemachineVirtualCamera currentActiceCam;
    public static void MoveCamera(CinemachineVirtualCamera camera, Vector3 addedPosition, Quaternion addedRotation)
    {

    }

    public static void SwitchCamera(CinemachineVirtualCamera newCamera)
    {
        if(currentActiceCam == null)
        {
            currentActiceCam = newCamera;
            newCamera.Priority = 20;
        }

        else
        {
            currentActiceCam.Priority = 1;
            newCamera.Priority = 20;
            currentActiceCam = newCamera;
        }
    }

    public static void LockOn(ILockable _target)
    {
        //Function if I want to make locking on enemies a thing
    }

    public static void Zoom()
    {

    }
}
