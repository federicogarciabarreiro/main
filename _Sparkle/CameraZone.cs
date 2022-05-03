using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZone : MonoBehaviour {


    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Hero>())
            Camera.main.GetComponent<MainCamera>().currentCameraMode.SetCurrentZone(this);
    }

}
