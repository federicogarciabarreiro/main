using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour {

    public Transform door;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Hero>())
        {
            door.GetComponent<IDoor>().Open();
        }

    }
}
