using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventLiftPlatforms : IEvent
{
    Hero hero;
    IEnumerator currentCoroutine;

    public void Enter()
    {
        //hero = Config.me.hero.GetComponent<Hero>();
        //currentCoroutine = LiftPlatforms();
        //GameManager.me.StartCoroutine(currentCoroutine);
        Config.me.platformMovable.GetComponent<Platform>().SetAutomaticClosure(false);
    }
    public void Action()
    {

    }


    public void Exit()
    {
        //GameManager.me.StopCoroutine(currentCoroutine);
    }

    //IEnumerator LiftPlatforms()
    //{

    //    foreach (var item in Config.me.platformsToDropDown)
    //    {
    //        if (item.GetComponent<Platform>().finalPos[0].y < item.GetComponent<Platform>().finalPos[1].y)
    //            item.GetComponent<IDoor>().ReceiveInfo(0);
    //        else
    //            item.GetComponent<IDoor>().ReceiveInfo(1);
    //        item.GetComponent<IDoor>().OpenOnly();
    //        yield return new WaitForSeconds(0.15f);

    //    }

    //    Config.me.noLiftablePowerwall.gameObject.SetActive(false);
    //    GameManager.me.im.AddInteractionObject(Config.me.cubeLevel03.GetComponent<CubeScifiLiftable>(), Skill.Lift);
    //    Config.me.platformMovable.GetComponent<Platform>().automaticClosure = false;

    //    yield return new WaitUntil(() => hero.transform.position.x < -10f && hero.transform.position.z < 234f && hero.transform.position.z > 230f);

    //    Config.me.platformMovable.GetComponent<IDoor>().ReceiveInfo(0);
    //    Config.me.platformMovable.GetComponent<IDoor>().OpenOnly();
    //}

}