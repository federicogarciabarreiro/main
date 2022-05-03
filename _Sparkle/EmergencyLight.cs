using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmergencyLight : MonoBehaviour {

    public float distance;
    public Transform target;

    void Update () {

        distance = Vector3.Distance(target.position, transform.position);
        Vector3 destination = (target.position - transform.position).normalized;
        if (distance < 7.5f)
            transform.forward = Vector3.Slerp(transform.forward, destination, 0.075f);
        else
            transform.forward = Vector3.Slerp(transform.forward, Vector3.up, 0.005f);
    }
}
