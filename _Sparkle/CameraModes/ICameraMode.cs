using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICameraMode {
    CameraZone currentZone { get; set; }

    void SetCurrentZone(CameraZone zone);

    void Enter();
    void Action();
    void SetTarget(Transform target);
}
