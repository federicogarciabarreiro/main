using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTrigger : MonoBehaviour {

    public GameEvent gameEvent;
    public bool exitLevel;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Hero>())
        {
            if (exitLevel)
                GameManager.me.ExitCurrentLevel();
            else
                GameManager.me.em.SetEvent(gameEvent);
            Destroy(gameObject);

        }
            
    }
}
