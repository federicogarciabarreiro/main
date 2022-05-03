using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenDoorBrain : MonoBehaviour {

    public BrokenDoor door;
    float timer;
    public float speed;
    public bool brainActivated;
    public Vector3 endPos;
    public bool setNewPosition;


	void Start () {
        door.SetSpeed(speed);
        Vector3 startPos = transform.localPosition;
        if(setNewPosition) door.SetPositions(startPos , endPos);

    }


    void Update () {
        if(brainActivated)
        {
            timer += Time.deltaTime;
            if (timer > 0.2f)
            {
                door.Open();
                timer = 0;
            }
        }

	}
}
