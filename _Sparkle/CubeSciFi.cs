using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSciFi : MonoBehaviour {

    public Vector3 rotationForce;
    public float speed;
    Vector3 startPos;
    Vector3 endPos;
    Vector3 targetPos;


    private void Start()
    {
        startPos = transform.position;
        endPos = transform.position + Vector3.up * 0.5f;
        targetPos = endPos;
    }

    private void Update()
    {
        transform.Rotate(rotationForce.x, rotationForce.y, rotationForce.z);
        if (transform.position == startPos)
            targetPos = endPos;
        if (transform.position == endPos)
            targetPos = startPos;
        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
    }
}
