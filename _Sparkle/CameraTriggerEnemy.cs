using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTriggerEnemy : MonoBehaviour {

    public Transform newTarget;
    public float time;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Hero>())
        {
            StartCoroutine(SetNewTarget());
        }
    }

    IEnumerator SetNewTarget()
    {
        Camera.main.GetComponent<MainCamera>().SetLaboratoryTarget(newTarget);
        yield return new WaitForSeconds(time);
        Camera.main.GetComponent<MainCamera>().SetLaboratoryTarget(Config.me.hero);
        Destroy(gameObject);
    }
}
