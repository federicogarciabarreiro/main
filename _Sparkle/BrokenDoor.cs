using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenDoor : Door, ITimeSlowable, IDoor, IInteractable, ILoadable {

    class CheckpointState
    {
        public Vector3 position;
        public bool opening;
        public Vector3 targetPos;
        public Color currentColor;
    }
    CheckpointState checkpointState = new CheckpointState();

    AudioSource _as;
    bool opening;
    bool opened;
    List<Material> originalMaterials = new List<Material>();
    public List<Transform> ToChangeMaterial;
    public List<Transform> ToChangeEmissiveMaterial;
    Outline outline;
    Vector3 initialPos;
    public Transform passBlocker;
    public Color currentColor;
    public bool isPlatform;

    private void Awake()
    {
        speed = 3.5f;
        startPos = transform.localPosition;
        finalPos = transform.localPosition + Vector3.up * GetComponent<Collider>().bounds.size.y;
        targetPos = finalPos;
    }
    void Start()
    {
        SetEmissiveColor(Color.red);
        AddToCheckpointList();
        foreach (var item in ToChangeMaterial)
        {
            originalMaterials.Add(item.GetComponent<Renderer>().material);
        }
        initialSpeed = speed;
        outline = GetComponent<Outline>();
        initialPos = transform.position;
        AddToList();

      
        _as = GetComponent<AudioSource>();
    }
    public void SlowDown(int xTimes)
    {
        speed = initialSpeed / xTimes;
        foreach (var item in ToChangeMaterial)
            item.GetComponent<Renderer>().material = Config.me.timeSlowMaterial;

    }

    public void Reset()
    {
        speed = initialSpeed;
        foreach (var item in ToChangeMaterial)
            item.GetComponent<Renderer>().material = originalMaterials[ToChangeMaterial.IndexOf(item)];
            
    }

    public void Open()
    {
        StartCoroutine(Transition());
        opened = false;
    }

    public void OpenOnly()
    {
        StartCoroutine(OpenOnlyTransition());
        opened = true;
    }

    public void OpenWithCondition(Func<bool> cond)
    {

    }

    public void CloseOnly()
    {
        StartCoroutine(CloseOnlyTransition());
        opened = false;
    }

    public void SetSpeed(float Speed)
    {
        speed = Speed;
        initialSpeed = Speed;
    }

    public void SetPositions(Vector3 startPOS, Vector3 finalPOS)
    {
        startPos = startPOS;
        finalPos = finalPOS;
        targetPos = finalPOS;
    }

    public bool IsPlatform()
    {
        return isPlatform;
    }

    public IEnumerator OpenOnlyTransition()
    {
        if (!opening)
        {
            opening = true;
            _as.Play();
            
            SetEmissiveColor(Color.green);
            while (transform.localPosition != finalPos)
            {
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPos, speed * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
            opening = false;
            targetPos = startPos;
            RemoveFromList();
        }

    }

    public IEnumerator CloseOnlyTransition()
    {
        
        if (!opening)
        {
            AddToList();
            opening = true;
            if(passBlocker)
                passBlocker.gameObject.SetActive(true);
            _as.Play();
            SetEmissiveColor(Color.red);
            while (transform.localPosition != startPos)
            {
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPos, speed * 2* Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
            targetPos = finalPos;
            opening = false;
            if(passBlocker)
            passBlocker.gameObject.SetActive(false);
        }

    }

    public IEnumerator Transition()
    {
        if(!opening)
        {
            _as.Play();
            opening = true;
            SetEmissiveColor(Color.green);
            while (transform.localPosition != finalPos)
            {
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPos, speed * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
            targetPos = startPos;

            _as.Play();

            SetEmissiveColor(Color.red);
            while (transform.localPosition != startPos)
            {
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPos, speed * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
            targetPos = finalPos;
            opening = false;
        }
    }

    public void SetEmissiveColor(Color color)
    {
        foreach (var item in ToChangeEmissiveMaterial)
        {
            item.GetComponent<Renderer>().material.color = color;
            Material mymat = item.GetComponent<Renderer>().material;
            mymat.SetColor("_EmissionColor", color);
            currentColor = color;
        }
    }

    public Vector3 GetPosition()
    {
        return transform.position;
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
        if (Physics.Raycast(initialPos, (hero.transform.position - initialPos).normalized, out hit2, 1000f, Config.me.heroAndWallsMask))
        {
            //Debug.Log(hit.transform);
        }

        return target != null && hero.GetCC().isGrounded && hit.transform.IsChildOf(transform) && hit2.transform == hero.transform && Vector3.Distance(hero.transform.position, transform.position) < 10f;
    }

    public void SwitchOutline(bool state, CursorTextureType type)
    {
        if (state) outline.OutlineWidth = 5f;
        else outline.OutlineWidth = 0f;
        CursorManager.me.SetCursorColor(state, type);
    }

    public void Interact(Skill interaction)
    {

    }

    public void AddToList()
    {
        GameManager.me.im.AddInteractionObject(this, Skill.SlowTime);
    }

    public void RemoveFromList()
    {
        GameManager.me.im.RemoveInteractionObject(this, Skill.SlowTime);
    }

    public void Selected()
    {
        
    }

    public void Unselected()
    {
        
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public void SaveForCheckpoint()
    {
        if(opening)
            checkpointState.position = targetPos;
        else
            checkpointState.position = transform.localPosition;
        checkpointState.opening = opening;
        checkpointState.targetPos = targetPos;
        checkpointState.currentColor = currentColor;
    }

    public void LoadForCheckpoint()
    {
        //transform.localPosition = checkpointState.position;
        //opening = checkpointState.opening;
        //targetPos = checkpointState.targetPos;
        //SetEmissiveColor(checkpointState.currentColor);
    }

    public void AddToCheckpointList()
    {
        GameManager.me.lisfOfCheckpointObjects.Add(this);
    }

    public void RemoveFromCheckpointList()
    {
        GameManager.me.lisfOfCheckpointObjects.Remove(this);
    }

    public void ReceiveInfo(int info)
    {

    }
}
