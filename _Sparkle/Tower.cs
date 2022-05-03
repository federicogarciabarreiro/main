using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Tower : MonoBehaviour
{

    public enum ActionState
    {
        Patrol,
        Shoot,
        Reload,
    }

    GenericFSM _sm;
    Hero hero;
    float timeToPatrol;
    float timeToTurn;
    float rotationSpeed = 1.25f;
    Vector3 dir;
    bool heroDetected;
    public Transform head;
    float timerToShoot;
    float coolDownToShoot = 0.075f;
    int bulletsLeft = 10;
    int maxBullets = 10;
    float reloadTimer;

    public GameObject bulletPrefab;
    public Transform SP;

    void Start()
    {
        dir = -head.forward;
        hero = GameObject.Find("Hero").GetComponent<Hero>();
        _sm = new GenericFSM();
        _sm.AddState(ActionState.Patrol, Patrol, PatrolEnter);
        _sm.AddState(ActionState.Shoot, Shoot, ShootEnter);
        _sm.AddState(ActionState.Reload, Reload);

        _sm.AddTransition(ActionState.Patrol, () => { return heroDetected; }, ActionState.Shoot);
        _sm.AddTransition(ActionState.Shoot, () => { return !heroDetected; }, ActionState.Patrol);
        _sm.AddTransition(ActionState.Shoot, () => { return bulletsLeft == 0; }, ActionState.Reload);
        _sm.AddTransition(ActionState.Reload, () => { return bulletsLeft == maxBullets; }, ActionState.Shoot);

        _sm.SetState(0);
    }

    void Update()
    {
        _sm.Update();
    }


    void Reload()
    {
        dir = hero.transform.position - head.transform.position;
        head.forward = Vector3.Slerp(head.forward, dir, rotationSpeed * 3f * Time.deltaTime);
        reloadTimer += Time.deltaTime;
        if (reloadTimer > 0.2f)
        {
            bulletsLeft++;
            reloadTimer = 0;
        }   
    }
    void PatrolEnter()
    {

    }
    void Patrol()
    {
        timeToPatrol += Time.deltaTime;
        if (timeToPatrol > 2f)
        {
            dir = head.forward + head.right;
            timeToPatrol = 0;
        }
        head.forward = Vector3.Slerp(head.forward, dir, rotationSpeed * Time.deltaTime);
    }

    void ShootEnter()
    {

    }

    void Shoot()
    {
        dir = hero.transform.position - head.transform.position;
        head.forward = Vector3.Slerp(head.forward, dir, rotationSpeed * 5f * Time.deltaTime);
        timerToShoot += Time.deltaTime;
        if (timerToShoot > coolDownToShoot)
        {
            bulletsLeft--;
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.transform.forward = hero.transform.position - SP.transform.position;
            bullet.transform.position = SP.position;
            timerToShoot = 0;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Hero>())
            heroDetected = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Hero>())
            heroDetected = false;
    }
}
