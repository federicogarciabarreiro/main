using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CameraModePortal : ICameraMode
{

    Transform camera;
    public Transform target;
    public Collider enemyTarget;
    Vector3 startPos;
    Vector3 endPos;
    Vector3 offset;
    float lerpSpeed = 0.01f;
    Quaternion finalRotation;

    public CameraZone currentZone { get; set; }

    public CameraModePortal(Transform Camera, Transform Target, Vector3 StartPos, Vector3 EndPos, Vector3 Offset, Quaternion FinalRotation)
    {
        camera = Camera;
        target = Target;
        startPos = StartPos;
        endPos = EndPos;
        offset = Offset;
        finalRotation = FinalRotation;
    }

    public void Enter()
    {

    }

    public void Action()
    {
        Vector3 pos = target.position - new Vector3(offset.x, offset.y, offset.z);
        //pos = new Vector3(Mathf.Clamp(pos.x, startPos.x, endPos.x), pos.y, pos.z);

        //camera.position = Vector3.Lerp(camera.position, pos, lerpSpeed);
        camera.GetComponent<Camera>().fieldOfView = Mathf.Lerp(camera.GetComponent<Camera>().fieldOfView, 35f, Time.deltaTime);
        camera.RotateAround(target.position, Vector3.up, Input.GetAxis("Horizontal") * Time.deltaTime * 100f);
        camera.forward = (target.position - camera.transform.position).normalized;
        //camera.rotation = Quaternion.Lerp(camera.rotation, finalRotation, 0.025f);

    }

    public void SetCurrentZone(CameraZone zone)
    {

    }
    public void SetTarget(Transform target)
    {
        this.target = target;
    }
}