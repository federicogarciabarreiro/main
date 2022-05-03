using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.FinalIK;

public class MindControlSkill : ISkill
{

    public KeyCode key { get; set; }
    public ITimeSlowable obj;
    Hero hero;
    Animator anim;
    int state;
    float timer;
    InteractionObject telekinesisHandInteractive;
    Transform handPowerEffect;
    bool isReseting;


    public MindControlSkill(Animator Anim)
    {
        hero = Config.me.hero.GetComponent<Hero>();
        anim = Anim;
        handPowerEffect = Config.me.handPowerEffect3;
    }

    public void Enter()
    {

    }

    public void Action()
    {
        if (state == 0 && obj != null && Input.GetKeyDown(key) && GameManager.me.GetLevel() >= 3)
        {
             state++;
        }

        if (state == 1)
        {
            anim.SetTrigger("BreakTrigger");
            Vector3 dir = (obj.GetTransform().position - hero.transform.position).normalized;
            dir.y = hero.transform.forward.y;
            hero.transform.forward = dir;
            timer = 0;
            state++;
        }

        if (state == 2)
        {
            timer += Time.deltaTime;
            if (timer > 0.4f)
                state++;
        }

        if (state == 3)
        {
            timer = 0;
            Debug.Log("ENEMY CONTROLLED");
            Camera.main.GetComponent<MainCamera>().SetTarget(obj.GetTransform());
            hero.Freeze(true);
            hero.GetComponent<CharacterController>().enabled = false;
            hero.gameObject.AddComponent<Rigidbody>();
            hero.GetComponent<Rigidbody>().isKinematic = true;
            hero.gameObject.AddComponent<BoxCollider>();
            (obj as Enemy).SetBrain(Enemy.EnemyBrain.MindControlled);
            GameManager.me.em.SetActionElapsed(ActionElapsed.FirstTimeMindControl);

            telekinesisHandInteractive = hero.transform.Find("TelekinesisHandInteractive").GetComponent<InteractionObject>();
            handPowerEffect.gameObject.SetActive(true);
            hero.GetIS().StartInteraction(FullBodyBipedEffector.LeftHand, telekinesisHandInteractive, true);

            state++;
        }

        if(state == 4)
        {
            timer += Time.deltaTime;
            if(Input.GetKeyDown(key) || obj == null || hero.isDead())
                state++;
        }

        if (state == 5)
        {

            Reset();
        }

    }

    public void Reset()
    {
        if(!isReseting)
        {
            isReseting = true;
            timer = 0;
            state = 0;
            if (!hero.isDead())
                hero.Freeze(false);
            Camera.main.GetComponent<MainCamera>().SetTarget(hero.transform);
            hero.GetComponent<CharacterController>().enabled = true;
            if (hero.GetComponent<Rigidbody>())
                MonoBehaviour.Destroy(hero.gameObject.GetComponent<Rigidbody>());
            if (hero.GetComponent<BoxCollider>())
                MonoBehaviour.Destroy(hero.gameObject.GetComponent<BoxCollider>());
            if (obj != null)
            {
                (obj as Enemy).SetBrain(Enemy.EnemyBrain.Self);
                obj = null;
            }

            handPowerEffect.gameObject.SetActive(false);
            hero.GetIS().StopInteraction(FullBodyBipedEffector.LeftHand);
            isReseting = false;
        }




    }

    public void Exit()
    {

    }

    public void SetInteractiveObject(IInteractable interactableObj)
    {
        obj = interactableObj as ITimeSlowable;
        if (GameManager.me.GetLevel() >= 3)
            state = 1;
    }


    public IInteractable GetInteractiveObject()
    {
        return obj as IInteractable;
    }

    public void SetKey(KeyCode Key)
    {
        key = Key;
    }

}