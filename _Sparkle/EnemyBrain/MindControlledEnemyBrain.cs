using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.FinalIK;

public class MindControlledEnemyBrain : IEnemyBrain {

    GenericFSM _sm = new GenericFSM();
    Enemy enemy;
    CharacterController cc;
    float gravityScale = 0.9f;
    float speed = 4.5f;
    Vector3 dir;
    Animator anim;
    float turningSpeed = 0.05f;
    AimIK aimIK;

    public MindControlledEnemyBrain(Enemy Enemy)
    {
        enemy = Enemy;
        cc = enemy.GetComponent<CharacterController>();
        anim = enemy.transform.Find("Soldier").GetComponent<Animator>();
        aimIK = enemy.transform.Find("Soldier").GetComponent<AimIK>();
    }


    public void Enter()
    {
        aimIK.enabled = false;
        anim.SetTrigger("idleTrigger");
        anim.SetBool("isShooting", false);
    }

    public void Action()
    {
        Movement();
        Gravity();
    }

    void Gravity()
    {
        //anim.SetBool("onGround", hero.GetIfGrounded());
        anim.SetFloat("SpeedX", Mathf.Abs(-Input.GetAxis("Vertical") * speed) + Mathf.Abs(Input.GetAxis("Horizontal") * speed));
    }

    void Movement()
    {
        dir = new Vector3(-Input.GetAxis("Vertical") * speed, dir.y, Input.GetAxis("Horizontal") * speed);
        dir.y += GetYDirection();

        enemy.transform.forward = Vector3.Slerp(enemy.transform.forward, new Vector3(dir.x, 0, dir.z), turningSpeed);

        cc.Move(dir * Time.deltaTime);
    }

    public float GetYDirection()
    {
        if (!enemy.GetIfGrounded())
            return Physics.gravity.y * gravityScale * Time.deltaTime;
        else
            return 0;
    }

    public void Exit()
    {
        anim.SetFloat("SpeedX", 0f);
        aimIK.enabled = true;
    }

    public GenericFSM GetSM()
    {
        return _sm;
    }
}
