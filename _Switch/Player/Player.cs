using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    Rigidbody _rb;
    public IStrategy iStrategy;

    [Range(8,12)]
    public int speed = 12;

    [Range(1, 3)]
    public int rotationSpeed = 2;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        iStrategy = new Idle();
    }

    void Update()
    {
        iStrategy.Action(this, _rb);
    }

    private void OnTriggerEnter(Collider other)
    {
        _rb.velocity = Vector3.zero;
        iStrategy = new Idle();
        transform.position = new Vector3(0,-5,transform.position.z - 5);
    }
}
