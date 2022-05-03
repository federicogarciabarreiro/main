using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.FinalIK;

public class LiftSkill : ISkill {

    public KeyCode key { get; set; }
    public ILiftable obj;
    InteractionObject telekinesisHandInteractive;
    Transform handPowerEffect;
    Hero hero;
    float timer;
    Transform target;
    Animator anim;
    int state;
    Vector3 forwardDir;
    Transform proyector;

    public LiftSkill(Animator Anim)
    {
        hero = Config.me.hero.GetComponent<Hero>();
        anim = Anim;
        handPowerEffect = Config.me.handPowerEffect;
        proyector = Config.me.proyector;
    }
    public void Enter()
    {

    }
    public void Action()
    {
        if (state == 0 && obj != null)
        {
            GameManager.me.em.SetActionElapsed(ActionElapsed.FirstTimeLiftedObject);
            GameManager.me.im.RemoveInteractionObject(obj as IInteractable, Skill.Lift);
            telekinesisHandInteractive = hero.transform.Find("TelekinesisHandInteractive").GetComponent<InteractionObject>();
            handPowerEffect.gameObject.SetActive(true);
            hero.GetIS().StartInteraction(FullBodyBipedEffector.LeftHand, telekinesisHandInteractive, true);

            state++;
        }
        if (state == 1)
        {
            timer += Time.deltaTime;
            if (timer > 0.1f)
            {
                timer = 0;
                state++;
            }
            forwardDir = (obj.GetTransform().position - hero.transform.position).normalized;
            hero.transform.forward = Vector3.Slerp(hero.transform.forward, new Vector3(forwardDir.x, 0, forwardDir.z), 0.3f);
        }
        if (state == 2)
        {
            obj.isLifted = true;
            proyector.gameObject.SetActive(true);
            state++;

        }
        if (state == 3)
        {
            timer += Time.deltaTime;

            //RAY -------------------------------------------------------
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 1000f, Config.me.objectsAndEnemies))
            {
                target = hit.transform;
                if (target != null)
                {
                    RaycastHit hit2;
                    if (Physics.Raycast(target.transform.position, (hero.transform.position - target.transform.position).normalized, out hit2, 10f, Config.me.heroAndWallsMask))
                    {
                        if (target != obj.GetTransform() && hit2.transform == hero.transform)
                            CursorManager.me.SetCursorColor(true, CursorTextureType.PRIMARY);
                        else
                            target = null;
                    }
                }
            }
            else
            {
                CursorManager.me.SetCursorColor(false, CursorTextureType.PRIMARY);
                target = null;
            }

            //------------------------------------------------------------

            proyector.transform.position = new Vector3(obj.GetTransform().position.x, hero.transform.position.y + 1f, obj.GetTransform().position.z);

            if ((Input.GetKeyDown(key) && timer > 0.1f && hero.GetIS().GetClosestTriggerIndex() == -1)
                || obj == null
                || hero.GetFreezedState())
            {
                proyector.gameObject.SetActive(false);
                hero.GetIS().StopInteraction(FullBodyBipedEffector.LeftHand);
                if (target != null)
                {
                    if (target && target != obj.GetTransform())
                    {
                        anim.SetTrigger("AttackTrigger");
                        GameManager.me.em.SetActionElapsed(ActionElapsed.FirstTimeThrownedObject);
                        forwardDir = (hit.transform.position - hero.transform.position).normalized;
                        timer = 0;
                        state++;
                    }

                }
                else
                {
                    Reset();
                }

            }

        }

        if (state == 4)
        {
            timer += Time.deltaTime;
            hero.transform.forward = Vector3.Slerp(hero.transform.forward, new Vector3(forwardDir.x, 0, forwardDir.z), 0.35f);

            if (timer > 0.4f)
            {
                hero.SetMovementMode(MovementMode.Normal);
                state++;
            }
        }

        if (state == 5)
        {
            if (target != null)
            {
                if (target != obj.GetTransform())
                    obj.LiftEnd(target.GetComponentInChildren<Collider>().bounds.center);
            }
            Reset();
        }
    }

    public void Exit()
    {
        Reset();
    }
    public void Reset()
    {
        if(obj != null)
        {
            obj.isLifted = false;
            GameManager.me.im.AddInteractionObject(obj as IInteractable, Skill.Lift);
            handPowerEffect.gameObject.SetActive(false);
            hero.GetIS().StopInteraction(FullBodyBipedEffector.LeftHand);
            timer = 0;
            state = 0;
            proyector.gameObject.SetActive(false);
            obj = null;
        }


    }

    public void SetInteractiveObject(IInteractable interactableObj)
    {
        obj = interactableObj as ILiftable;
        if(obj != null)
        if(obj.GetTransform().GetComponent<DoorOpener>())
        {
            obj = null;
            Debug.Log("boton quiso entrar");
        }
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
