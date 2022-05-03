using UnityEngine;
using System.Collections;

public class ItemRed : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Player")
        {
            Destroy(this.gameObject);

        }
    }
}
