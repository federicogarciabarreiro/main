using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CubeScifiLiftable : MonoBehaviour, ILiftable, IInteractable, ILoadable {

    public enum conditionToRespawn
    {
        None,
        PlatformInInitialPos
    }

    class CheckpointState
    {
        public Vector3 position;
        public Quaternion rotation;
    }
    CheckpointState checkpointState = new CheckpointState();

    AudioSource _as;

    public Collider col { get; set; }
    Rigidbody rb;
    Material originalMaterial;
    Outline outline;
    Hero hero;
    Vector3 startPosition;
    public bool isStatic;
    public bool isLifted { get; set; }
    public Dictionary<Skill, Action> interactions = new Dictionary<Skill, Action>();
    List<Func<bool>> listOfConditionsToRespawn = new List<Func<bool>>();
    public conditionToRespawn condToRespawn;

    private void Start()
    {
        startPosition = transform.position;
        hero = Config.me.hero.GetComponent<Hero>();
        rb = GetComponent<Rigidbody>();
        AddToCheckpointList();
        SetContinuousRB();
        originalMaterial = transform.Find("Block").GetComponent<Renderer>().material;
        outline = transform.Find("Block").GetComponent<Outline>();
        interactions.Add(Skill.Lift, Lift);
        interactions.Add(Skill.EnergyShield, EnergyShield);
        AddToList();

        listOfConditionsToRespawn.Add(() => true);
        listOfConditionsToRespawn.Add(() => Config.me.platformMovable.localPosition == new Vector3(-12f, -1.6f, 178f));

        _as = GetComponent<AudioSource>();
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public void MoveToStartPosition()
    {
        StartCoroutine(MoveToStart(listOfConditionsToRespawn[(int)condToRespawn]));
    }

    public IEnumerator MoveToStart(Func<bool> cond)
    {
        transform.position = new Vector3(10000, 10000, 10000);
        yield return new WaitUntil(cond);
        yield return new WaitForSeconds(0.5f);
        transform.position = startPosition;
    }

    private void Update()
    {
        if(isLifted)
        {
            Vector3 origin = transform.position;
            Vector3 destination = hero.transform.position + Vector3.up + hero.transform.forward * 2;
            rb.velocity = (destination - origin) * 5f;
        }
    }

    public void Lift()
    {
        _as.pitch = 2f;
        _as.Play();

    }

    public void EnergyShield()
    {

    }

    public void LiftEnd(Vector3 destination)
    {
        _as.pitch = 1.5f;
        _as.Play();

        Vector3 origin = transform.position;
        rb.velocity = (destination - origin).normalized * 65f;
        gameObject.layer = 9;
    }

    public void TargetedState()
    {
        transform.Find("Block").GetComponent<Renderer>().material = Config.me.hologramMaterial;
    }

    public void UnselectedState()
    {
        transform.Find("Block").GetComponent<Renderer>().material = originalMaterial;
    }

    public bool CanInteract(Hero hero)
    {
        //RAY -------------------------------------------------------
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Transform target;
        if (Physics.Raycast(ray, out hit, 1000f, Config.me.objectsMask))
        {
            target = hit.transform;

        }
        else
        {
            target = null;
        }

        RaycastHit hit2;
        if (Physics.Raycast(transform.position, (hero.transform.position - (transform.position)).normalized, out hit2, 15f, Config.me.heroAndWallsMask))
        {

        }

        return target != null && hero.GetCC().isGrounded && hit.transform == transform && hit2.transform == hero.transform;

    }

    public void SwitchOutline(bool state, CursorTextureType type)
    {
        if(state)outline.OutlineWidth = 5f;
        else outline.OutlineWidth = 0f;
        CursorManager.me.SetCursorColor(state, type);
    }

    public void Interact(Skill interaction)
    {
        interactions[interaction]();
    }

    public void AddToList()
    {
        if(!isStatic)
            GameManager.me.im.AddInteractionObject(this, Skill.Lift);
        GameManager.me.im.AddInteractionObject(this, Skill.EnergyShield);
    }

    public void RemoveFromList()
    {
        if (!isStatic)
            GameManager.me.im.RemoveInteractionObject(this, Skill.Lift);
        GameManager.me.im.RemoveInteractionObject(this, Skill.EnergyShield);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Capsule>())
        {
            RemoveFromList();
            (hero.listOfSkills[(int)Skill.Lift] as LiftSkill).Reset();
            transform.position = new Vector3(10000, 10000, 10000);
            other.GetComponent<Capsule>().SwitchCubeScifiState(true);
        }

        //if (other.GetComponent<DoorOpener>())
        //{
        //    other.GetComponent<DoorOpener>().Interact(Skill.SlowTime);
        //}

        if (other.GetComponent<Enemy>())
        {
            if (isStatic)
            {
                print("COLISIONE CON ENEMIGO");
                rb.AddForce(GetClosestDirection(-(other.transform.position - transform.position).normalized) * 15000, ForceMode.Force);
            }
        }
    }

    Vector3 GetClosestDirection(Vector3 dir)
    {
        Dictionary<Vector3, Vector3> dic = new Dictionary<Vector3, Vector3>();
        dir.y = 0;
        dic[Vector3.forward - dir] = Vector3.forward;
        dic[Vector3.back - dir] = Vector3.back;
        dic[Vector3.right - dir] = Vector3.right;
        dic[Vector3.left - dir] = Vector3.left;

        return dic[dic.Select(x => x.Key).OrderBy(x => x.magnitude).First()];
    }

    public void OnDestroy()
    {
        RemoveFromCheckpointList();
        RemoveFromList();
    }

    public Transform GetTransform()
    {
        return transform;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.GetComponent<Enemy>())
        {
            if(other.relativeVelocity.magnitude > 15f)
                other.collider.GetComponent<Enemy>().Die();
        }

        if (other.collider.GetComponentInParent<DoorOpener>())
        {
            other.collider.GetComponentInParent<DoorOpener>().Interact(Skill.SlowTime);
        }
    }

    public void SetContinuousRB()
    {
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
    }

    public Rigidbody GetRigidbody()
    {
        return rb;
    }

    public void SaveForCheckpoint()
    {
        checkpointState.position = transform.position;
        checkpointState.rotation = transform.rotation;
    }

    public void LoadForCheckpoint()
    {
        transform.position = checkpointState.position;
        transform.rotation = checkpointState.rotation;
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
