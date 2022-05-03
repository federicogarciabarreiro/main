using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CameraModeGeneric: ICameraMode
{

    Transform camera;
    public Transform target;
    public Collider enemyTarget;
    Vector3 startPos;
    Vector3 endPos;
    Vector3 offset;
    float lerpSpeed = 0.025f;
    Quaternion finalRotation;
    float fieldOfView;

    public CameraZone currentZone { get; set; }

    public CameraModeGeneric(Transform Camera, Transform Target, Vector3 StartPos, Vector3 EndPos, Vector3 Offset, Quaternion FinalRotation, float FieldOfView)
    {
        camera = Camera;
        target = Target;
        startPos = StartPos;
        endPos = EndPos;
        offset = Offset;
        finalRotation = FinalRotation;
        fieldOfView = FieldOfView;
    }

    public void Enter()
    {

    }

    public void Action()
    {
        enemyTarget = Physics.OverlapSphere(target.position, 11f, Config.me.enemyLayer).FirstOrDefault();
        if(enemyTarget)
        {
            RaycastHit hit;
            if (Physics.Raycast(enemyTarget.transform.position, (target.transform.position - enemyTarget.transform.position).normalized, out hit, 11f, Config.me.heroAndWallsMask))
            {
                if (hit.transform != target.transform)
                    enemyTarget = null;
            }
        }

        Vector3 pos = Vector3.zero;
        if(enemyTarget)
        {
            pos = target.position + (enemyTarget.transform.position - target.position)/2;
            pos.x = target.position.x;
            pos -= new Vector3(offset.x, offset.y, offset.z);
            pos = new Vector3(Mathf.Clamp(pos.x, startPos.x, endPos.x), Mathf.Clamp(pos.y, startPos.y, endPos.y), Mathf.Clamp(pos.z, startPos.z, endPos.z));
        }
        else
        {
            pos = target.position - new Vector3(offset.x, offset.y, offset.z);
            pos = new Vector3(Mathf.Clamp(pos.x, startPos.x, endPos.x), Mathf.Clamp(pos.y, startPos.y, endPos.y), Mathf.Clamp(pos.z, startPos.z, endPos.z));
        }

        camera.GetComponent<Camera>().fieldOfView = Mathf.Lerp(camera.GetComponent<Camera>().fieldOfView, fieldOfView, Time.deltaTime);
        camera.position = Vector3.Lerp(camera.position, pos, lerpSpeed);
        camera.rotation = Quaternion.Lerp(camera.rotation, finalRotation, 0.025f);
    }

    public void SetCurrentZone(CameraZone zone)
    {
        
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }
}