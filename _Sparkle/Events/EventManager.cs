using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameEvent
{
    None,
    WalkKeys,
    TelekinesisPowerUp,
    LiftAndThrowObject,
    SlowDownObject,
    FromTutorialToLaboratory,
    FromLaboratoryToSandBox,
    LearnSlowDownEnemy,
    TurnOnLaser,
    LearnShieldOnObject,
    BridgeGoesUp,
    EventPortalEnterCinematic,
    LiftPlatforms,
    LearnMindControl
}

public enum ActionElapsed
{
    FirstTimeSlowDownObject,
    FirstTimeLiftedObject,
    FirstTimeThrownedObject,
    FirstTimeSlowDownEnemy,
    FirstTimeShieldOnObject,
    FirstTimeShieldOnSelf,
    FirstTimeMindControl,
    Count
}

public class EventManager {

    List<IEvent> listOfEvents = new List<IEvent>();
    public IEvent currentEvent;
    List<bool> listOfActionsElapsed = new List<bool>();
    
    public EventManager()
    {
        listOfEvents.Add(new EventNone());
        listOfEvents.Add(new EventWalkKeys());
        listOfEvents.Add(new EventGrabTelekinesisPower());
        listOfEvents.Add(new EventLiftAndThrowObject());
        listOfEvents.Add(new EventSlowDownObject());
        listOfEvents.Add(new EventFromTutorialToLaboratory());
        listOfEvents.Add(new EventFromLaboratoryToSandBox());
        listOfEvents.Add(new EventLearnSlowDownEnemy());
        listOfEvents.Add(new EventTurnLaserOn());
        listOfEvents.Add(new EventLearnObjectOnShield());
        listOfEvents.Add(new EventBridgeGoesUp());
        listOfEvents.Add(new EventPortalEnterCinematic());
        listOfEvents.Add(new EventLiftPlatforms());
        listOfEvents.Add(new EventLearnMindControl());

        SetEvent(GameEvent.None);

        for (int i = 0; i < (int)ActionElapsed.Count; i++)
        {
            listOfActionsElapsed.Add(false);
        }
        
    }

    public void SetEvent(GameEvent ev)
    {
        if (currentEvent != null)
            currentEvent.Exit();
        currentEvent = listOfEvents[(int)ev];
        currentEvent.Enter();
        GameManager.me.currentEvent = ev;
        Debug.Log(ev);
    }

    public void SetActionElapsed(ActionElapsed action)
    {
        listOfActionsElapsed[(int)action] = true;
    }

    public bool GetActionElapsed(ActionElapsed action)
    {
        return listOfActionsElapsed[(int)action];
    }
}
