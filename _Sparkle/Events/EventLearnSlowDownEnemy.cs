using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventLearnSlowDownEnemy : IEvent
{

    Hero hero;
    public void Enter()
    {
        string text = string.Format("<color=orange>{0}</color> on enemies to <color=magenta>{1}</color> them down", Config.me.controlKeys[(int)ControlKey.Second].text, "SLOW");
        GameManager.me.dm.AddDialog(text, Speaker.Info, () => GameManager.me.em.GetActionElapsed(ActionElapsed.FirstTimeSlowDownEnemy), 0);
        hero = Config.me.hero.GetComponent<Hero>();
        hero.FreezePosition(true);
        hero.transform.forward = Vector3.forward;
        hero.SetSkill(Skill.None, KeyCode.A);
    }
    public void Action()
    {
        if (GameManager.me.em.GetActionElapsed(ActionElapsed.FirstTimeSlowDownEnemy))
        {
            hero.FreezePosition(false);
            hero.SetSkill(Skill.Lift, KeyCode.Mouse0);
            GameManager.me.em.SetEvent(GameEvent.None);
        }
    }


    public void Exit()
    {

    }

}