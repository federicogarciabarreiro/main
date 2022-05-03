using UnityEngine;
using System.Collections;

public class Generator : MonoBehaviour {


    public GameObject PJ; 
    public GameObject[] blocks; 
    public float timeToCreateBlocks;
    public float currentTimeToCreateBlocks;

    void Start () {
        currentTimeToCreateBlocks = 0f;
                  }
	
	void Update () {

        if (PJ.GetComponent<Rigidbody2D>().velocity.x >= 1f) 
                {
            UpdateTimerToCreateBlocks();
                }
    }

    void UpdateTimerToCreateBlocks()
    {
        currentTimeToCreateBlocks += Time.deltaTime;
        if (currentTimeToCreateBlocks >= timeToCreateBlocks)
        {
            createBlocks();
        }
    }

    void createBlocks()
    {
        currentTimeToCreateBlocks = 0;
        GameObject.Instantiate(blocks[Random.Range(0, blocks.Length)], this.transform.position, Quaternion.identity);

    }

}
