using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeBubbleSphere : MonoBehaviour {

    public bool activated;

    void Update()
    {
        if (activated)
            transform.position = GameObject.Find("Hero").transform.position;
        else
            transform.position = new Vector3(1000, 1000, 1000);
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 10)
            other.GetComponent<IFreezable>().Freeze();
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 10)
            other.GetComponent<IFreezable>().Unfreeze();
    }
}
