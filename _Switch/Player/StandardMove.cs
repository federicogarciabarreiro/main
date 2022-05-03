using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardMove : IStrategy
{
    public void Action(Player _player, Rigidbody _rb)
    {
        Quaternion _quaternion = Quaternion.AngleAxis(_player.rotationSpeed, Vector3.forward);
        Vector3 _position = _quaternion * (_rb.transform.position);
        _rb.MovePosition(_position);
        _rb.MoveRotation(_rb.transform.rotation * _quaternion);
        _rb.velocity = new Vector3(0, 0, Mathf.Abs(_player.speed));
        if (Input.anyKeyDown) _player.rotationSpeed = -_player.rotationSpeed;
    }
}
