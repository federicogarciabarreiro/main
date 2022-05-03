using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum actionType
{
    awake,
    execute,
    sleep
}
public class GenericFSM
{
    public int _currentState = -1;
    public List<int> _states = new List<int>();
    public Dictionary<int, List<Action>> _awakes = new Dictionary<int, List<Action>>();
    public Dictionary<int, List<Action>> _executes = new Dictionary<int, List<Action>>();
    public Dictionary<int, List<Action>> _sleeps = new Dictionary<int, List<Action>>();
    public List<Transition> _transitions = new List<Transition>();

    public void Update()
    {

        int changeState = -1;
        if (_transitions.Where(x => x.state == _currentState || x.state == -2).Count() > 0)
        {
            var posTrans = _transitions.Where(x => x.state == _currentState || x.state == -2).ToList();
            foreach (var tran in posTrans)
            {
                changeState = tran.nextState;
                foreach (var cond in tran.condition)
                    if (!cond())
                        changeState = -1;
                if (changeState != -1)
                {
                    SetState(changeState);
                    return;
                }
            }

        }
        if (_currentState != -1 && _executes.ContainsKey(_currentState) && changeState == -1)
            foreach (var act in _executes[_currentState])
                act();
    }

    public void AddTransition(object st, List<Func<bool>> cond, object next)
    {
        _transitions.Add(new Transition() { state = (int)st, condition = cond, nextState = (int)next });
    }
    public void AddTransition(object st, Func<bool> cond, object next)
    {
        _transitions.Add(new Transition() { state = (int)st, condition = new List<Func<bool>>().Compose(cond), nextState = (int)next });
    }

    public void AddAction(object st, actionType type, Action action)
    {
        int state = (int)st;
        switch (type)
        {
            case actionType.awake:
                _awakes[state].Add(action);
                break;
            case actionType.execute:
                _executes[state].Add(action);
                break;
            case actionType.sleep:
                _sleeps[state].Add(action);
                break;
        }
    }

    public void AddState(object st, Action baseExecute = null, Action baseAwake = null, Action baseSleep = null)
    {
        int state = (int)st;
        int ss = SearchState(state);
        if (ss == -1)
        {
            _states.Add(state);
            _awakes[state] = new List<Action>();
            _executes[state] = new List<Action>();
            _sleeps[state] = new List<Action>();
            if (baseExecute != null)
                AddAction(state, actionType.execute, baseExecute);
            if (baseAwake != null)
                AddAction(state, actionType.awake, baseAwake);
            if (baseSleep != null)
                AddAction(state, actionType.sleep, baseSleep);
        }
    }

    public void SetState(object st)
    {
        int state = (int)st;
        if (IsActualState(state))
            return;

        for (int i = 0; i < _states.Count; i++)
        {
            if (_states[i] == state)
            {
                if (_currentState != -1 && _sleeps.ContainsKey(_currentState))
                    foreach (var act in _sleeps[_currentState])
                        act();
                _currentState = _states[i];
                if (_awakes.ContainsKey(_currentState))
                    foreach (var act in _awakes[_currentState])
                        act();
            }
        }
    }

    public void EndCurrentState()
    {
        if (_currentState != -1 && _sleeps.ContainsKey(_currentState))
            foreach (var act in _sleeps[_currentState])
                act();
        _currentState = -1;
    }

    public bool IsActualState(object state)
    {
        int st = (int)state;
        return (_currentState != -1) ? (_currentState == st) : false;
    }

    public int SearchState(object st)
    {
        int s = (int)st;
        int ammountOfStates = _states.Count;
        for (int i = 0; i < ammountOfStates; i++)
            if (_states[i] == s)
                return i;
        return -1;
    }

    public int GetCurrentState()
    {
        return _currentState;
    }
}
public struct Transition
{
    public int state;
    public List<Func<bool>> condition;
    public int nextState;
}

public static class Extensions
{
    public static List<T> Compose<T>(this List<T> baseList, T value)
    {
        baseList.Add(value);
        return baseList;
    }
}