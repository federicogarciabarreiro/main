using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnergyShield : MonoBehaviour {

    float timer;
    Transform target;
    Hero hero;
    float originalAlpha;
    float fresnelWidth;
    Color originalColor;
    private void Awake()
    {
        hero = Config.me.hero.GetComponent<Hero>();
        originalAlpha = transform.Find("Sphere").GetComponent<Renderer>().material.GetColor("_MainColor").a;
        fresnelWidth = transform.Find("Sphere").GetComponent<Renderer>().material.GetFloat("_FresnelWidth");
        originalColor = transform.Find("Sphere").GetComponent<Renderer>().material.GetColor("_MainColor");
    }

    public void Activate()
    {
        timer = 0;
        StopAllCoroutines();
        StartCoroutine(DeactivateShield(8f));
        transform.Find("Sphere").GetComponent<Renderer>().material.SetFloat("_FresnelWidth", fresnelWidth);

    }

    public void StopShield()
    {
        StopAllCoroutines();
        StartCoroutine(DeactivateShield(0.1f));
    }

    public IEnumerator DeactivateShield(float time)
    {
        StartCoroutine(ShieldAlpha(time));
        yield return new WaitForSeconds(time);
        SwitchLayer(target);
        target = null;
        hero.SetShieldState(false);
    }

    public void SwitchLayer(Transform target)
    {
        if (target)
        {
            if (target.gameObject.layer == 14)
                StartCoroutine(ChangeLayer(13, target));
            if (target.gameObject.layer == 15)
                StartCoroutine(ChangeLayer(9, target));
        }

    }

    IEnumerator ChangeLayer(int layer, Transform target)
    {
        while (Physics.OverlapSphere(target.transform.position, 0.35f, Config.me.EnergyWallMask).Length != 0)
        {
            yield return new WaitForSeconds(0.01f);
        }

        target.gameObject.layer = layer;
        foreach (Transform item in target.transform)
        {
            item.gameObject.layer = layer;
        }
            
    }

    IEnumerator ShieldAlpha(float time)
    {
        
        transform.Find("Sphere").GetComponent<Renderer>().material.SetColor("_MainColor", originalColor);
        yield return new WaitForSeconds(time * 0.66f);
        transform.Find("Sphere").GetComponent<Renderer>().material.SetColor("_MainColor", Color.grey);
        while (transform.Find("Sphere").GetComponent<Renderer>().material.GetFloat("_FresnelWidth") > 0.03f)
        {
            transform.Find("Sphere").GetComponent<Renderer>().material.SetFloat("_FresnelWidth", transform.Find("Sphere").GetComponent<Renderer>().material.GetFloat("_FresnelWidth") -  0.01f);
            yield return new WaitForSeconds(0.025f);
        }
    }
    private void Update()
    {
        if(target)
            transform.position = target.transform.position;
        else
            transform.position = new Vector3(10000, 10000, 10000);
    }

    public void SetTarget(Transform Target)
    {
        target = Target;
        if (target.gameObject.layer == 13)
            target.gameObject.layer = 14;
        if (target.gameObject.layer == 9)
        {
            target.gameObject.layer = 15;
            foreach (Transform item in target.transform)
                item.gameObject.layer = 15;
        }
            
    }
}
