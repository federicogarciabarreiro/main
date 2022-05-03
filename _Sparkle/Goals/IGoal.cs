using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGoal {

    bool IsCompleted();

    void Progress();
    void Regress();

    List<IGoalListener> listOfListeners { get; set; }
    void AddListener(IGoalListener listener);
    void RemoveListener(IGoalListener listener);
    void Notify();
}
