using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventLiftAndThrowObject : IEvent
{

    Hero hero;
    public void Enter()
    {
        string text = string.Format("<color=orange>{0}</color> on objects to <color=cyan>{1}</color> them up", Config.me.controlKeys[(int)ControlKey.First].text, "LIFT");
        GameManager.me.dm.AddDialog(text, Speaker.Info, ()=> GameManager.me.em.GetActionElapsed(ActionElapsed.FirstTimeLiftedObject), 0);
        hero = Config.me.hero.GetComponent<Hero>();
    }
    public void Action()
    {
        if (Input.GetKeyDown(Config.me.controlKeys[(int)ControlKey.First].key))
        {
            string text = string.Format("<color=orange>{0}</color> on other objects to <color=cyan>{1}</color> it at them", Config.me.controlKeys[(int)ControlKey.First].text, "THROW");
            GameManager.me.dm.AddDialog(text, Speaker.Info, () => GameManager.me.em.GetActionElapsed(ActionElapsed.FirstTimeThrownedObject), 0);
            GameManager.me.em.SetEvent(GameEvent.None);
        }
    }


    public void Exit()
    {

    }

}