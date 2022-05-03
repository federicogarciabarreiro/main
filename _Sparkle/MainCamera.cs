using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CameraMode
{
    Lineal,
    Behind,
    Target,
    FocusSpot,
    Tutorial,
    Level01,
    Level01_LASERS,
    Level02_A,
    Level02_B,
    Level02_C,
    Portal,
    Level03,
    Level03_B,
    Level03_C
}

public enum CameraLayer
{
    None,
    OnlyHero
}

public class MainCamera : MonoBehaviour {

    public ICameraMode currentCameraMode;
    List<ICameraMode> listOfCameraModes = new List<ICameraMode>();
    public CameraMode cameraMode;
    public ICameraLayer currentCameraLayer;
    List<ICameraLayer> listOfCameraLayers = new List<ICameraLayer>();
    public CameraLayer cameraLayer;

    void Start()
    {
        transform.position = Config.me.hero.position;
        listOfCameraModes.Add(new CameraModeLineal(transform, Config.me.hero, new Vector3(10, -3, -155), new Vector3(12, 7, 160), new Vector3(-8, -4, 0)));
        listOfCameraModes.Add(new CameraModeBehind(transform, Config.me.hero));
        listOfCameraModes.Add(new CameraModeTarget(transform, null, Vector3.zero));
        listOfCameraModes.Add(new CameraModeFocusSpot(transform));
        //TUTORIAL
        listOfCameraModes.Add(new CameraModeGeneric(transform, Config.me.hero, new Vector3(4f, 3.8f, -428.0f), new Vector3(11.15f, 3.8f, -349.3f), new Vector3(-8, -4, 0), Quaternion.Euler(32f, -90, 0), 60f));
        //LEVEL01
        listOfCameraModes.Add(new CameraModeGeneric(transform, Config.me.hero, new Vector3(4f, 3.8f, -326.22f), new Vector3(11.15f, 3.8f, -225.57f), new Vector3(-8, -4, 0), Quaternion.Euler(32f, -90, 0), 60f));
        //LEVEL01_LASERS
        listOfCameraModes.Add(new CameraModeGeneric(transform, Config.me.hero, new Vector3(11f, 0.8f, -326.22f), new Vector3(11f, 0.8f, -225.57f), new Vector3(-8, -4, 0), Quaternion.Euler(17f, -90, 0), 60f));
        //LEVEL02_A
        listOfCameraModes.Add(new CameraModeGeneric(transform, Config.me.hero, new Vector3(4f, 3.8f, -12.8f), new Vector3(10.8f, 7.66f, 93.43f), new Vector3(-8, -4, 0), Quaternion.Euler(32f, -90, 0), 60f));
        //LEVEL02_B
        listOfCameraModes.Add(new CameraModeGeneric(transform, Config.me.hero, new Vector3(4f, 3.8f, 104.99f), new Vector3(10.8f, 7.66f, 112.18f), new Vector3(-8, -4, 0), Quaternion.Euler(32f, -90, 0), 60f));
        //LEVEL02_C
        listOfCameraModes.Add(new CameraModeGeneric(transform, Config.me.hero, new Vector3(4f, 3.8f, 124.77f), new Vector3(10.8f, 7.66f, 129.21f), new Vector3(-8, 4.54f, 0), Quaternion.Euler(20f, -90, 0), 60f));
        //Portal
        listOfCameraModes.Add(new CameraModePortal(transform, Config.me.hero, new Vector3(1f, 3.8f, 124.77f), new Vector3(1f, 7.66f, 129.21f), new Vector3(-8, 4.54f, 0), Quaternion.Euler(20f, -90, 0)));
        //LEVEL03
        listOfCameraModes.Add(new CameraModeGeneric(transform, Config.me.hero, new Vector3(-48.47f, 3.8f, 154.9f), new Vector3(5.65f, 7.2f, 238.9f), new Vector3(-6, -4f, 0), Quaternion.Euler(32f, -90, 0), 60f));
        //LEVEL03_B
        listOfCameraModes.Add(new CameraModeGeneric(transform, Config.me.hero, new Vector3(-37.52f, 7.46f, 183.3f), new Vector3(-16f, 7.46f, 183.3f), new Vector3(0, -4f, 0), Quaternion.Euler(80f, -90, 0), 60f));
        //LEVEL03_C
        listOfCameraModes.Add(new CameraModeGeneric(transform, Config.me.hero, new Vector3(-85.6f, 3.8f, 182.97f), new Vector3(-82.16f, 3.8f, 182.97f), new Vector3(-6, -4f, 0), Quaternion.Euler(32f, -90, 0), 75f));



        listOfCameraLayers.Add(new CameraLayerNone());
        listOfCameraLayers.Add(new CameraLayerOnlyHero());

        SetCameraMode(cameraMode);
        SetCameraLayer(cameraLayer);

    }

    void Update ()
    {
        Action();
    }

    public void SetCameraMode(CameraMode mode, float time = 0)
    {
        StartCoroutine(SettingCamera(mode, time));
    }

    public IEnumerator SettingCamera(CameraMode mode, float time)
    {
        yield return new WaitForSeconds(time);
        currentCameraMode = listOfCameraModes[(int)mode];
        currentCameraMode.Enter();
    }

    public void SetTargetCamera(Transform target, Vector3 offset)
    {
        var mode = listOfCameraModes[(int)CameraMode.Target] as CameraModeTarget;
        mode.target = target;
        mode.offset = offset;
    }

    public void SetTarget(Transform target)
    {
        currentCameraMode.SetTarget(target);
    }

    public void SetLaboratoryTarget(Transform target)
    {
        var mode = listOfCameraModes[(int)CameraMode.Level01] as CameraModeGeneric;
        mode.target = target;
    }

    public void SetFocusSpotCamera(Vector3 pos, Vector3 rot, float fieldOfView)
    {
        var mode = listOfCameraModes[(int)CameraMode.FocusSpot] as CameraModeFocusSpot;
        mode.pos = pos;
        mode.rot = rot;
        mode.fieldOfView = fieldOfView;
    }

    public void SetCameraLayer(CameraLayer mode)
    {
        if (currentCameraLayer != null)
            currentCameraLayer.Exit(GetComponent<Camera>());
        currentCameraLayer = listOfCameraLayers[(int)mode];
        GetComponent<Camera>().cullingMask = Config.me.anyLayer;
        currentCameraLayer.Enter(GetComponent<Camera>());
    }

    void Action()
    {
        if(currentCameraMode != null)
            currentCameraMode.Action();
    }
}
