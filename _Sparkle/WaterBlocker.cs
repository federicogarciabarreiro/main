using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBlocker : MonoBehaviour {

    public GameObject waterToPullUp;
    public bool turnON;
    Vector3 initialPos;
    Vector3 endPos;
    float lerpSpeed = 0.03f;

	void Start () {

        initialPos = transform.position;
        endPos = transform.position - transform.forward;
	}
	
	void Update () {

        if(turnON)
        {
            transform.position = Vector3.Lerp(transform.position, endPos, lerpSpeed);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, initialPos, lerpSpeed);
        }
	}

    public void SwitchState()
    {
        turnON = !turnON;
        if (turnON) waterToPullUp.GetComponent<RisingWater>().SetWaterHeight(1);
        else waterToPullUp.GetComponent<RisingWater>().SetWaterHeight(-1);
    }
}
