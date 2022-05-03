using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EventFromTutorialToLaboratory : IEvent
{

    Hero hero;
    float timer;
    int state;
    public void Enter()
    {
        string text = string.Format("Loading...");
        GameManager.me.dm.AddDialog(text, Speaker.Saved, () => true, 2);
        hero = Config.me.hero.GetComponent<Hero>();
    }
    public void Action()
    {
        if(state == 0)
        {
            HUDManager.me.BlackoutScreen(HUDManager.me.BlackBG, 0.02f);
            hero.Freeze(true);
            state++;
        }
        if(state == 1)
        {
            timer += Time.deltaTime;
            if(timer > 8)
            {
                timer = 0;
                state++;
            }
        }


        if (state == 2)
        {
            GameManager.me.Clear();
            SceneManager.LoadScene(2);
        }

    }


    public void Exit()
    {

    }

}