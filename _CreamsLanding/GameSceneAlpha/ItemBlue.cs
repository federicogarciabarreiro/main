using UnityEngine;
using System.Collections;

public class ItemBlue : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Player")
        {
            Destroy(this.gameObject);

        }
    }
}