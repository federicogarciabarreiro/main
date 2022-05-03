using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventFromLaboratoryToSandBox : IEvent
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
        Debug.Log(state);
        if (state == 0)
        {
            HUDManager.me.BlackoutScreen(HUDManager.me.BlackBG, 0.02f);
            hero.Freeze(true);
            state++;
        }
        if (state == 1)
        {
            timer += Time.deltaTime;
            if (timer > 4)
            {
                timer = 0;
                state++;
            }
        }
        if (state == 2)
        {
            hero.transform.position = Config.me.sandBoxStartPos.position;
            hero.transform.forward = Config.me.sandBoxStartPos.forward;
            foreach (var item in hero.heroSkills)
                item.Exit();
            hero.StopHandsInteractions();
            Camera.main.transform.position = hero.transform.position;
            state++;
        }
        if(state == 3)
        {
            timer += Time.deltaTime;
            if (timer > 0.5f)
            {
                timer = 0;
                state++;
            }
        }
        if (state == 4)
        {
            Camera.main.GetComponent<MainCamera>().SetCameraMode(CameraMode.Level02_A);
            Camera.main.transform.position = hero.transform.position;
            //string text = string.Format("Press <color=orange>{0}</color> to switch between skills", "Q");
            //GameManager.me.dm.AddDialog(text, Speaker.Info, () => Input.GetKeyDown(KeyCode.Q), 8);
            HUDManager.me.LightupScreen(HUDManager.me.BlackBG, 0.01f);
            hero.Freeze(false);
            GameManager.me.SetLevel(2);
            GameManager.me.em.SetEvent(GameEvent.None);
        }

    }


    public void Exit()
    {

    }

}