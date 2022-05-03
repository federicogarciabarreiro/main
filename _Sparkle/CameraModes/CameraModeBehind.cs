using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraModeBehind : ICameraMode {

    Transform camera;
    Transform target;
    Vector3 startPos;
    Vector3 endPos;
    Vector3 offset;
    float lerpSpeed = 0.03f;
    Vector3 finalRotation;
    Vector3 finalPos;

    public CameraZone currentZone { get; set; }

    public CameraModeBehind(Transform Camera, Transform Target)
    {
        camera = Camera;
        target = Target;
    }
    public void Enter()
    {
         
    }
    public void Action()
    {
        finalPos = target.position + Vector3.right * 3 + Vector3.up;
        camera.position = Vector3.Lerp(camera.position, finalPos, Time.deltaTime * 2.5f);
        Vector3 dirToLook = (target.transform.position - finalPos).normalized;
        camera.forward = Vector3.Slerp(camera.forward, dirToLook, Time.deltaTime *2f);
    }

    public void SetCurrentZone(CameraZone zone)
    {
        
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }
}
