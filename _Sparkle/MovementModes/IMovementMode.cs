using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMovementMode {

    IGravityMode gravityMode { get; set; }
    IGravityMode GetGravityMode();
    void Enter();
    void Action(float inputH, float inputV);
    void Exit();

}
