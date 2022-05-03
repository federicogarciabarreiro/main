using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrigger : MonoBehaviour {

    public Vector3 cameraPosition;
    public Vector3 cameraRotation;
    public float fieldOfView;
    float originalFieldOfView;
    public float exitTime;
    public CameraMode modeToExit;
    public bool selfDestructive;

    private void Start()
    {
        originalFieldOfView = Camera.main.fieldOfView;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Hero>())
        {
            Camera.main.GetComponent<MainCamera>().SetFocusSpotCamera(cameraPosition, cameraRotation, fieldOfView);
            Camera.main.GetComponent<MainCamera>().SetCameraMode(CameraMode.FocusSpot);
            if(exitTime > 0)
            {
                StartCoroutine(Exit(modeToExit));
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Hero>())
        {
            Camera.main.fieldOfView = originalFieldOfView;
            Camera.main.GetComponent<MainCamera>().SetCameraMode(CameraMode.Level01);
        }
    }

    IEnumerator Exit(CameraMode mode)
    {
        yield return new WaitForSeconds(0.5f);
        Camera.main.fieldOfView = originalFieldOfView;
        Camera.main.GetComponent<MainCamera>().SetCameraMode(mode);
        if (selfDestructive)
            Destroy(gameObject);
    }
}
