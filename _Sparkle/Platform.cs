using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Platform : MonoBehaviour, IDoor, ILoadable {

    public List<Vector3> finalPos;
    int posIndex;
    int savedPosIndex;
    bool savedAutomaticClousure;
    public float speed;
    bool opening;
    public bool automaticClosure;
    AudioSource _as;
    public ConditionToOpen condition;
    bool conditionDone;
    List<Func<bool>> listOfConditionsToOpen = new List<Func<bool>>();
    List<Func<bool>> listOfConditionsToExit = new List<Func<bool>>();
    public enum ConditionToOpen
    {
        None,
        Level03_WaitPlatformToReachPoint01,
        Level03_WaitPlatformToReachPoint02,
        Level03_WaitPlatformToReachPoint03
    }

    public enum ConditionToClose
    {
        None,
        Level03_WaitPlatformToReachPoint01,
        Level03_WaitPlatformToReachPoint02,
        Level03_WaitPlatformToReachPoint03,
        Level03_BoxDropsFromPlatform
    }

    public enum ActionsToExecute
    {
        None,
        AutomaticClosureFalse,
        AutomaticClosureTrue
    }

    private void Start()
    {
        _as = GetComponent<AudioSource>();
        finalPos[0] = transform.localPosition;
        automaticClosure = true;
        AddToCheckpointList();
        listOfConditionsToOpen.Add(() => true);
        listOfConditionsToOpen.Add(() => Config.me.platformMovable.localPosition == new Vector3(-12f, -1.6f, 192f));
        listOfConditionsToOpen.Add(() => Config.me.platformMovable.localPosition == new Vector3(-12f, -1.6f, 204f));
        listOfConditionsToOpen.Add(() => Config.me.platformMovable.localPosition == new Vector3(-12f, -1.6f, 222f));


    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
            OpenOnly();
        if (Input.GetKeyDown(KeyCode.J))
            CloseOnly();
    }

    void NoAction()
    {

    }

    void AutomaticClosureFalse()
    {
        SetAutomaticClosure(false);
    }

    void AutomaticClosureTrue()
    {
        SetAutomaticClosure(true);
    }

    public void Open()
    {
        
    }

    public void OpenOnly()
    {
        StartCoroutine(OpenOnlyTransition());

    }

    public void CloseOnly()
    {
        StartCoroutine(CloseOnlyTransition());
    }



    public IEnumerator OpenOnlyTransition()
    {
        
        if (!opening)
        {
            opening = true;
            if(!conditionDone)
            {
                yield return new WaitUntil(listOfConditionsToOpen[(int)condition]);
                conditionDone = true;
            }
            _as.Play();

            print("llegue hasta el condicional");
            while (transform.localPosition != finalPos[posIndex])
            {
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, finalPos[posIndex], speed * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
            print("llegue hasta el final");
            opening = false;
        }

    }

    public IEnumerator CloseOnlyTransition()
    {
        if (!opening)
        {
            opening = true;
            _as.Play();
            while (transform.localPosition != finalPos[0])
            {
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, finalPos[0], speed * 5 * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
            opening = false;
        }

    }

    public void ReceiveInfo(int info)
    {
        posIndex = info;
    }

    public int GetInfo()
    {
        return posIndex;
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponentInParent<CubeScifiLiftable>())
        {
            other.transform.parent.transform.parent = transform;
        }
        if(other.GetComponent<Hero>())
        {
            other.transform.parent = transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponentInParent<CubeScifiLiftable>())
        {
            other.transform.parent.transform.parent = null;
            if(automaticClosure)
                StartCoroutine(GoBackToStart(1f));
        }
        if (other.GetComponent<Hero>())
        {
            other.transform.parent = null;
        }
    }

    IEnumerator GoBackToStart(float time)
    {
        yield return new WaitForSeconds(time);
        StopAllCoroutines();
        opening = false;
        CloseOnly();
    }

    public void SetAutomaticClosure(bool state)
    {
        automaticClosure = state;
    }

    public void SaveForCheckpoint()
    {
        savedPosIndex = posIndex;
        savedAutomaticClousure = automaticClosure;
    }

    public void LoadForCheckpoint()
    {
        posIndex = savedPosIndex;
        automaticClosure = savedAutomaticClousure;
        transform.localPosition = finalPos[posIndex];
        conditionDone = false;

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
