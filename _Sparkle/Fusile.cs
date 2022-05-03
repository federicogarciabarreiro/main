using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fusile : MonoBehaviour, ILiftable, IInteractable, ILoadable {

    class CheckpointState
    {
        public Quaternion rotation;
        public Vector3 position;
        public FusileBox currentFusileBox;
    }
    CheckpointState checkpointState = new CheckpointState();
    public Collider col { get; set; }
    public bool isLifted { get; set; }
    public FusileType typeOfFusile;
    Outline outline;
    Rigidbody rb;
    Hero hero;
    FusileBox currentFusileBox;
    public Dictionary<Skill, Action> interactions = new Dictionary<Skill, Action>();

    void Start()
    {
        AddToCheckpointList();
        outline = GetComponent<Outline>();
        rb = GetComponent<Rigidbody>();
        SetContinuousRB();
        AddToList();
        interactions.Add(Skill.Lift, Lift);
        interactions.Add(Skill.EnergyShield, EnergyShield);

    }

    void Update()
    {
        if (isLifted)
        {
            Vector3 origin = transform.position;
            Vector3 destination = hero.transform.position + Vector3.up * 1.5f + hero.transform.forward * 2;
            rb.velocity = (destination - origin) * 5;
        }
    }


    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public void EnergyShield()
    {

    }

    public void Lift()
    {
        StartCoroutine(FusileBoxTransp());
        hero = Config.me.hero.GetComponent<Hero>();
        rb.isKinematic = false;
    }

    IEnumerator FusileBoxTransp()
    {
        if(currentFusileBox)
        {
            currentFusileBox.SetFusile(typeOfFusile, false);
            currentFusileBox.gameObject.layer = 23;
            yield return new WaitForSeconds(0.65f);
            currentFusileBox.gameObject.layer = 9;
            currentFusileBox = null;
        }

    }

    IEnumerator FusileInactive()
    {
        RemoveFromList();
        yield return new WaitForSeconds(1f);
        AddToList();
    }

    public void LiftEnd(Vector3 destination)
    {
        Vector3 origin = transform.position;
        rb.velocity = (destination - origin).normalized * 40f;
        gameObject.layer = 9;
    }

    public void TargetedState()
    {
        
    }

    public void UnselectedState()
    {
        
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
        if (Physics.Raycast(transform.position, (hero.transform.position - transform.position).normalized, out hit2, 15f, Config.me.heroAndWallsMask))
        {
            //Debug.Log(hit.transform);
        }

        return target != null && hero.GetCC().isGrounded && hit.transform == transform && hit2.transform == hero.transform;
    }

    public void SwitchOutline(bool state, CursorTextureType type)
    {
        if (state) outline.OutlineWidth = 5f;
        else outline.OutlineWidth = 0f;
        CursorManager.me.SetCursorColor(state, type);
    }

    public void Interact(Skill interaction)
    {
        interactions[interaction]();
    }

    public void AddToList()
    {
        GameManager.me.im.AddInteractionObject(this, Skill.Lift);
        GameManager.me.im.AddInteractionObject(this, Skill.EnergyShield);
    }

    public void RemoveFromList()
    {
        GameManager.me.im.RemoveInteractionObject(this, Skill.Lift);
        GameManager.me.im.RemoveInteractionObject(this, Skill.EnergyShield);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.GetComponent<FusileBox>())
        {
            if(other.collider.GetComponent<FusileBox>().GetFusile() == FusileType.None && currentFusileBox == null)
            {
                currentFusileBox = other.collider.GetComponent<FusileBox>();
                currentFusileBox.SetFusile(typeOfFusile, true);
                isLifted = false;
                StartCoroutine(FusileInactive());
                (hero.listOfSkills[(int)Skill.Lift] as LiftSkill).Exit();
                transform.position = other.transform.position + (other.transform.forward * GetComponent<Collider>().bounds.size.x/3);
                transform.right = other.transform.forward;
                rb.isKinematic = true;
            }
        }

        if (other.collider.GetComponentInParent<DoorOpener>())
        {
            other.collider.GetComponentInParent<DoorOpener>().Interact(Skill.SlowTime);
        }


    }

    private void OnTriggerEnter(Collider other)
    {
        //if (other.GetComponent<DoorOpener>())
        //{
        //    other.GetComponent<DoorOpener>().Interact(Skill.SlowTime);
        //}
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
        checkpointState.currentFusileBox = currentFusileBox;
    }

    public void LoadForCheckpoint()
    {
        transform.position = checkpointState.position;
        transform.rotation = checkpointState.rotation;
        currentFusileBox = checkpointState.currentFusileBox;
        rb.isKinematic = true;
        if(currentFusileBox)
        {
            if(Vector3.Distance(currentFusileBox.transform.position, transform.position) > 1f)
            {
                currentFusileBox.SetFusile(typeOfFusile, false);
                currentFusileBox = null;
            }
        }
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
