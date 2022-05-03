using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.FinalIK;

public enum DoorTriggerFunction
{
    OpenAndClose,
    Open,
    Close
}

public class DoorOpener : MonoBehaviour, IInteractable, ILoadable {

    public Transform door;
    public int mainInfoToSend;
    public List<Transform> listOfDoors;
    Outline outline;
    Transform button;
    public GameEvent eventToTrigger;

    public int infoToSend;
    bool eventTriggered;
    public DoorTriggerFunction function;
    public bool opening;
    Hero hero;


    private void Awake()
    {
        outline = transform.GetComponentInParent<Outline>();
    }

    private void Start()
    {
        AddToList();
        button = transform.Find("Graphic");
        hero = Config.me.hero.GetComponent<Hero>();
        AddToCheckpointList();
    }
    public void AddToList()
    {
        GameManager.me.im.AddInteractionObject(this, Skill.Lift);
    }

    public void RemoveFromList()
    {
        GameManager.me.im.RemoveInteractionObject(this, Skill.Lift);
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
        print("DOOR INTERACTIONNN");
        if(!opening)
            StartCoroutine(InteractWithDoor(hero));
    }

    public void OpenDoor()
    {
        door.GetComponent<IDoor>().OpenOnly();
        button.GetComponent<Renderer>().material.color = Color.green;
    }

    public void CloseDoor()
    {
        door.GetComponent<IDoor>().CloseOnly();
        button.GetComponent<Renderer>().material.color = Color.red;
    }


    IEnumerator InteractWithDoor(Hero hero)
    {
        opening = true;
        if (hero.GetCC().isGrounded && hero.GetIS().GetClosestTriggerIndex() != -1)
        {
            
            hero.SetMovementMode(MovementMode.None);
            //yield return new WaitForSeconds(0.25f);
            Vector3 forward = (transform.position - hero.transform.position).normalized;
            forward.y = hero.transform.forward.y;
            hero.transform.forward = forward;
            hero.GetAnim().SetFloat("SpeedX", 1f);

            while (Vector3.Distance(hero.transform.position, transform.position) > 1f)
            {
                hero.transform.position = Vector3.MoveTowards(hero.transform.position, new Vector3(transform.position.x, hero.transform.position.y, transform.position.z), 0.05f);
                yield return new WaitForSeconds(0.01f);
            }
            hero.GetIS().TriggerInteraction(hero.GetIS().GetClosestTriggerIndex(), false);
            hero.SetIO(hero.GetIS().GetInteractionObject(FullBodyBipedEffector.RightHand));
            hero.GetAnim().SetFloat("SpeedX", 0f);
            //yield return new WaitForSeconds(0.25f);

        }

        if (!eventTriggered)
        {
            GameManager.me.em.SetEvent(eventToTrigger);
            eventTriggered = true;
        }



        button.GetComponent<Renderer>().material.color = Color.green;
        //yield return new WaitForSeconds(0.25f);
        if(door)
        {
            print(mainInfoToSend);
            door.GetComponent<IDoor>().ReceiveInfo(mainInfoToSend);
            if (function == DoorTriggerFunction.OpenAndClose)
                door.GetComponent<IDoor>().Open();
            if (function == DoorTriggerFunction.Open)
                door.GetComponent<IDoor>().OpenOnly();
            if (function == DoorTriggerFunction.Close)
                door.GetComponent<IDoor>().CloseOnly();
        }
        foreach (var item in listOfDoors)
        {
            item.GetComponent<IDoor>().ReceiveInfo(infoToSend);
            if (function == DoorTriggerFunction.OpenAndClose)
                item.GetComponent<IDoor>().Open();
            if (function == DoorTriggerFunction.Open)
                item.GetComponent<IDoor>().OpenOnly();
            if (function == DoorTriggerFunction.Close)
                item.GetComponent<IDoor>().CloseOnly();
        }
        yield return new WaitForSeconds(0.25f);
        hero.SetMovementMode(MovementMode.Normal);
        opening = false;
        yield return new WaitForSeconds(2.5f);
        button.GetComponent<Renderer>().material.color = Color.red;

    }



    public void SwitchOutline(bool state, CursorTextureType type)
    {
        if(outline)
        {
            if (state) outline.OutlineWidth = 5f;
            else outline.OutlineWidth = 0f;
        }
    }

    public void SaveForCheckpoint()
    {

    }

    public void LoadForCheckpoint()
    {
        eventTriggered = false;
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
