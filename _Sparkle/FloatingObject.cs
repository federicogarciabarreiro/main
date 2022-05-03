using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingObject : MonoBehaviour {

    public bool floats;
    Vector3 initialPos;
    float timer;
    float height;

	void Start () {
        initialPos = transform.position;
        height = GetComponent<Collider>().bounds.size.y;
        transform.position -= Vector3.up * height / 2;
    }
	
	void Update () {
        if(floats)
        {
            timer += Time.deltaTime;
            Vector3 pos = transform.position;
            pos.y += Mathf.Sin(timer) * Time.deltaTime / 3;
            transform.position = pos;
            transform.Rotate(0, 0, 1f * Time.deltaTime * 10);
        }

	}
}
