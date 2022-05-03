using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DyingEnemy : MonoBehaviour {

    public Transform pelvis;

	void Start () {

        Destroy(gameObject, 5f);
        pelvis.GetComponent<Rigidbody>().AddExplosionForce(2000, Config.me.hero.transform.position - Vector3.up * 3, 50);

    }
}
