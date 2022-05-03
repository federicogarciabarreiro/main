using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoneGoal : IGoal
{
    public List<IGoalListener> listOfListeners { get; set; }

    public void AddListener(IGoalListener listener)
    {
        
    }

    public bool IsCompleted()
    {
        return true;
    }

    public void Notify()
    {
       
    }

    public void Progress()
    {
        
    }

    public void Regress()
    {
        
    }

    public void RemoveListener(IGoalListener listener)
    {
        
    }
}
