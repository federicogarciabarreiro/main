using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTriggerB : MonoBehaviour {

    public CameraMode cameraModeToEnter;
    public CameraMode cameraModeToExit;
    public float delayToEnter;
    public float delayToExit;
    public float freezeHeroTime;
    public bool selfDestruct;
    public bool deactivateShield;
    public List<Transform> objectsToDeactivate = new List<Transform>();


    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Hero>())
        {
            Camera.main.GetComponent<MainCamera>().SetCameraMode(cameraModeToEnter, delayToEnter);
            StartCoroutine(FreezeHero(freezeHeroTime));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Hero>())
        {
            Camera.main.GetComponent<MainCamera>().SetCameraMode(cameraModeToExit, delayToExit);
        }
    }

    public IEnumerator FreezeHero(float time)
    {
        if (deactivateShield)
            Config.me.energyShield.GetComponent<EnergyShield>().StopShield();
        Config.me.hero.GetComponent<Hero>().Freeze(true);
        yield return new WaitForSeconds(time);
        Config.me.hero.GetComponent<Hero>().Freeze(false);
        foreach (var item in objectsToDeactivate)
            item.gameObject.SetActive(false);
        if (selfDestruct)
            Destroy(gameObject);
            
    }
}
