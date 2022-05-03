using UnityEngine;
using System.Collections;

public class Destroyer : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D c) 
        
      
    {
        if (c.tag == "Player")
        { Time.timeScale = 0; }
        else
        { GameObject.Destroy(c.gameObject); }
       
    }
}
