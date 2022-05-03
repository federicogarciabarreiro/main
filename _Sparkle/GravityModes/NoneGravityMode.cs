using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoneGravityMode : IGravityMode {
    public void Action()
    {
        
    }

    public float GetYDirection()
    {
        return 0;
    }
}
