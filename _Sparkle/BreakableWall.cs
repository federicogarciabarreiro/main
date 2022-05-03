using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableWall : MonoBehaviour, IBreakable, IInteractable, ILoadable {

    public class CheckpointState
    {
        public Vector3 position;
        public Quaternion rotation;
        public bool exists;
        public bool canRespawn;
    }
    public CheckpointState checkpointState = new CheckpointState();

    AudioSource _as;

    public bool affectedByTimeStop;
    public Transform graphic;
    Outline outline;
    public GameEvent eventToTrigger;
    bool eventTriggered;

    public void Awake()
    {
        outline = GetComponent<Outline>();
    }

    private void Start()
    {
        AddToList();
        AddToCheckpointList();

        _as = GetComponent<AudioSource>();
    }

    public void Break()
    {
        _as.Play();

        if (!affectedByTimeStop) StartCoroutine(BreakOnly());
        else StartCoroutine(BreakAndRewind());
    }
    IEnumerator BreakOnly()
    {
        checkpointState.exists = false;
        RemoveFromList();

        GetComponent<BoxCollider>().enabled = false;
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
            if (child.GetComponent<GlassFragment>())
                child.GetComponent<GlassFragment>().Break();
        }

        graphic.gameObject.SetActive(false);
        yield return new WaitForSeconds(6.5f);
        Destroy(gameObject);
    }
    IEnumerator BreakAndRewind()
    {
        GetComponent<BoxCollider>().enabled = false;
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
            if (child.GetComponent<GlassFragment>())
            {
                child.GetComponent<GlassFragment>().Break();
                child.GetComponent<GlassFragment>().StartRewindMode();
            }
        }

        graphic.GetComponent<Renderer>().enabled = false;

        yield return new WaitForSeconds(0.5f);

        foreach (Transform child in transform)
        {
            if (child.GetComponent<GlassFragment>())
                child.GetComponent<GlassFragment>().rewindON = true;
        }

        yield return new WaitForSeconds(0.5f);

        GetComponent<BoxCollider>().enabled = true;
        foreach (Transform child in transform)
        {
            if (child.GetComponent<GlassFragment>())
            {
                child.GetComponent<GlassFragment>().rewindON = false;
                child.GetComponent<GlassFragment>().StopRewindMode();
                child.transform.position = child.GetComponent<GlassFragment>().startPos;
                child.transform.rotation = child.GetComponent<GlassFragment>().startRot;
                child.gameObject.SetActive(false);
            }     
        }
        graphic.GetComponent<Renderer>().enabled = true;

    }
    public void SwitchBreakableState(bool state)
    {
        affectedByTimeStop = state;
    }


    public void SwitchOutline(bool state, CursorTextureType type)
    {
        outline.enabled = state;
        if (state == true && !eventTriggered)
        {
            GameManager.me.em.SetEvent(eventToTrigger);
            eventTriggered = true;
        }
        
    }

    public bool CanInteract(Hero hero)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up, (hero.transform.position - (transform.position + Vector3.up)).normalized, out hit, 10f))
        {
            
        }
        Collider obj = hero.CheckForObjectInRadius(hero.transform.position + hero.transform.forward * 3, 3, Config.me.breakableMask);
        return obj != null && hero.GetCC().isGrounded && hit.transform == hero.transform;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public void Interact(Skill interaction)
    {
        //var mode = hero.listOfTelekinesisSkills[(int)TelekinesisSkill.BreakObject] as BreakTelekinesisSkill;
        //mode.target = GetComponent<IBreakable>();
        //hero.SetTelekinesisSkill(TelekinesisSkill.BreakObject);
        //Camera.main.GetComponent<CameraShake>().ShakeCamera(1f,0.1f);
    }
    public void AddToList()
    {
        GameManager.me.im.AddInteractionObject(this, Skill.Lift);
    }

    public void RemoveFromList()
    {
        GameManager.me.im.RemoveInteractionObject(this, Skill.Lift);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.relativeVelocity.magnitude > 20f)
        {
            other.collider.attachedRigidbody.velocity = Vector3.zero;
            Break();
        }


    }

    public void SaveForCheckpoint()
    {
        if (this)
        {
            checkpointState.position = transform.position;
            checkpointState.rotation = transform.rotation;
        }

        checkpointState.canRespawn = checkpointState.exists;
    }

    public void LoadForCheckpoint()
    {
        if (!checkpointState.exists && checkpointState.canRespawn)
        {
            GameObject wall = Instantiate(Config.me.listOfPrefabs[(int)Prefabs.BreakableWall]);
            wall.transform.position = checkpointState.position;
            wall.transform.rotation = checkpointState.rotation;
            wall.GetComponent<BreakableWall>().checkpointState.position = checkpointState.position;
            wall.GetComponent<BreakableWall>().checkpointState.rotation = checkpointState.rotation;
            wall.GetComponent<BreakableWall>().checkpointState.canRespawn = checkpointState.canRespawn;
            GameManager.me.StartCoroutine(GameManager.me.RemoveFromCheckpointList(this, 0.5f));
        }
    }

    public void AddToCheckpointList()
    {
        GameManager.me.lisfOfCheckpointObjects.Add(this);
        checkpointState.exists = true;
    }

    public void RemoveFromCheckpointList()
    {
        GameManager.me.lisfOfCheckpointObjects.Remove(this);
    }
}
