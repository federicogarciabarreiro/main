using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserDrone : MonoBehaviour {

    public GameObject laserPrefab;
    public float lasersLineLenght;
    public List<Transform> myLasersEnds;
    public float laserLenght;
    public float offset;
    public int amountOfLasers;
    float timer;
    float dir = 1;


	void Start () {
        myLasersEnds = SpawnLasers(lasersLineLenght, amountOfLasers, laserPrefab);
	}
	
	
	void Update () {
        timer += Time.deltaTime/2 * dir;

        if (timer > 1 || timer < -1)
            dir *= -1;

        foreach (var item in myLasersEnds)
        {
            item.transform.position += transform.forward * 5 * dir * Time.deltaTime; 
        }
	}

    List<Transform> SpawnLasers(float lasersLineLenght, int amount, GameObject prefab)
    {
        float laserFraction = lasersLineLenght / amount;
        List<Transform> l = new List<Transform>();
        for (int i = 0; i < amount; i++)
        {
            GameObject mylaser = Instantiate(prefab);
            mylaser.transform.parent = transform;
            mylaser.transform.localPosition = -transform.position;
            mylaser.transform.GetChild(0).transform.localPosition = transform.position;
            mylaser.transform.GetChild(1).transform.localPosition =  transform.position - (Vector3.up * laserLenght) +  (transform.right * laserFraction * i);
            mylaser.transform.GetChild(1).transform.localPosition = new Vector3(transform.position.x + offset + (laserFraction * i), 0, mylaser.transform.GetChild(1).transform.localPosition.z);
            l.Add(mylaser.transform.GetChild(1));
        }

        return l;
    }

    
}
