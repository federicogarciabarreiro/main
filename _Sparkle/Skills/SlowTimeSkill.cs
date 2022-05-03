using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.FinalIK;

public class SlowTimeSkill : ISkill
{
    public KeyCode key { get; set; }
    public ITimeSlowable obj;
    InteractionObject telekinesisHandInteractive;
    Transform handPowerEffect;
    Hero hero;
    float timer;
    int state;
    Vector3 forwardDir;

    public SlowTimeSkill()
    {
        hero = Config.me.hero.GetComponent<Hero>();
    }
    public void Enter()
    {
        handPowerEffect = Config.me.handPowerEffect2;

    }
    public void Action()
    {
        if (state == 0)
        {
            if(obj != null)
            {
                GameManager.me.em.SetActionElapsed(ActionElapsed.FirstTimeSlowDownObject);
                if(obj.GetTransform().GetComponent<Enemy>())
                {
                    GameManager.me.em.SetActionElapsed(ActionElapsed.FirstTimeSlowDownEnemy);
                }
                obj.SlowDown(3);
                GameManager.me.im.RemoveInteractionObject(obj as IInteractable, Skill.SlowTime);
                telekinesisHandInteractive = hero.transform.Find("TelekinesisHandInteractive_Right").GetComponent<InteractionObject>();
                handPowerEffect.gameObject.SetActive(true);
                hero.GetIS().StartInteraction(FullBodyBipedEffector.RightHand, telekinesisHandInteractive, true);
                state++;
            }
        }

        if(state == 1)
        {
            timer += Time.deltaTime;
            if(timer > 0.1f)
            {
                timer = 0;
                state++;
            }
            forwardDir = (obj.GetTransform().GetComponentInChildren<Collider>().bounds.center - hero.transform.position).normalized;
            hero.transform.forward = Vector3.Slerp(hero.transform.forward, new Vector3(forwardDir.x, 0, forwardDir.z), 0.3f);
        }
        if (state == 2)
        {
            timer += Time.deltaTime;

            if ((Input.GetKeyDown(key) && timer > 0.1f)
                || obj == null
                || Vector3.Distance(hero.transform.position, obj.GetTransform().position) > 15.5f
                || timer > 2.5f
                || hero.GetFreezedState())
            {
                Reset();
            }
        }
    }

    public void Reset()
    {
        if (obj != null)
        {
            obj.Reset();
            GameManager.me.im.AddInteractionObject(obj as IInteractable, Skill.SlowTime);
        }
        //if(handPowerEffect)
            handPowerEffect.gameObject.SetActive(false);
        hero.GetIS().StopInteraction(FullBodyBipedEffector.RightHand);
        timer = 0;
        state = 0;
        obj = null;
    }

    public void Exit()
    {
        Reset();

    }

    public void SetInteractiveObject(IInteractable interactableObj)
    {
        obj = interactableObj as ITimeSlowable;
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
