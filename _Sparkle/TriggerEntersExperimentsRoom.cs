using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEntersExperimentsRoom : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Hero>())
        {
            gameObject.SetActive(false);
        }
            
    }
}
