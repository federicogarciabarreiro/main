using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.FinalIK;

public class Box : MonoBehaviour, IRepelable, IInteractable {

    public Collider col { get; set; }
    Material originalMaterial;
    Rigidbody rb;
    CharacterController cc;
    State lastState;
    Outline outline;
    bool eventTriggered;
    public GameEvent eventToTrigger;
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

    private Memento _memento = new Memento();

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        outline = GetComponent<Outline>();
        AddToList();
        lastState = new State(Vector3.zero, Quaternion.identity);
        originalMaterial = GetComponent<Renderer>().material;
        col = GetComponent<Collider>();
        //StartCoroutine(SaveState());
    }

    void Update()
    {
        rb.velocity = new Vector3(0, rb.velocity.y, 0);
        //Rewind();

        if(Input.GetKeyDown(KeyCode.R))
        {
            GetComponent<Renderer>().material = Config.me.hologramMaterial;
        }
        if(Input.GetKeyUp(KeyCode.R))
        {
            GetComponent<Renderer>().material = originalMaterial;
        }
    }

    public void Move(Vector3 dir)
    {
        Vector3 destination = transform.position + dir * 0.1f;
        destination.y = transform.position.y;
        transform.position = Vector3.MoveTowards(transform.position, destination, 0.025f);
        //dir.y = rb.velocity.y;
        //rb.velocity = dir;
    }
    public void Repelled(Vector3 dir)
    {
        //rb.velocity = dir * 3f;
    }

    public void Lift()
    {
        Hero caster = Config.me.hero.GetComponent<Hero>();
        Vector3 origin = transform.position;
        Vector3 destination = caster.transform.position + Vector3.up + caster.transform.forward * 2;
        //rb.velocity = (destination - origin) * 10f;
    }

    public void LiftEnd(Hero caster, Vector3 destination)
    {
        Vector3 origin = transform.position;
        //rb.velocity = Vector3.zero;
        //rb.velocity = (destination - origin).normalized * (rb.mass *0.65f);
    }

    public void UnselectedState()
    {
        GetComponent<Renderer>().material = originalMaterial;
    }

    public void TargetedState()
    {
        GetComponent<Renderer>().material = Config.me.hologramMaterial;
    }

    //public IEnumerator SaveState()
    //{
    //    //Entry
    //    //rb.isKinematic = false;
    //    //Action
    //    while (!GameManager.gameIsRewinding)
    //    {
    //        if (transform.position != lastState.pos)
    //        {
    //            lastState = new State(transform.position, transform.rotation);
    //            _memento.states.Add(lastState);
    //        }
    //        yield return new WaitForSeconds(1 / _memento.framesPerSecond);
    //    }
    //    //Exit
    //    StartCoroutine(LoadState());
    //}

    //public IEnumerator LoadState()
    //{
    //    //Entry
    //    //rb.isKinematic = true;
    //    //Action
    //    while (GameManager.gameIsRewinding)
    //    {
    //        if (_memento.states.Count > 0)
    //            _memento.states.RemoveAt(_memento.states.Count - 1);
    //        yield return new WaitForSeconds(0.35f / _memento.framesPerSecond);
    //    }
    //    //Exit
    //    StartCoroutine(SaveState());
    //}

    //public void Rewind()
    //{   
    //    if (GameManager.gameIsRewinding && _memento.states.Count > 0)
    //    {
    //        var pos = (_memento.states[_memento.states.Count - 1] as State).pos;
    //        var rot = (_memento.states[_memento.states.Count - 1] as State).rotation;
    //        transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime * _memento.framesPerSecond);
    //        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * _memento.framesPerSecond);
    //    }
    //}

    void OnCollisionEnter(Collision other)
    {
        
        //if(other.collider.GetComponent<Enemy>() && (Mathf.Abs(rb.velocity.x) > 5|| Mathf.Abs(rb.velocity.z) > 5))
        //{
        //    other.collider.GetComponent<Enemy>().Die();
        //}
    }

    public void SwitchOutline(bool state, CursorTextureType type)
    {
        outline.enabled = state;
        CursorManager.me.SetCursorColor(state, type);

        if (!eventTriggered && state)
        {
            GameManager.me.em.SetEvent(eventToTrigger);
            eventTriggered = true;
        }
    }

    public bool CanInteract(Hero hero)
    {
        return hero.GetCC().isGrounded 
            && hero.GetIS().GetClosestTriggerIndex() != -1;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public void Interact(Skill interaction)
    {
        Hero hero = Config.me.GetComponent<Hero>();
        if (hero.GetCC().isGrounded)
        {
            if (hero.GetIS().GetClosestTriggerIndex() != -1 && hero.movementMode != MovementMode.Drag)
            {
                hero.GetIS().TriggerInteraction(hero.GetIS().GetClosestTriggerIndex(), false);
                hero.SetIO(hero.GetIS().GetInteractionObject(FullBodyBipedEffector.RightHand));
                if (hero.GetIS().GetInteractionObject(FullBodyBipedEffector.RightHand).GetComponent<Box>())
                    hero.SetMovementMode(MovementMode.Drag);
            }
            else
            {
                if (hero.GetIO() != null)
                    hero.GetIO().GetComponent<Box>().Move(Vector3.zero);
                if (hero.movementMode == MovementMode.Drag)
                    hero.SetMovementMode(MovementMode.Normal);
                hero.GetIS().ResumeInteraction(FullBodyBipedEffector.LeftHand);
                hero.GetIS().ResumeInteraction(FullBodyBipedEffector.RightHand);
            }
        }
    }

    public void AddToList()
    {
        GameManager.me.im.AddInteractionObject(this, Skill.Lift);
    }

    public void RemoveFromList()
    {
        GameManager.me.im.RemoveInteractionObject(this, Skill.Lift);
    }

    public void OnMouseEnter()
    {
        //CursorManager.me.SetCursorType(CursorTextureType.NORMAL_GREEN);
    }

    public void OnMouseExit()
    {
        //CursorManager.me.SetCursorType(CursorTextureType.NORMAL_WHITE);
    }
}
