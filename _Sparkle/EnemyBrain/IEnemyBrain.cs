using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyBrain {

    void Enter();
    void Action();
    void Exit();
    GenericFSM GetSM();
}
