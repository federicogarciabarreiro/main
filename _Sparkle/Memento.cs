using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Memento
{

    public float framesPerSecond = 10;
    public List<IState> states = new List<IState>();
}
