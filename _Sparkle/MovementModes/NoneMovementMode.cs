using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoneMovementMode : IMovementMode
{
    Animator anim;
    public NoneMovementMode(Animator Anim, IGravityMode GravMode)
    {
        gravityMode = GravMode;
        anim = Anim;
    }

    public IGravityMode gravityMode { get; set; }

    public void Action(float inputH, float inputV)
    {
        
    }

    public void Enter()
    {
        anim.SetFloat("SpeedX", 0);
        anim.SetBool("onGround", true);
    }

    public void Exit()
    {
        
    }

    public IGravityMode GetGravityMode()
    {
        return gravityMode;
    }
}
