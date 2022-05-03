using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventGrabTelekinesisPower : IEvent
{
    BrokenDoor doorToOpen;
    BrokenDoor doorToClose;

    Hero hero;
    public void Enter()
    {
        doorToOpen = Config.me.doorToOpenFrom01to02.GetComponent<BrokenDoor>();
        doorToClose = Config.me.doorToCloseFrom01to02.GetComponent<BrokenDoor>();
        string text = string.Format("New ability acquired: <color=yellow>{0}</color>", "ENERGY SHIELD");
        GameManager.me.dm.AddDialog(text, Speaker.EnergyShield, () => true, 8);
        doorToClose.CloseOnly();
        doorToOpen.OpenOnly();
    }
    public void Action()
    {

    }


    public void Exit()
    {

    }

}