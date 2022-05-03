using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable {

    Vector3 GetPosition();
    bool CanInteract(Hero hero);
    void SwitchOutline(bool state, CursorTextureType type);
    void Interact(Skill interaction);
    void AddToList();
    void RemoveFromList();

}
