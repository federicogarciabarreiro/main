using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideMovementMode : IMovementMode
{
    public IGravityMode gravityMode
    {
        get
        {
            throw new NotImplementedException();
        }

        set
        {
            throw new NotImplementedException();
        }
    }

    public void Action(float inputH, float inputV)
    {
        throw new NotImplementedException();
    }

    public void Enter()
    {
        throw new NotImplementedException();
    }

    public void Exit()
    {
        throw new NotImplementedException();
    }

    public IGravityMode GetGravityMode()
    {
        throw new NotImplementedException();
    }
}
