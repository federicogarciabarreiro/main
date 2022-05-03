using UnityEngine;
using System.Collections;
using System;

public class Block : MonoBehaviour, IPoolable
{
    public void OnAcquire()
    {
		gameObject.SetActive(true);
	}

    public void OnRelease()
    {
        gameObject.SetActive(false);    
    }

	public bool CanRecycle()
    {
        return Config.instance.playerTransform.position.z - 10 > transform.position.z;
    }
}
