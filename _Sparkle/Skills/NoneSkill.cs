using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoneSkill : ISkill {
    public KeyCode key { get; set; }

    public void Action()
    {
        
    }

    public void Enter()
    {
        
    }

    public void Exit()
    {
        
    }

    public IInteractable GetInteractiveObject()
    {
        throw new NotImplementedException();
    }

    public void SetInteractiveObject(IInteractable interactableObj)
    {

    }

    public void SetKey(KeyCode Key)
    {
        key = Key;
    }
}
