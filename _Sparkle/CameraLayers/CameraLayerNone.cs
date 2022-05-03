using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLayerNone : ICameraLayer
{

    public void Enter(Camera cam)
    {
        cam.cullingMask = Config.me.cameraLayerEverythingButCinematic;
    }

    public void Exit(Camera cam)
    {

    }
}