using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipelinesEntrance : MonoBehaviour {

    public Transform exit;
    public bool isEntrance;

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Hero>())
        {
            other.GetComponent<Hero>().SetMovementMode(MovementMode.Pipelines);
            List<PipelinesWaypoint> list = new List<PipelinesWaypoint>();
            foreach (Transform child in transform.parent.Find("WayPoints"))
                list.Add(child.GetComponent<PipelinesWaypoint>());
            other.GetComponent<Hero>().ConfigPipelinesMovementMode(list, transform.parent.Find("Entrance"), transform.parent.Find("Exit"), isEntrance);
        }
            

    }
}
