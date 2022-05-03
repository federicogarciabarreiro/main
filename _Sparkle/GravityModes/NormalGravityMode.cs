using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalGravityMode : IGravityMode
{
    CharacterController cc;
    Hero hero;
    public Vector3 dir;
    Animator anim;
    float gravityScale = 0.9f;
    float speed = 3;

    public NormalGravityMode(Hero Hero, CharacterController CC, Animator Anim)
    {
        hero = Hero;
        anim = Anim;
        cc = CC;
    }

    public void Action()
    {
        anim.SetBool("onGround", hero.GetIfGrounded());
        if (!hero.GetFreezedState() && !hero.GetFreezedPositionState())
            anim.SetFloat("SpeedX", Mathf.Abs(-Input.GetAxis("Vertical") * speed) + Mathf.Abs(Input.GetAxis("Horizontal") * speed));
        else
            anim.SetFloat("SpeedX", 0f);
    }

    public float GetYDirection()
    {
        if (!hero.GetIfGrounded())
            return Physics.gravity.y * gravityScale * Time.deltaTime;
        else
            return 0;
    }

}
