using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    void Update()
    {
        Transform _target = Config.instance.playerTransform;
        Vector3 _targetPos = new Vector3(transform.position.x, transform.position.y, _target.transform.position.z - 10);
        transform.position = Vector3.Lerp(transform.position, _targetPos, Mathf.Abs(_target.GetComponent<Player>().speed));
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.identity, Mathf.Abs(_target.GetComponent<Player>().speed));
        transform.LookAt(new Vector3(_target.transform.position.x / 2, _target.transform.position.y / 2, _target.transform.position.z));
    }
}
