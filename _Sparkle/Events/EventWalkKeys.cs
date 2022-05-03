using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EventWalkKeys : IEvent
{

    Hero hero;
    float timer;
    bool exit;
    bool sprinted;
    bool jumped;
    public void Enter()
    {
        string text = string.Format("Use <color=orange>{0}</color>, <color=orange>{1}</color>, <color=orange>{2}</color>, <color=orange>{3}</color> keys to walk around", Config.me.controlKeys[(int)ControlKey.Left].text, Config.me.controlKeys[(int)ControlKey.Up].text, Config.me.controlKeys[(int)ControlKey.Down].text, Config.me.controlKeys[(int)ControlKey.Right].text);
        GameManager.me.dm.AddDialog(text, Speaker.Info, () => Input.GetKeyDown(Config.me.controlKeys[(int)ControlKey.Left].key)
            || Input.GetKeyDown(Config.me.controlKeys[(int)ControlKey.Up].key)
            || Input.GetKeyDown(Config.me.controlKeys[(int)ControlKey.Down].key)
            || Input.GetKeyDown(Config.me.controlKeys[(int)ControlKey.Right].key), 0);
        hero = Config.me.hero.GetComponent<Hero>();
        //hero.SetMovementMode(MovementMode.None);
        exit = false;
        timer = 0;
    }
    public void Action()
    {

        if(timer > 2f && !sprinted)
        {
           // string text = string.Format("Hold <color=orange>{0}</color> to sprint", Config.me.controlKeys[(int)ControlKey.Boost].text);
            //GameManager.me.dm.AddDialog(text, Speaker.Info, () => Input.GetKeyDown(Config.me.controlKeys[(int)ControlKey.Boost].key), 0);
            sprinted = true;
        }

        if(timer > 6f && !jumped)
        {
            string text = string.Format("Press <color=orange>{0}</color> to jump", Config.me.controlKeys[(int)ControlKey.Jump].text);
            GameManager.me.dm.AddDialog(text, Speaker.Info, () => Input.GetKeyDown(Config.me.controlKeys[(int)ControlKey.Jump].key), 0);
            jumped = true;
        }

        if(timer > 12f)
        {
            //string text = string.Format("<color=orange>{0}</color> on objects to slow them down", Config.me.controlKeys[(int)ControlKey.Secondary].text);
            //GameManager.me.dm.AddDialog(text, Speaker.Info, () => GameManager.me.em.GetActionElapsed(ActionElapsed.FirstTimeSlowDownObject), 0);
            GameManager.me.em.SetEvent(GameEvent.None);
        }
        if (Input.GetKeyDown(Config.me.controlKeys[(int)ControlKey.Left].key) 
            || Input.GetKeyDown(Config.me.controlKeys[(int)ControlKey.Up].key)
            || Input.GetKeyDown(Config.me.controlKeys[(int)ControlKey.Down].key)
            || Input.GetKeyDown(Config.me.controlKeys[(int)ControlKey.Right].key))
        {
            //hero.SetMovementMode(MovementMode.Normal);
            exit = true;
        }
        if (exit)
            timer += Time.deltaTime;
    }


    public void Exit()
    {

    }

}
