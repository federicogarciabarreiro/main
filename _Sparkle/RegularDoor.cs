using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegularDoor : Door, IDoor, IGoalListener
{

    AudioSource _as;
    bool canPlaySound = true;
    public Transform emissive;

    void Start()
    {
        speed = 2.25f;
        startPos = transform.localPosition;
        finalPos = transform.localPosition + Vector3.up * transform.localScale.y;
        targetPos = finalPos;
        AddToListOfListeners();

        _as = GetComponent<AudioSource>();
    }

    public void Open()
    {
        if(GoalManager.me.listOfGoals[(int)Goal.Kill3EnemiesArea01].IsCompleted())
        {
            
            if (canPlaySound)
            {
                _as.Play();
                canPlaySound = false;
            }
            StartCoroutine(Transition());
        }

    }

    public IEnumerator Transition()
    {
        while (transform.localPosition != finalPos)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPos, speed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }

    public void AddToListOfListeners()
    {
        GoalManager.me.SubscribeGoalListener(Goal.Kill3EnemiesArea01, this);
    }

    public void RemoveFromListOfListeners()
    {
        GoalManager.me.UnsubscribeGoalListener(Goal.Kill3EnemiesArea01, this);
    }

    public void Notify()
    {
        if(emissive)
        {
            emissive.GetComponent<Renderer>().material.color = Color.green;
            Material mymat = emissive.GetComponent<Renderer>().material;
            mymat.SetColor("_EmissionColor", Color.green);
        }

    }

    public void OpenOnly()
    {
        
    }

    public void OpenWithCondition(Func<bool> cond)
    {

    }

    public void CloseOnly()
    {
        
    }

    public void ReceiveInfo(int info)
    {

    }
}
