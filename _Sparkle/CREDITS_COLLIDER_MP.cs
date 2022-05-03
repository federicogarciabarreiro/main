using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CREDITS_COLLIDER_MP : MonoBehaviour {

    public Outline pc;
    public Transform credits;
    Vector3 cameraInitialPos;
    Quaternion cameraInitialRot;
    public Vector3 cameraTargetPos;
    public Vector3 cameraTargetRot;
    bool hasZoom;
    public bool isWall;
    public Transform canvas;

    private void Start()
    {
        cameraInitialPos = Camera.main.transform.position;
        cameraInitialRot = Camera.main.transform.rotation;
    }

    private void OnMouseDown()
    {
        hasZoom = !hasZoom;

    }

    private void Update()
    {
        if(hasZoom)
        {
            canvas.gameObject.SetActive(false);
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, cameraTargetPos, Time.deltaTime *2f);
            Camera.main.transform.rotation = Quaternion.Lerp(Camera.main.transform.rotation, Quaternion.Euler(cameraTargetRot), Time.deltaTime *2f);
        }
        else
        {
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, cameraInitialPos, Time.deltaTime * 2f);
            Camera.main.transform.rotation = Quaternion.Lerp(Camera.main.transform.rotation, cameraInitialRot, Time.deltaTime * 2f);
            if(Vector3.Distance(Camera.main.transform.position, cameraInitialPos) < 0.25f)
                canvas.gameObject.SetActive(true);
        }
    }
    IEnumerator ZoomCamera()
    {

        yield return new WaitForSeconds(1f);
    }

    private void OnMouseEnter()
    {
        print("ENTER");
        pc.OutlineWidth = 5f;
        credits.gameObject.SetActive(true);
    }
    private void OnMouseExit()
    {
        print("EXIT");
        pc.OutlineWidth = 0f;
        credits.gameObject.SetActive(false);
    }
}
