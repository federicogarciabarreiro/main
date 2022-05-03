using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraModeLineal : ICameraMode {

    Transform camera;
    Transform target;
    Vector3 startPos;
    Vector3 endPos;
    Vector3 offset;

    float lerpSpeed = 0.03f;

    public CameraZone currentZone { get; set; }

    public CameraModeLineal(Transform Camera, Transform Target, Vector3 StartPos, Vector3 EndPos, Vector3 Offset)
    {
        camera = Camera;
        target = Target;
        startPos = StartPos;
        endPos = EndPos;
        offset = Offset;
    }

    public void Enter()
    {

    }

    public void Action()
    {
        Vector3 pos = target.position - new Vector3(offset.x, offset.y, offset.z);
        pos = new Vector3(Mathf.Clamp(pos.x, startPos.x, endPos.x), Mathf.Clamp(pos.y, startPos.y, endPos.y), Mathf.Clamp(pos.z, startPos.z, endPos.z));
        camera.position = Vector3.Lerp(camera.position, pos, lerpSpeed);
    }

    public void SetCurrentZone(CameraZone zone)
    {
        
    }
    public void SetTarget(Transform target)
    {
        this.target = target;
    }
}
