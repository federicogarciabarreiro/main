using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICameraLayer {
    void Enter(Camera cam);
    void Exit(Camera cam);
}
