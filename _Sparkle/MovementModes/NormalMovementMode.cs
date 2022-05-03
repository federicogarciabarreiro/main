using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalMovementMode : IMovementMode
{
    public IGravityMode gravityMode {get;set;}
    CharacterController cc;
    public float initialSpeed;
    public float speed = 3;
    public Vector3 dir;
    public Vector3 addedForce;
    public Vector3 forceDir;
    public float jumpForce = 9;
    Hero hero;
    float turningSpeed = 0.1f;
    public NormalMovementMode(Hero Hero, CharacterController CC, Animator Anim, IGravityMode GravMode)
    {
        hero = Hero;
        initialSpeed = speed;
        cc = CC;
        gravityMode = GravMode;
    }

    public void Enter()
    {
        
    }

    public void Action(float inputH, float inputV)
    {
        gravityMode.Action();
        dir = new Vector3(-inputH * speed, dir.y, inputV * speed);
        addedForce *= 0.95f;
        dir.y += gravityMode.GetYDirection();


        if (hero.GetFreezedState() || hero.GetFreezedPositionState())
        {
            dir.x = 0;
            dir.z = 0;
        }
        else
            hero.transform.forward = Vector3.Slerp(hero.transform.forward, new Vector3(dir.x, 0, dir.z), turningSpeed);



        cc.Move((dir + addedForce + forceDir) * Time.deltaTime);

    }



    public void Exit()
    {
        
    }

    public IGravityMode GetGravityMode()
    {
        return gravityMode;
    }
}
