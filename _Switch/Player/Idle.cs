using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : IStrategy
{
    public void Action(Player _player, Rigidbody _rb)
    {
        if (Input.anyKeyDown) _player.iStrategy = new StandardMove();
    }
}
