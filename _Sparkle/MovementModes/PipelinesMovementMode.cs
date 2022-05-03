using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PipelinesMovementMode : IMovementMode
{
    public Image icon;
    public Image icon_2;
    public IGravityMode gravityMode { get; set; }
    public List<PipelinesWaypoint> waypoints;
    int currentPos;
    Transform heroGraphic;
    public Transform entrance;
    public Transform exit;
    Hero hero;
    float speed = 5;
    bool eventTriggered;

    public PipelinesMovementMode(Hero Hero, Transform HeroGraphic, Animator Anim, IGravityMode GravMode)
    {
        hero = Hero;
        heroGraphic = HeroGraphic;
        gravityMode = GravMode;
    }

    public void Enter()
    {
        heroGraphic.gameObject.SetActive(false);
        icon = GameObject.Find("Canvas").transform.Find("heroIcon").GetComponent<Image>();
        icon_2 = GameObject.Find("Canvas").transform.Find("heroIcon_2").GetComponent<Image>();
        icon.gameObject.SetActive(true);
        icon_2.gameObject.SetActive(true);
    }

    public void Settings(bool isEntrance)
    {
        if (isEntrance)
        {
            currentPos = 0;
            hero.transform.position = waypoints[currentPos].transform.position;
        }
            
        else
        {
            currentPos = waypoints.Count - 1;
            hero.transform.position = waypoints[currentPos].transform.position;
        }
            
    }


    public void Action(float inputH, float inputV)
    {
        gravityMode.Action();

        icon.transform.position = Camera.main.WorldToScreenPoint(hero.transform.position + Vector3.up *1.1f);

        if (hero.transform.position == waypoints[0].transform.position || hero.transform.position == waypoints[waypoints.Count - 1].transform.position)
            icon_2.transform.position = Camera.main.WorldToScreenPoint(hero.transform.position + Vector3.up * 1.1f);
        else
            icon_2.transform.position = Camera.main.WorldToScreenPoint(hero.transform.position + Vector3.up * 1000f);

        if (Input.GetAxisRaw("Horizontal") == 1 && currentPos <= waypoints.Count - 1)
        {
            int dirPos = currentPos + 1;
            if (dirPos > waypoints.Count - 1) dirPos = waypoints.Count - 1;
            hero.transform.position = Vector3.MoveTowards(hero.transform.position, waypoints[dirPos].transform.position, speed * Time.deltaTime);
            if (hero.transform.position == waypoints[dirPos].transform.position)
                currentPos++;
            if (currentPos > waypoints.Count - 1) currentPos = waypoints.Count - 1;
        }
        if(Input.GetAxisRaw("Horizontal") == -1 && currentPos >= 0)
        {
            int dirPos = currentPos;
            if (dirPos < 0) dirPos = 0;
            hero.transform.position = Vector3.MoveTowards(hero.transform.position, waypoints[dirPos].transform.position, speed * Time.deltaTime);
            if (hero.transform.position == waypoints[dirPos].transform.position)
                currentPos--;
            if (currentPos < 0) currentPos = 0;
        }

        if (currentPos == 0 && hero.transform.position == waypoints[0].transform.position && Input.GetKeyDown(Config.me.controlKeys[(int)ControlKey.First].key))
        {
            hero.transform.position = entrance.transform.position - Vector3.up;
            hero.SetMovementMode(MovementMode.Normal);
        }

        if (!eventTriggered)
        {
            if (currentPos == waypoints.Count - 1)
            {
                eventTriggered = true;
            }
        }
        else
        {
            if (currentPos == waypoints.Count - 1 && hero.transform.position == waypoints[waypoints.Count - 1].transform.position && Input.GetKeyDown(Config.me.controlKeys[(int)ControlKey.First].key))
            {
                hero.transform.position = exit.transform.position - Vector3.up;
                hero.SetMovementMode(MovementMode.Normal);
            }
        }


    }

    public void Exit()
    {
        heroGraphic.gameObject.SetActive(true);
        icon.gameObject.SetActive(false);
        icon_2.gameObject.SetActive(false);
    }

    public IGravityMode GetGravityMode()
    {
        return gravityMode;
    }
}
