using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour {

    LineRenderer lr;
    public Transform lineStart;
    public Transform lineEnd;

	void Start () {
        lr = GetComponent<LineRenderer>();
	}
	

	void Update () {
        lr.SetPosition(0, lineStart.position);
        RaycastHit hit;
        if (Physics.Raycast(lineStart.position, (lineEnd.position - lineStart.position).normalized, out hit))
        {
            if (hit.collider)
            {
                if (hit.collider.name == "Hero")
                    GameObject.Find("Hero").transform.position = new Vector3(2f, 2f, 72f);
                lr.SetPosition(1,hit.point);
            }

        }
        else lr.SetPosition(1, lineEnd.position); 
            
	}
}
