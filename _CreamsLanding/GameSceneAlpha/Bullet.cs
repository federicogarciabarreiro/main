using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
    public float speed;

    void Update()
    {
        this.transform.position += this.transform.up * speed * Time.deltaTime; 
    }

    void OnTriggerEnter2D(Collider2D c)
    {
        if (c.tag != "Player")

        { GameObject.Destroy(this.gameObject);
                   }
   
    }
}
