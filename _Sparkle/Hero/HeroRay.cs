using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroRay : MonoBehaviour {


    public bool ShootRay()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up * 0.25f, -transform.up, out hit, 2, Config.me.floorLayerMask))
        {
            //print(hit.distance);
        }
        if (hit.distance < 0.5f)
            return true;
        else
            return false;
    }
}
