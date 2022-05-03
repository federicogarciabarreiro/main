using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Hero>())
        {
            GameManager.me.SaveCheckpoint();
            GameManager.me.dm.AddDialog("Game saved successfully!", Speaker.Saved, () => true, 8);
            Destroy(gameObject);
        }
    }
}
