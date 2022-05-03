using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.FinalIK;
using System;

public enum GravityMode
{
    Normal,
    None
}

public enum MovementMode
{
    None,
    Normal,
    Drag,
    Pipelines,
    Lineal
}

public enum Skill
{
    Lift,
    SlowTime,
    EnergyShield,
    MindControl,
    None,
    Count
}

public enum Death
{
    Laser,
    FrontHit,
    BackHit,
    SidewaysHit
}
public class Hero : MonoBehaviour, ILoadable {

    class CheckpointState
    {
        public Vector3 position;
        public Quaternion rotation;
    }
    CheckpointState checkpointState = new CheckpointState();

    public MovementMode movementMode;
    List<IMovementMode> listOfMovementModes = new List<IMovementMode>();
    public IMovementMode currentMovementMode;

    public GravityMode gravityMode;
    List<IGravityMode> listOfGravityModes = new List<IGravityMode>();


    public List<Skill> skillTypes = new List<Skill>();
    public List<ISkill> listOfSkills = new List<ISkill>();
    public List<ISkill> heroSkills = new List<ISkill>();

    AudioSource _as;
    bool freezed;
    bool positionFreezed;
    public AudioClip[] sounds;
    InteractionSystem IS;
    InteractionObject sliderInteraction;
    InteractionObject telekinesisHandInteractive;
    Animator anim;
    CharacterController cc;
    Vector3 dir;

    Vector3 targetPosition;
    Transform timeBubble;
    Transform timeStopBubble;

    bool isLifting;
    bool timeStopBubbleON;

    Collider currentSlider;
    bool groundChecker;
    bool sliderChecker;

    InteractionObject currentIO;
    public bool shielded;
    Material originalMaterial;
    Material switchSkillMaterial;
    bool canSwitchSkill = true;
    bool dead;

    private void Awake()
    {
        anim = transform.Find("Boy").GetComponent<Animator>();
    }

    void Start()
    {
        originalMaterial = Config.me.heroOriginalMaterial;
        switchSkillMaterial = Config.me.heroChangeSkillMaterial;
        _as = GetComponent<AudioSource>();
        AddToCheckpointList();
        cc = GetComponent<CharacterController>();
        cc.detectCollisions = true;
        cc.enableOverlapRecovery = true;
        anim = transform.Find("Boy").GetComponent<Animator>();
        IS = GetComponent<InteractionSystem>();
        sliderInteraction = transform.Find("SliderInteractive").GetComponent<InteractionObject>();

        timeBubble = Config.me.timeBubble;
        timeStopBubble = Config.me.timeStopBubble;

        listOfGravityModes.Add(new NormalGravityMode(this, cc, anim));
        listOfGravityModes.Add(new NoneGravityMode());

        listOfMovementModes.Add(new NoneMovementMode(anim, GetGravityMode(GravityMode.Normal)));
        listOfMovementModes.Add(new NormalMovementMode(this, cc, anim, GetGravityMode(GravityMode.Normal)));
        listOfMovementModes.Add(new DragMovementMode(this, cc, anim, GetGravityMode(GravityMode.Normal)));
        listOfMovementModes.Add(new PipelinesMovementMode(this, transform.Find("Boy"), anim, GetGravityMode(GravityMode.None)));
        listOfMovementModes.Add(new LinealMovementMode(this, cc, anim, GetGravityMode(GravityMode.Normal)));

        listOfSkills.Add(new LiftSkill(anim));
        listOfSkills.Add(new SlowTimeSkill());
        listOfSkills.Add(new EnergyShieldSkill(anim, Config.me.shockWave.GetComponent<ParticleSystem>()));
        listOfSkills.Add(new MindControlSkill(anim));
        listOfSkills.Add(new NoneSkill());

        skillTypes.Add(Skill.Lift);
        skillTypes.Add(Skill.SlowTime);
        skillTypes.Add(Skill.EnergyShield);
        skillTypes.Add(Skill.MindControl);
        skillTypes.Add(Skill.None);

        for (int i = 0; i < (int)Skill.Count; i++)
            heroSkills.Add(listOfSkills[(int)Skill.None]);

        SetMovementMode(MovementMode.Normal);

        SetSkill(Skill.Lift, KeyCode.Mouse0);
        SetSkill(Skill.SlowTime, KeyCode.Mouse1);
        SetSkill(Skill.EnergyShield, KeyCode.Q);
        SetSkill(Skill.MindControl, KeyCode.F);
    }
    void Update()
    {
        if(!GetFreezedState())
        {
            heroSkills[(int)Skill.Lift].Action();
            heroSkills[(int)Skill.SlowTime].Action();
            heroSkills[(int)Skill.EnergyShield].Action();
        }
        heroSkills[(int)Skill.MindControl].Action();

        currentMovementMode.Action(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal"));
    }
    public void Interact(Skill interaction)
    {
        var obj = GameManager.me.im.GetClosestInteractionObject(interaction);
        if (obj != null && !freezed)
        {
            obj.Interact(interaction);
            if(listOfSkills[(int)interaction].GetInteractiveObject() == null)
                listOfSkills[(int)interaction].SetInteractiveObject(obj);
        }
            
    }

    public void SetShieldState(bool state)
    {
        shielded = state;
    }

    public bool GetShieldState()
    {
        return shielded;
    }
    public void StopHandsInteractions()
    {
        GetIS().StopInteraction(FullBodyBipedEffector.LeftHand);
        GetIS().StopInteraction(FullBodyBipedEffector.RightHand);
    }

    public void Freeze(bool state)
    {
        foreach (var item in heroSkills)
            item.Exit();
        freezed = state;
    }

    public void FreezePosition(bool state)
    {
        positionFreezed = state;
    }

    public bool GetFreezedState()
    {
        return freezed;
    }

    public bool GetFreezedPositionState()
    {
        return positionFreezed;
    }

    public bool isDead()
    {
        return dead;
    }

    public void Die(Death deathType)
    {
        if(!freezed)
        {
            dead = true;
            if (deathType == Death.Laser)
            {
                ApplyForce(-transform.forward * 500 * Time.deltaTime);
                RemoveForce(5f);
                anim.SetTrigger("DeathLaser");
            }
            if (deathType == Death.FrontHit)
            {
                anim.SetTrigger("DeathFrontHit");
            }
           if(deathType == Death.BackHit)
            {
                anim.SetTrigger("DeathBackHit");
            }

            StartCoroutine(ChangeLayer(14));
            StopHandsInteractions();
            foreach (var item in heroSkills)
                item.Exit();
            
            GameManager.me.StartCoroutine("RestartGame");
            freezed = true;
        }

    }

    IEnumerator ChangeLayer(int layer)
    {
       yield return new WaitForSeconds(0.5f);
       gameObject.layer = layer;
       yield return new WaitForSeconds(5.25f);
       gameObject.layer = 13;

    }
    public void SetMovementMode(MovementMode mode)
    {
        if(currentMovementMode != null)
            currentMovementMode.Exit();
        currentMovementMode = listOfMovementModes[(int)mode];
        currentMovementMode.Enter();
        movementMode = mode;
        gravityMode = (GravityMode)listOfGravityModes.IndexOf(currentMovementMode.gravityMode);
    }
    //public void SetSkill(Skill skill, MouseSkill mouseSkill)
    //{
    //    int type = (int)mouseSkill;
    //    if(type == 0)
    //    {
    //        if (primarySkill != null)
    //            primarySkill.Exit();
    //        primarySkill = listOfSkills[(int)skill];
    //        primarySkill.Enter();
    //        primarySkillType = skill;
    //    }

    //    if(type == 1)
    //    {
    //        if (secondarySkill != null)
    //            secondarySkill.Exit();
    //        secondarySkill = listOfSkills[(int)skill];
    //        secondarySkill.Enter();
    //        secondarySkillType = skill;
    //    }
    //}

    public void SetSkill(Skill skill, KeyCode key)
    {
        heroSkills[(int)skill].Exit();
        heroSkills[(int)skill] = listOfSkills[(int)skill];
        heroSkills[(int)skill].Enter();
        heroSkills[(int)skill].SetKey(key);
    }

    public IGravityMode GetGravityMode(GravityMode mode)
    {
        return listOfGravityModes[(int)mode];
    }
    public void ConfigPipelinesMovementMode(List<PipelinesWaypoint> list, Transform entrance, Transform exit, bool IsEntrance)
    {
        if (currentMovementMode is PipelinesMovementMode)
        {
            var mode = listOfMovementModes[(int)MovementMode.Pipelines] as PipelinesMovementMode;
            mode.waypoints = list;
            mode.entrance = entrance;
            mode.exit = exit;
            mode.Settings(IsEntrance);
        }
    }
    public void Boost(float multiplier, bool boosting)
    {
        if(currentMovementMode is NormalMovementMode)
        {
            var mode = listOfMovementModes[(int)MovementMode.Normal] as NormalMovementMode;
            mode.speed = mode.initialSpeed * multiplier;
            anim.SetBool("boostingMode", boosting);
        }
    }

    public void ApplyForce(Vector3 dir)
    {
        if (currentMovementMode is NormalMovementMode)
        {
            var mode = listOfMovementModes[(int)MovementMode.Normal] as NormalMovementMode;
            mode.forceDir = dir;
        }
    }

    public void RemoveForce(float speed)
    {
        StartCoroutine(RemoveForceOnTime(speed));
    }

    public IEnumerator RemoveForceOnTime(float speed)
    {
        var time = Time.time;
        var mode = listOfMovementModes[(int)MovementMode.Normal] as NormalMovementMode;
        
        while (mode.forceDir != Vector3.zero)
        {

            mode.forceDir = Vector3.Lerp(mode.forceDir, Vector3.zero, speed * Time.deltaTime);
            yield return new WaitForSeconds(Time.deltaTime * speed / 10);
        }
        print(Math.Abs(time - Time.time));
    }
    public void Jump()
    {  
        if (cc.isGrounded && currentMovementMode is NormalMovementMode &&!freezed)
        {
            var mode = listOfMovementModes[(int)MovementMode.Normal] as NormalMovementMode;
            mode.dir.y = mode.jumpForce;
            anim.SetTrigger("JumpTrigger");

            //CODIGOCABEZA
            _as.clip = sounds[0];
            _as.Play();
        }
    }

    public void AddForce(Vector3 forceToAdd, string hitType)
    {
        if (/*cc.isGrounded && */currentMovementMode is NormalMovementMode)
        {
            var mode = listOfMovementModes[(int)MovementMode.Normal] as NormalMovementMode;
            mode.addedForce = forceToAdd;
            anim.SetTrigger(hitType);

            //CODIGOCABEZA
            _as.clip = sounds[0];
            _as.Play();
        }
    }

    public Collider CheckForObjectInRadius(Vector3 pos, float radius, LayerMask mask)
    {
        return Physics.OverlapSphere(pos, radius, mask).FirstOrDefault();
    }
    public bool GetIfGrounded()
    {
        return cc.isGrounded;
    }
    public InteractionSystem GetIS()
    {
        return IS;
    }
    public InteractionObject GetIO()
    {
        return currentIO;
    }
    public void SetIO(InteractionObject obj)
    {
        currentIO = obj;
    }
    public CharacterController GetCC()
    {
        return cc;
    }

    public Animator GetAnim()
    {
        return anim;
    }
    public void ActivateWaterBlocker()
    {
        var waterBlocker = CheckForObjectInRadius(transform.position + transform.forward * 1.5f, 1.5f, Config.me.waterBlocker);
        if(waterBlocker)
            waterBlocker.GetComponent<WaterBlocker>().SwitchState();
    }
    IEnumerator DeactivateCollider(Collider col)
    {
        col.enabled = false;
        yield return new WaitForSeconds(0.5f);
        col.enabled = true;
    }
    public void StopInteraction(InteractionObject io)
    {
        if(IS.GetInteractionObject(FullBodyBipedEffector.LeftHand) == io)
        {
            IS.StopInteraction(FullBodyBipedEffector.LeftHand);
            IS.StopInteraction(FullBodyBipedEffector.RightHand);
            IS.StopInteraction(FullBodyBipedEffector.LeftFoot);
            IS.StopInteraction(FullBodyBipedEffector.RightFoot);
        }
    }
    public void Repulsion()
    {
        var objectsInArea = Physics.OverlapSphere(transform.position + transform.forward * 5f, 10f, Config.me.repelableMask).ToList();
        foreach (var item in objectsInArea)
        {
            item.GetComponent<IRepelable>().Repelled((item.transform.position - transform.position) + item.transform.up);
        }
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
        Camera.main.transform.position = Config.me.hero.transform.position;
        freezed = false;
        anim.SetTrigger("IdleTrigger");
        SetMovementMode(MovementMode.Normal);
        dead = false;
    }

    public void AddToCheckpointList()
    {
        GameManager.me.lisfOfCheckpointObjects.Add(this);
    }

    public void RemoveFromCheckpointList()
    {
        GameManager.me.lisfOfCheckpointObjects.Remove(this);
    }

    public IEnumerator SetNewSkillAnimation(Skill skill, Color matColor)
    {
        canSwitchSkill = false;
        anim.SetTrigger("BreakTrigger");
        transform.Find("Boy").Find("boy").GetComponent<Renderer>().material = switchSkillMaterial;
        transform.Find("Boy").Find("boy").GetComponent<Renderer>().material.color = matColor;
        yield return new WaitForSeconds(0.75f);
        transform.Find("Boy").Find("boy").GetComponent<Renderer>().material = originalMaterial;
        canSwitchSkill = true;
    }

    public bool CanSwitchSkill()
    {
        return canSwitchSkill;
    }
}
