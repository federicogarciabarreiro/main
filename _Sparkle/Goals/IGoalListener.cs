using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGoalListener {

    void AddToListOfListeners();
    void RemoveFromListOfListeners();
    void Notify();
}
