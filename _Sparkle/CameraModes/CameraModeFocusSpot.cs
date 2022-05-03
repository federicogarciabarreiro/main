using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraModeFocusSpot : ICameraMode
{

    Transform camera;
    public Vector3 pos;
    public Vector3 rot;
    public float fieldOfView;

    public CameraZone currentZone { get; set; }

    public CameraModeFocusSpot(Transform Camera)
    {
        camera = Camera;
    }

    public void Enter()
    {
        camera.position = pos;
        camera.rotation = Quaternion.Euler(rot.x, rot.y, rot.z);
        camera.GetComponent<Camera>().fieldOfView = fieldOfView;

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
        
    }
}
