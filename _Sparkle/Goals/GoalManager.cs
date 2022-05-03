using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Goal
{
    None,
    Kill3EnemiesArea01
}

public class GoalManager : MonoBehaviour {

    public static GoalManager me;
    public List<IGoal> listOfGoals = new List<IGoal>();

    private void Awake()
    {
        me = this;
        listOfGoals.Add(new NoneGoal());
        listOfGoals.Add(new Kill3EnemiesArea01Goal());
    }

    public void SubscribeGoalListener(Goal goal, IGoalListener listener)
    {
        listOfGoals[(int)goal].AddListener(listener);
    }

    public void UnsubscribeGoalListener(Goal goal, IGoalListener listener)
    {
        listOfGoals[(int)goal].RemoveListener(listener);
    }
}
