using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

    public float speed;
    public GameObject personaje;
   

	void Update () {
                this.transform.position += Vector3.left * speed * Time.deltaTime; //Direccion.-
                      }

    void OnTriggerEnter2D(Collider2D c)
                  {
                if (c.tag == "Bullet")
                   {
                     GameObject.Destroy(this.gameObject);

            personaje.gameObject.GetComponent<Score>().counter = personaje.gameObject.GetComponent<Score>().counter + 8f;
            
               }

                if (c.tag == "Player")
                   { Time.timeScale = 0; }
                      }
        }
   
