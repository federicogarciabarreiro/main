using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventLearnMindControl : IEvent
{

    Hero hero;

    public void Enter()
    {
        string text = string.Format("<color=orange>{0}</color> on enemies to <color=yellow>{1}</color> them", Config.me.controlKeys[(int)ControlKey.Forth].text, "MIND CONTROL");
        GameManager.me.dm.AddDialog(text, Speaker.Info, () => GameManager.me.em.GetActionElapsed(ActionElapsed.FirstTimeMindControl), 0);
        GameManager.me.em.SetEvent(GameEvent.None);
    }


    public void Action()
    {
        
    }


    public void Exit()
    {

    }

}