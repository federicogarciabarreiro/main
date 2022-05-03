using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using RootMotion.FinalIK;

public class State : IState
{
    public Vector3 pos;
    public Quaternion rotation;
    
    public State(Vector3 pos, Quaternion rotation)
    {
        this.pos = pos;
        this.rotation = rotation;
    }
}

public enum EnemyActionState
{
    Idle,
    Shoot,
    Death,
    Turn,
    None
}

public class Enemy : MonoBehaviour, ITimeSlowable, IInteractable, ILoadable
{

    public enum EnemyBrain
    {
        Self,
        MindControlled
    }


    public class CheckpointState
    {
        public Vector3 position;
        public Quaternion rotation;
        public bool ccControllerState;
    }
    public CheckpointState checkpointState = new CheckpointState();

    public AudioClip[] sounds;
    public bool canPlaySound;

    public float distanceToShoot = 11f;
    public bool isPassiveEnemy;
    public GameObject shockWave;
    public Transform soldier;
    public Transform ragdoll;
    public Transform ragdollPelvis;
    public Transform SP;

    public GameObject bulletPrefab;
    Animator anim;
    public float speed;
    List<Func<bool>> listOfFunctions = new List<Func<bool>>();
    public int currentState;

    Vector3 startPosition;

    float timeToPatrol;
    private Memento _memento = new Memento();
    public Goal goalToTrigger;
    public LineRenderer lr;
    Material originalMaterial;
    public EnemyActionState smState;


    IEnemyBrain currentBrain;
    public EnemyBrain brainEnum;
    List<IEnemyBrain> brains = new List<IEnemyBrain>();
    void Start()
    {
        AddToList();
        AddToCheckpointList();

        originalMaterial = transform.Find("Soldier").Find("suit_low").GetComponent<Renderer>().material;

        anim = transform.Find("Soldier").GetComponent<Animator>();
        startPosition = transform.position;

        brains.Add(new SelfControlledEnemyBrain(this));
        brains.Add(new MindControlledEnemyBrain(this));

        SetBrain(EnemyBrain.Self);
    }
    void Update()
    {
        currentBrain.Action();
    }
    public void SetBrain(EnemyBrain brain)
    {
        if (currentBrain != null)
            currentBrain.Exit();
        currentBrain = brains[(int)brain];
        currentBrain.Enter();
        brainEnum = brain;
    }
    public void Die()
    {
        anim.SetTrigger("Death");
        currentBrain.GetSM().SetState(EnemyActionState.Death);
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }


    public bool GetIfGrounded()
    {
        return GetComponent<CharacterController>().isGrounded;
    }

    public Animator GetAnim()
    {
        return anim;
    }
    public bool CanInteract(Hero hero)
    {
        //RAY -------------------------------------------------------
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Transform target;
        if (Physics.Raycast(ray, out hit, 1000f, Config.me.enemyLayer))
        {
            target = hit.transform;
        }
        else
        {
            target = null;
        }

        RaycastHit hit2;
        if (Physics.Raycast(transform.position, (hero.transform.position - transform.position).normalized, out hit2, 1000f))
        {

        }

        return target != null && hero.GetCC().isGrounded && hit.transform.IsChildOf(transform) && hit2.transform == hero.transform && Vector3.Distance(hero.transform.position, transform.position) < 10f;

    }
    public void SwitchOutline(bool state, CursorTextureType type)
    {
        //CursorManager.me.SetCursorColor(state, type);
    }
    public void Interact(Skill interaction)
    {
        //SwitchOutline(false, CursorTextureType.SECONDARY);

    }
    public void AddToList()
    {
        GameManager.me.im.AddInteractionObject(this, Skill.SlowTime);
        GameManager.me.im.AddInteractionObject(this, Skill.MindControl);
    }
    public void RemoveFromList()
    {
        GameManager.me.im.RemoveInteractionObject(this, Skill.SlowTime);
        GameManager.me.im.RemoveInteractionObject(this, Skill.MindControl);
    }
    public void SlowDown(int xTimes)
    {
        anim.speed = 0.2f;
        if (currentBrain is SelfControlledEnemyBrain)
        {
            (currentBrain as SelfControlledEnemyBrain).cooldownToShoot *= 5;
            (currentBrain as SelfControlledEnemyBrain).turningSpeed /= 5;
        }

        //cooldownToShoot *= 5;
        //turningSpeed /= 5;
        transform.Find("Soldier").Find("suit_low").GetComponent<Renderer>().material = Config.me.timeSlowMaterial;
    }
    public void Reset()
    {
        anim.speed = 1;
        if (currentBrain is SelfControlledEnemyBrain)
        {
            (currentBrain as SelfControlledEnemyBrain).cooldownToShoot = (currentBrain as SelfControlledEnemyBrain).initialCooldownToShoot;
            (currentBrain as SelfControlledEnemyBrain).turningSpeed = (currentBrain as SelfControlledEnemyBrain).initialTurningSpeed;
        }
        transform.Find("Soldier").Find("suit_low").GetComponent<Renderer>().material = originalMaterial;
    }
    public Transform GetTransform()
    {
        return transform;
    }
    public void SaveForCheckpoint()
    {
        checkpointState.position = transform.position;
        checkpointState.rotation = transform.rotation;
        checkpointState.ccControllerState = GetComponent<CharacterController>().enabled;
    }

    private void OnDestroy()
    {
        RemoveFromList();
    }
    public void LoadForCheckpoint()
    {
        transform.position = checkpointState.position;
        transform.rotation = checkpointState.rotation;
        GetComponent<CharacterController>().enabled = checkpointState.ccControllerState;
        soldier.gameObject.SetActive(checkpointState.ccControllerState);
        GetComponent<BoxCollider>().enabled = checkpointState.ccControllerState;
        if(checkpointState.ccControllerState)
            currentBrain.GetSM().SetState(EnemyActionState.Idle);
        else
            currentBrain.GetSM().SetState(EnemyActionState.None);

    }

    public IEnemyBrain GetCurrentBrain()
    {
        return currentBrain;
    }
    public void AddToCheckpointList()
    {
        GameManager.me.lisfOfCheckpointObjects.Add(this);
    }
    public void RemoveFromCheckpointList()
    {
        GameManager.me.lisfOfCheckpointObjects.Remove(this);
    }
}
