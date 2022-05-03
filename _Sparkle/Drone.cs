using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Drone : MonoBehaviour {

    public enum ActionState
    {
        Patrol,
        Turn,
        Chase,
    }

    Rigidbody rb;
    GenericFSM _sm;
    Hero hero;
    float timeToPatrol;
    float timeToTurn;
    public float speed;
    Vector3 oppositeVector;
    float rotationSpeed = 1.2f;
    Vector3 dir;
    bool heroDetected;
    public Light myDroneLight;
    public Color patrolLight;
    public Color chaseLight;
    public Transform waypoints;
    int currentWaypoint;

    void Start () {
        dir = transform.forward;
        rb = GetComponent<Rigidbody>();
        hero = GameObject.Find("Hero").GetComponent<Hero>();
        _sm = new GenericFSM();
        _sm.AddState(ActionState.Patrol, Patrol, PatrolEnter);
        _sm.AddState(ActionState.Chase, Chase, ChaseEnter);

        _sm.AddTransition(ActionState.Patrol, () => { return heroDetected; }, ActionState.Chase);
        _sm.AddTransition(ActionState.Chase, () => { return !heroDetected; }, ActionState.Patrol);

        _sm.SetState(0);
    }
	
	void Update () {
        _sm.Update();
	}


    void PatrolEnter()
    {
        myDroneLight.color = patrolLight;
    }
    void Patrol()
    {
        dir = (waypoints.GetChild(currentWaypoint).position - transform.position).normalized;
        transform.forward = Vector3.Slerp(transform.forward, dir, rotationSpeed * Time.deltaTime);
        rb.velocity = transform.forward * speed;
        if (Vector3.Distance(transform.position, waypoints.GetChild(currentWaypoint).position) < 1f)
            currentWaypoint++;
        if (currentWaypoint == waypoints.childCount)
            currentWaypoint = 0;
        //timeToPatrol += Time.deltaTime;
        //if (timeToPatrol > 7.5f)
        //{
        //    dir = -transform.forward;
        //    timeToPatrol = 0;
        //}
        //transform.forward = Vector3.Slerp(transform.forward, dir, rotationSpeed * Time.deltaTime);
        //rb.velocity = transform.forward * speed;
    }

    void ChaseEnter()
    {
        myDroneLight.color = chaseLight;
    }

    void Chase()
    {
        dir = new Vector3(hero.transform.position.x,transform.position.y,hero.transform.position.z)- transform.position;
        transform.forward = Vector3.Slerp(transform.forward, dir, rotationSpeed * Time.deltaTime);
        rb.velocity = transform.forward * speed*2;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Hero>())
            heroDetected = true;
    }
}
