using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraModeTarget : ICameraMode
{

    Transform camera;
    public Transform target;
    public Vector3 offset;

    float lerpSpeed = 0.03f;

    public CameraZone currentZone { get; set; }

    public CameraModeTarget(Transform Camera, Transform Target, Vector3 Offset)
    {
        camera = Camera;
        target = Target;
        offset = Offset;
        
    }

    public void Enter()
    {
        camera.position = target.position - new Vector3(offset.x, offset.y, offset.z);
        camera.forward = (target.position - camera.transform.position).normalized;

    }

    public void Action()
    {
        //Vector3 pos = target.position - new Vector3(offset.x, offset.y, offset.z);
        
        //camera.position = Vector3.Lerp(camera.position, pos, lerpSpeed);
    }

    public void SetCurrentZone(CameraZone zone)
    {

    }
    public void SetTarget(Transform target)
    {
        this.target = target;
    }
}
