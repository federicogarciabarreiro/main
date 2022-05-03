using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectAffectedByTelekinesis : MonoBehaviour {

    Rigidbody rb;
    public Transform pivot;
    public bool isAffectedByTelekinesis = true;

	void Start () {
        rb = GetComponent<Rigidbody>();
	}

    private void Update()
    {
        if (isAffectedByTelekinesis)
        {
            transform.Rotate(0.5f, 1f, 0);
            transform.RotateAround(pivot.position, Vector3.up, 1f);
        }
            
    }

    public void DeactivateEffect()
    {
        isAffectedByTelekinesis = false;
        rb.isKinematic = false;
    }
}
