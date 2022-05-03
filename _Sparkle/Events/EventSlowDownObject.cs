using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSlowDownObject : IEvent
{

    Hero hero;
    public void Enter()
    {
        string text = string.Format("<color=orange>{0}</color> on objects to <color=magenta>{1}</color> them down", Config.me.controlKeys[(int)ControlKey.Second].text, "SLOW");
        GameManager.me.dm.AddDialog(text, Speaker.Info, () => GameManager.me.em.GetActionElapsed(ActionElapsed.FirstTimeSlowDownObject), 0);
        hero = Config.me.hero.GetComponent<Hero>();
        GameManager.me.em.SetEvent(GameEvent.None);
    }
    public void Action()
    {

    }


    public void Exit()
    {

    }

}