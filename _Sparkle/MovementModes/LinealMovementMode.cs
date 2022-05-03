using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinealMovementMode : IMovementMode
{
    public IGravityMode gravityMode { get; set; }
    CharacterController cc;
    public float initialSpeed;
    public float speed = 3;
    public Vector3 dir;
    public float jumpForce = 9;
    Hero hero;
    float turningSpeed = 0.35f;
    Animator anim;
    //GameObject indicator;
    Vector3 forwardDir;
    public LinealMovementMode(Hero Hero, CharacterController CC, Animator Anim, IGravityMode GravMode)
    {
        hero = Hero;
        anim = Anim;
        initialSpeed = speed;
        cc = CC;
        gravityMode = GravMode;
        //indicator = GameObject.Find("Indicator");

    }

    public void Enter()
    {
        //indicator.SetActive(true);
        anim.SetFloat("SpeedX", 0);
        anim.SetBool("onGround", true);
    }

    public void Action(float inputH, float inputV)
    {
        //gravityMode.Action();
        //dir = new Vector3(-inputH * speed, dir.y, inputV * speed);
        //dir.y += gravityMode.GetYDirection();

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000f, Config.me.floorAndEnemies))
        {
            //indicator.transform.position = hit.point;
            if (hit.transform.gameObject.layer == 10)
            {
                //indicator.GetComponent<Renderer>().material.color = Color.green;
                forwardDir = (hit.transform.position - hero.transform.position).normalized;
                //CursorManager.me.SetCursorType(CursorTextureType.AIM_GREEN);
            }
            else
            {
                //indicator.GetComponent<Renderer>().material.color = Color.red;
                forwardDir = (hit.point - hero.transform.position).normalized;
                //CursorManager.me.SetCursorType(CursorTextureType.AIM_WHITE);
            }  
        }

        //if (Physics.Raycast(ray, out hit, Config.me.enemyLayer))
        //{
        //    indicator.transform.position = hit.point;
        //}
        //else
        //{
        //    indicator.transform.position = new Vector3(1000, 1000, 1000); 
        //}

        hero.transform.forward = Vector3.Slerp(hero.transform.forward, new Vector3(forwardDir.x, 0, forwardDir.z), turningSpeed);
        cc.Move(dir * Time.deltaTime);
    }



    public void Exit()
    {
        //indicator.SetActive(false);
        //CursorManager.me.SetCursorType(CursorTextureType.NORMAL_WHITE);
    }

    public IGravityMode GetGravityMode()
    {
        return gravityMode;
    }
}
