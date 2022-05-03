using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLayerOnlyHero : ICameraLayer
{

    public void Enter(Camera cam)
    {
        cam.cullingMask = Config.me.cameraLayerHeroAndCinematic;
    }

    public void Exit(Camera cam)
    {

    }
}