using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventPortalEnterCinematic: IEvent
{

    Hero hero;
    float timer;
    int state;
    public void Enter()
    {
        string text = string.Format("Loading...");
        GameManager.me.dm.AddDialog(text, Speaker.Saved, () => true, 5);
        hero = Config.me.hero.GetComponent<Hero>();
    }
    public void Action()
    {
        if (state == 0)
        {
            Camera.main.GetComponent<MainCamera>().SetCameraLayer(CameraLayer.OnlyHero);
            Camera.main.GetComponent<MainCamera>().SetCameraMode(CameraMode.Portal);
            hero.GetAnim().SetTrigger("Floating");
            hero.enabled = false;
            state++;
        }
        if (state == 1)
        {
            timer += Time.deltaTime;
            if (timer > 6f)
            {
                timer = 0;
                HUDManager.me.BlackoutScreen(HUDManager.me.WhiteBG, 0.01f);
                state++;
            }
        }

        if(state == 2)
        {
            timer += Time.deltaTime;
            if (timer > 1.5f)
            {
                timer = 0;
                Camera.main.GetComponent<MainCamera>().SetCameraMode(CameraMode.Level03);
                Camera.main.GetComponent<MainCamera>().SetCameraLayer(CameraLayer.None);
                state++;
            }
        }

        if (state == 3)
        {
            timer += Time.deltaTime;
            if (timer > 2.5f)
            {
                timer = 0;
                HUDManager.me.LightupScreen(HUDManager.me.WhiteBG, 0.01f);
                state++;
            }
        }
        if (state == 4)
        {

            timer += Time.deltaTime;
            if (timer > 2f)
            {
                timer = 0;
                state++;
            }
        }
        if (state == 5)
        {
            hero.transform.position = Config.me.portallvl02OutStartPos.position;
            hero.transform.forward = Config.me.portallvl02OutStartPos.forward;
            hero.enabled = true;
            hero.ApplyForce(hero.transform.forward * 13f);
            hero.RemoveForce(5f);
            hero.GetAnim().SetTrigger("HardLanding");
            
            hero.Freeze(true);

            state++;
        }

        if(state == 6)
        {
            timer += Time.deltaTime;
            if (timer > 2.5f)
            {
                timer = 0;
                Config.me.crystalPortalOutlvl2.gameObject.SetActive(false);
                hero.Freeze(false);
                //hero.transform.position = new Vector3(-0.58f, 0.451f, 177.85f);
                //hero.GetAnim().SetTrigger("IdleTrigger");

                state++;
            }
        }

    }


    public void Exit()
    {

    }

}