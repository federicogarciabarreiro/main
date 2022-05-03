using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragMovementMode : IMovementMode {

    public IGravityMode gravityMode { get; set; }
    CharacterController cc;
    Animator anim;
    Hero hero;
    Vector3 dir;
    float speed = 3;
    public DragMovementMode(Hero Hero, CharacterController CC, Animator Anim, IGravityMode GravMode)
    {
        cc = CC;
        hero = Hero;
        anim = Anim;
        gravityMode = GravMode;
    }

    public void Enter()
    {

    }

    public void Action(float inputH, float inputV)
    {
        gravityMode.Action();
        var currentIO = hero.GetIO();

        dir = new Vector3(-inputH * speed, dir.y, inputV * speed);
        dir.y += gravityMode.GetYDirection();

        var distance = Vector3.Distance(currentIO.transform.position, hero.transform.position);

        hero.transform.forward = new Vector3(hero.transform.forward.x, 0, hero.transform.forward.z);
        currentIO.GetComponent<Box>().Move(dir);

        if (distance > 1.25f)
        {
            currentIO.GetComponent<Box>().Move(Vector3.zero);
            hero.SetMovementMode(MovementMode.Normal);
            hero.StopInteraction(currentIO);
        }
        if (inputH == 0 && inputV == 0)
            currentIO.GetComponent<Box>().Move(Vector3.zero);

        ForwardProximity(inputH, inputV);

        if (hero.GetIS().GetClosestTriggerIndex() == -1 && currentIO != null)
        {
            hero.SetMovementMode(MovementMode.Normal);
            hero.StopInteraction(currentIO);
        }

        cc.Move(dir * Time.deltaTime / 2);

    }

    public void Exit()
    {

    }

    void ForwardProximity(float inputH, float inputV)
    {
        var currentIO = hero.GetIO();

        Vector3 forwardProximity = new Vector3(Mathf.RoundToInt(hero.transform.forward.x), Mathf.RoundToInt(hero.transform.forward.y), Mathf.RoundToInt(hero.transform.forward.z));
        if (forwardProximity == Vector3.forward || forwardProximity == -Vector3.forward)
        {
            dir = new Vector3(Vector3.forward.x * inputV * speed, dir.y, Vector3.forward.z * inputV * speed); ;
            anim.SetFloat("SpeedX", Mathf.Abs(-inputH * speed) + Mathf.Abs(inputV * speed));
            if (inputH != 0)
            {
                currentIO.GetComponent<Box>().Move(Vector3.zero);
                hero.SetMovementMode(MovementMode.Normal);
                hero.StopInteraction(currentIO);
            }

        }
        if (forwardProximity == Vector3.right || forwardProximity == -Vector3.right)
        {
            dir = Vector3.right * -inputH * speed;
            anim.SetFloat("SpeedX", Mathf.Abs(-inputH * speed) + Mathf.Abs(inputV * speed));
            if (inputV != 0)
            {
                currentIO.GetComponent<Box>().Move(Vector3.zero);
                hero.SetMovementMode(MovementMode.Normal);
                hero.StopInteraction(currentIO);
            }
        }
    }

    public IGravityMode GetGravityMode()
    {
        return gravityMode;
    }
}
