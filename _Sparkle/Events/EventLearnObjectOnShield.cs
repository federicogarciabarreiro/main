using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventLearnObjectOnShield : IEvent
{

    Hero hero;
    Transform target;

    public void Enter()
    {
        target = Config.me.targetBoxLearnShieldOnObject;
        string text = string.Format("<color=orange>{0}</color> on objects to <color=yellow>{1}</color> them up", Config.me.controlKeys[(int)ControlKey.Third].text, "SHIELD");
        GameManager.me.dm.AddDialog(text, Speaker.Info, () => GameManager.me.em.GetActionElapsed(ActionElapsed.FirstTimeShieldOnObject), 0);
        GameManager.me.em.SetEvent(GameEvent.None);
    }
    public void Action()
    {
        
    }


    public void Exit()
    {

    }

}