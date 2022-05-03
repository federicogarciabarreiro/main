using UnityEngine;
using System.Collections;

public class Level : MonoBehaviour {

    public GameObject cameraParallaxPointRestart;
   

    public float xToRestart;
  
    SpriteRenderer part1;
    SpriteRenderer part2;
    SpriteRenderer part3;
    SpriteRenderer part4;
    SpriteRenderer part1B;
    SpriteRenderer part2B;
    SpriteRenderer part3B;
    SpriteRenderer part4B;

    float speed = 3.8f; 
    
    void Start() {

        part1 = this.transform.FindChild("part1").GetComponent<SpriteRenderer>();
        part2 = this.transform.FindChild("part2").GetComponent<SpriteRenderer>();
        part3 = this.transform.FindChild("part3").GetComponent<SpriteRenderer>();
        part4 = this.transform.FindChild("part4").GetComponent<SpriteRenderer>();
        part1B = this.transform.FindChild("part1B").GetComponent<SpriteRenderer>();
        part2B = this.transform.FindChild("part2B").GetComponent<SpriteRenderer>();
        part3B = this.transform.FindChild("part3B").GetComponent<SpriteRenderer>();
        part4B = this.transform.FindChild("part4B").GetComponent<SpriteRenderer>();

                }
	
	void Update ()
                    {

        xToRestart = cameraParallaxPointRestart.GetComponent<Main>().xToRestartParallax;
        MoveLevel();

                    }

     void MoveLevel()
    {
        part1.transform.position += Vector3.left * speed * 1 * Time.deltaTime;
        part2.transform.position += Vector3.left * speed * 0.5f * Time.deltaTime;
        part3.transform.position += Vector3.left * speed * 0.25f * Time.deltaTime;
        part4.transform.position += Vector3.left * speed * 0.10f * Time.deltaTime;
        part1B.transform.position += Vector3.left * speed * 1 * Time.deltaTime;
        part2B.transform.position += Vector3.left * speed * 0.5f * Time.deltaTime;
        part3B.transform.position += Vector3.left * speed * 0.25f * Time.deltaTime;
        part4B.transform.position += Vector3.left * speed * 0.10f * Time.deltaTime;
   
       
        if (part1.transform.position.x + part1.bounds.size.x / 2 <= (xToRestart))
        {
            part1.transform.position = part1B.transform.position + Vector3.right * part1B.bounds.size.x;
        }

        if (part1B.transform.position.x + part1.bounds.size.x / 2 <= (xToRestart))
        {
            part1B.transform.position = part1.transform.position + Vector3.right * part1.bounds.size.x;
        }

        if (part2.transform.position.x + part2.bounds.size.x / 2 <= (xToRestart))
        {
            part2.transform.position = part2B.transform.position + Vector3.right * part2B.bounds.size.x;
        }

        if (part2B.transform.position.x + part2B.bounds.size.x / 2 <= (xToRestart))
        {
            part2B.transform.position = part2.transform.position + Vector3.right * part2.bounds.size.x;
        }

        if (part3.transform.position.x + part3.bounds.size.x / 2 <= (xToRestart))
        {
            part3.transform.position = part3B.transform.position + Vector3.right * part3B.bounds.size.x;
        }

        if (part3B.transform.position.x + part3B.bounds.size.x / 2 <= (xToRestart))
        {
            part3B.transform.position = part3.transform.position + Vector3.right * part3.bounds.size.x;
        }

        if (part4.transform.position.x + part4.bounds.size.x / 2 <= (xToRestart))
        {
            part4.transform.position = part4B.transform.position + Vector3.right * part4B.bounds.size.x;
        }

        if (part4B.transform.position.x + part4B.bounds.size.x / 2 <= (xToRestart))
        {
            part4B.transform.position = part4.transform.position + Vector3.right * part4.bounds.size.x;
        }
    }   

}
