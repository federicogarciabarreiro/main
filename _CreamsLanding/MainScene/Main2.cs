using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Main2 : MonoBehaviour
{
    public Transform parallaxLimit;
    public float xToRestartParallax;

    void Update()
    {
        xToRestartParallax = parallaxLimit.transform.position.x;
      }
}
