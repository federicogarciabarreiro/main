using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectContainer : MonoBehaviour {

    private void OnCollisionEnter(Collision other)
    {
        if(other.transform.GetComponent<CubeScifiLiftable>())
        {
            Vector3 pos = transform.position;
            pos.y = transform.position.y + GetComponent<Collider>().bounds.size.y/2;
            other.transform.position = pos;
            other.transform.rotation = Quaternion.identity;
            other.transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
            other.transform.parent = transform;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.transform.GetComponent<CubeScifiLiftable>())
        {
            other.transform.parent = null;
        }
    }
}
