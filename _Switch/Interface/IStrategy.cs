using UnityEngine;
using System.Collections;

public interface IStrategy
{
    void Action(Player _player, Rigidbody _rb);
}
