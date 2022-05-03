using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISkill {

    KeyCode key { get; set; }
    void SetKey(KeyCode Key);
    void Enter();
    void Action();
    void Exit();
    void SetInteractiveObject(IInteractable interactableObj);
    IInteractable GetInteractiveObject();
}
