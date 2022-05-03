using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RisingWater : MonoBehaviour {

    public int height = 0;
    Vector3 initialPos;
    float lerpSpeed = 0.0025f;

    private void Start()
    {
        initialPos = transform.position;
    }
    public void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, initialPos + Vector3.up * height * 1.2f, lerpSpeed);
    }

    public void SetWaterHeight(int amount)
    {
        height += amount;
    }


}
