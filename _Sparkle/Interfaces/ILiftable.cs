using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILiftable {

    Collider col { get; set; }
    bool isLifted { get; set; }
    void Lift();
    void LiftEnd(Vector3 destination);
    void TargetedState();
    void UnselectedState();

    Vector3 GetPosition();
    Transform GetTransform();

    Rigidbody GetRigidbody();

    void SetContinuousRB();
}
