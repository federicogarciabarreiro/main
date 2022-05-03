using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kill3EnemiesArea01Goal : IGoal
{
    int enemiesKilled;
    public List<IGoalListener> listOfListeners { get; set; }

    public Kill3EnemiesArea01Goal()
    {
        listOfListeners = new List<IGoalListener>();
    }


    public bool IsCompleted()
    {
        return enemiesKilled == 3;
    }

    public void Progress()
    {
        enemiesKilled++;
        if (enemiesKilled == 3)
            Notify();
    }

    public void Regress()
    {
        
    }

    public void AddListener(IGoalListener listener)
    {
        listOfListeners.Add(listener);
    }

    public void RemoveListener(IGoalListener listener)
    {
        listOfListeners.Remove(listener);
    }

    public void Notify()
    {
        foreach (var item in listOfListeners)
            item.Notify();
    }


}
