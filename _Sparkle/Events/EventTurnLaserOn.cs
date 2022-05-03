using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTurnLaserOn : IEvent
{
    Transform laser;

    public void Enter()
    {
        laser = Config.me.laser00;
        laser.gameObject.SetActive(true);
        Camera.main.GetComponent<MainCamera>().SetFocusSpotCamera(new Vector3(10.84f, 4.6f, 108.65f), new Vector3(31.53f, -90,0), 60);
        Camera.main.GetComponent<MainCamera>().SetCameraMode(CameraMode.FocusSpot);
        Camera.main.GetComponent<MainCamera>().SetCameraMode(CameraMode.Level02_B, 0.5f);
        GameManager.me.em.SetEvent(GameEvent.None);
    }
    public void Action()
    {

    }


    public void Exit()
    {

    }

}