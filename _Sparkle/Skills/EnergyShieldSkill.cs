using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyShieldSkill : ISkill {

    public KeyCode key { get; set; }
    public ILiftable obj;
    Transform lastUsedObject;
    Hero hero;
    Animator anim;
    int state;
    EnergyShield shield;
    float timer;
    ParticleSystem shockWaveEffect;
    Vector3 forwardDir;
    bool targetOnSelf;


    public EnergyShieldSkill(Animator Anim, ParticleSystem ShockWaveEffect)
    {
        hero = Config.me.hero.GetComponent<Hero>();
        anim = Anim;
        shield = Config.me.energyShield.GetComponent<EnergyShield>();
        shockWaveEffect = ShockWaveEffect;
    }

    public void Enter()
    {

    }

    public void Action()
    {
        if(state == 0 && GameManager.me.GetLevel() >= 2)
        {
            if (Input.GetKeyDown(key))
            {
                state++;
            }
        }

        if(state == 1)
        {
            anim.SetTrigger("BreakTrigger");
            timer = 0;
            state++;
        }

        if(state == 2)
        {
            timer += Time.deltaTime;
            if (timer > 0.1f)
                state++;
        }

        if(state == 3)
        {
            timer = 0;
            if (obj == null)
            {
                GameManager.me.em.SetActionElapsed(ActionElapsed.FirstTimeShieldOnSelf);
                shield.SetTarget(hero.transform);
                hero.SetShieldState(true);
                targetOnSelf = true;
            }
            else
            {
                GameManager.me.em.SetActionElapsed(ActionElapsed.FirstTimeShieldOnObject);
                //GameManager.me.im.RemoveInteractionObject(obj as IInteractable, Skill.EnergyShield);
                hero.SetShieldState(false);
                shield.SetTarget(obj.GetTransform());
                forwardDir = (obj.GetTransform().position - hero.transform.position).normalized;
                hero.transform.forward = new Vector3(forwardDir.x, 0, forwardDir.z);
                targetOnSelf = false;
                lastUsedObject = obj.GetTransform();
            }

            obj = null;
            shield.Activate();
            state++;
        }

        if (state == 4)
        {
            if (Input.GetKeyDown(key) && timer > 0.1f)
            {
                if (!targetOnSelf)
                {
                    shield.SwitchLayer(lastUsedObject);
                }
                else
                {
                    shield.SwitchLayer(hero.transform);
                }
                state = 1;

            }

            timer += Time.deltaTime;
            if(timer > 8f)
            {
                Reset();
            }

        }
    }

    public void Reset()
    {
        //if (obj != null && !GameManager.me.im.listOfInteractionObjects[(int)Skill.EnergyShield].Contains(obj as IInteractable))
        //    GameManager.me.im.AddInteractionObject(obj as IInteractable, Skill.EnergyShield);
        state = 0;
        timer = 0;
        obj = null;
    }

    public void Exit()
    {
        Reset();
    }

    public void SetInteractiveObject(IInteractable interactableObj)
    {
        obj = interactableObj as ILiftable;
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
