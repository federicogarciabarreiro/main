using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToxicWater : MonoBehaviour {

    //bool firstTimeCollideWithCube;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Hero>())
            other.GetComponent<Hero>().Die(Death.FrontHit);
        if (other.GetComponentInParent<CubeScifiLiftable>())
        {
            //if (!firstTimeCollideWithCube)
            //    GameManager.me.em.SetEvent(GameEvent.BoxFallsDown);
            other.transform.parent.GetComponent<CubeScifiLiftable>().MoveToStartPosition();
            //firstTimeCollideWithCube = true;
        }

        if (other.GetComponent<Fusile>())
        {
            print("Game Restarted");
            Config.me.hero.GetComponent<Hero>().Die(Death.FrontHit);
        }
    }
}
