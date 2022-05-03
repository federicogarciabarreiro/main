using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.FinalIK;

public class SelfControlledEnemyBrain : IEnemyBrain {

    GenericFSM _sm;
    Enemy enemy;
    Animator anim;
    Transform aimIKTarget;
    Vector3 aimIKStartPos;
    public float turningSpeed = 0.1f;
    public float initialTurningSpeed;
    AudioSource _as;
    bool heroSpotted;
    Hero hero;
    Vector3 dir;
    CharacterController cc;
    float gravityScale = 0.9f;

    Rigidbody rb;
    float turningTimer;
    AimIK aimIK;
    float timerToShoot;
    public float cooldownToShoot = 2f;
    public float initialCooldownToShoot;

    public SelfControlledEnemyBrain(Enemy Enemy)
    {
        enemy = Enemy;
        anim = enemy.transform.Find("Soldier").GetComponent<Animator>();
        aimIKTarget = enemy.transform.Find("AimIKTarget");
        aimIKStartPos = aimIKTarget.position;
        _as = enemy.GetComponent<AudioSource>();
        hero = Config.me.hero.GetComponent<Hero>();
        rb = enemy.GetComponent<Rigidbody>();
        aimIK = enemy.transform.Find("Soldier").GetComponent<AimIK>();
        initialCooldownToShoot = cooldownToShoot;
        initialTurningSpeed = turningSpeed;
        cc = enemy.GetComponent<CharacterController>();
    }



    public void Enter()
    {
        _sm = new GenericFSM();

        _sm.AddState(EnemyActionState.Idle, Idle);
        _sm.AddState(EnemyActionState.Shoot, Shoot, ShootEnter, ShootExit);
        _sm.AddState(EnemyActionState.Turn, Turn, TurnEnter, TurnExit);
        _sm.AddState(EnemyActionState.Death, Death);
        _sm.AddState(EnemyActionState.None, Idle);

        _sm.AddTransition(EnemyActionState.Idle, () => { return heroSpotted && !enemy.isPassiveEnemy; }, EnemyActionState.Shoot);
        _sm.AddTransition(EnemyActionState.Shoot, () => { return !heroSpotted; }, EnemyActionState.Idle);
        _sm.AddTransition(EnemyActionState.Shoot, () => { return Vector3.Dot((hero.transform.position - enemy.transform.position).normalized, enemy.transform.forward) < 0.2; }, EnemyActionState.Turn);
        _sm.AddTransition(EnemyActionState.Turn, () => { return Vector3.Dot((hero.transform.position - enemy.transform.position).normalized, enemy.transform.forward) > 0.2 && turningTimer > 0.67f; }, EnemyActionState.Shoot);

        _sm.SetState(EnemyActionState.Idle);
    }
    public void Action()
    {
        _sm.Update();
        Movement();
        SpotHero();
    }
    public void Exit()
    {
        enemy.lr.SetPosition(0, Vector3.zero);
        enemy.lr.SetPosition(1, Vector3.zero);
        _sm.SetState(EnemyActionState.Idle);
    }

    void Movement()
    {
        dir.y += GetYDirection();

        cc.Move(dir * Time.deltaTime);
    }

    public float GetYDirection()
    {
        if (!enemy.GetIfGrounded())
            return Physics.gravity.y * gravityScale * Time.deltaTime;
        else
            return 0;
    }

    void SpotHero()
    {
        RaycastHit hit;
        if (Physics.Raycast(enemy.transform.position, (hero.transform.position - enemy.transform.position).normalized, out hit, enemy.distanceToShoot, Config.me.heroAndWallsMask))
        {
            if (hit.transform == hero.transform)
                heroSpotted = true;
            else
                heroSpotted = false;
        }
        else
            heroSpotted = false;
    }

    void Idle()
    {
        anim.SetBool("isShooting", false);
        enemy.smState = EnemyActionState.Idle;
        aimIKTarget.position = Vector3.MoveTowards(aimIKTarget.position, aimIKStartPos, turningSpeed * Time.deltaTime * 90);

    }

    void Death()
    {
        aimIK.enabled = false;
        _as.clip = enemy.sounds[1];
        _as.Play();

        if ((hero.listOfSkills[(int)Skill.SlowTime] as SlowTimeSkill).obj == this as ITimeSlowable)
            (hero.listOfSkills[(int)Skill.SlowTime] as SlowTimeSkill).obj = null;

        GoalManager.me.listOfGoals[(int)enemy.goalToTrigger].Progress();
        //enemy.gameObject.layer = 0;

        enemy.soldier.gameObject.SetActive(false);
        enemy.GetComponent<BoxCollider>().enabled = false;
        enemy.GetComponent<CharacterController>().enabled = false;

        GameObject dyingEnemy = MonoBehaviour.Instantiate(Config.me.dyingEnemyPrefab);
        dyingEnemy.transform.position = enemy.transform.position + Vector3.up * 0.5f;
        dyingEnemy.transform.rotation = enemy.transform.rotation;


        enemy.shockWave.GetComponent<ParticleSystem>().Play();

        GameManager.me.StartCoroutine(MoveDown(5f));
        _sm.SetState(EnemyActionState.None);
    }

    IEnumerator MoveDown(float time )
    {
        yield return new WaitForSeconds(time);
        enemy.transform.position = enemy.transform.position - Vector3.up * 1000f;

    }

    void Turn()
    {
        turningTimer += Time.deltaTime;
        enemy.smState = EnemyActionState.Turn;
        Vector3 destination = (hero.transform.position - enemy.transform.position).normalized;
        destination.y = enemy.transform.forward.y;
        enemy.transform.forward = Vector3.Slerp(enemy.transform.forward, destination, turningSpeed * Time.deltaTime * 20);
        aimIKTarget.position = Vector3.MoveTowards(aimIKTarget.position, enemy.transform.position + enemy.transform.forward, turningSpeed * Time.deltaTime * 60);
    }

    float AngleDir(Vector3 fwd, Vector3 targetDir, Vector3 up)
    {
        Vector3 perp = Vector3.Cross(fwd, targetDir);
        float dir = Vector3.Dot(perp, up);

        if (dir > 0f)
        {
            return 1f;
        }
        else if (dir < 0f)
        {
            return -1f;
        }
        else
        {
            return 0f;
        }
    }

    void TurnEnter()
    {
        Vector3 heading = hero.transform.position - enemy.transform.position;
        if (AngleDir(enemy.transform.forward, heading, enemy.transform.up) == 1)
            anim.SetTrigger("turnLeft");
        else
            anim.SetTrigger("turnRight");
        turningTimer = 0;
        anim.SetBool("isTurning", true);

        aimIK.enabled = false;
    }

    void TurnExit()
    {
        aimIK.enabled = true;
    }

    void ShootEnter()
    {
        anim.SetBool("isTurning", false);
        enemy.smState = EnemyActionState.Shoot;
        if (enemy.canPlaySound)
        {
            _as.clip = enemy.sounds[0];
            _as.Play();
            enemy.canPlaySound = false;
        }
        anim.SetBool("isShooting", true);
        aimIK.enabled = true;
        aimIK.solver.IKPositionWeight = 0;

    }

    void ShootExit()
    {
        enemy.lr.SetPosition(0, Vector3.zero);
        enemy.lr.SetPosition(1, Vector3.zero);

        timerToShoot = 0f;
        aimIK.enabled = false;
    }
    void Shoot()
    {
        aimIK.solver.IKPositionWeight += Time.deltaTime * 2;
        aimIKTarget.position = Vector3.MoveTowards(aimIKTarget.position, hero.transform.position + enemy.transform.forward, turningSpeed * Time.deltaTime * 60);

        if (!hero.GetFreezedState())
            timerToShoot += Time.deltaTime;
        if (timerToShoot > (cooldownToShoot - (cooldownToShoot / 3)) && !hero.GetFreezedState())
        {
            enemy.lr.SetPosition(0, enemy.SP.position);
            enemy.lr.SetPosition(1, hero.transform.position + Vector3.up * 0.25f);
        }
        else
        {
            enemy.lr.SetPosition(0, Vector3.zero);
            enemy.lr.SetPosition(1, Vector3.zero);
        }
        if (timerToShoot > cooldownToShoot)
        {
            _as.clip = enemy.sounds[2];
            _as.Play();

            enemy.lr.SetPosition(0, Vector3.zero);
            enemy.lr.SetPosition(1, Vector3.zero);
            anim.SetTrigger("shoot");
            GameObject bullet = MonoBehaviour.Instantiate(enemy.bulletPrefab);
            bullet.transform.forward = hero.transform.position - enemy.SP.transform.position;
            bullet.transform.position = enemy.SP.position;
            timerToShoot = 0;
        }

    }

    public GenericFSM GetSM()
    {
        return _sm;
    }
}
