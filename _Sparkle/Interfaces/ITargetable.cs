using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITargetable {

    void Selected();
    void Unselected();
    void AddToTargetsList();
    void RemoveFromTargetsList();
    Vector3 GetPosition();
}
