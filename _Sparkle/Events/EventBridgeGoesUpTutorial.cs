using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventBridgeGoesUp : IEvent
{
    public void Enter()
    {
        Camera.main.GetComponent<MainCamera>().SetFocusSpotCamera(new Vector3(9.54f, 3.6811f, -383.21f), new Vector3(49.435f, -90, 0), 60);
        Camera.main.GetComponent<MainCamera>().SetCameraMode(CameraMode.FocusSpot);
        Camera.main.GetComponent<MainCamera>().SetCameraMode(CameraMode.Tutorial, 1f);
        GameManager.me.em.SetEvent(GameEvent.None);
    }
    public void Action()
    {

    }


    public void Exit()
    {

    }

}