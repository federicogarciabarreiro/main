using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Dialog {

    public string text;
    public Transform bubble;
    public Sprite icon;
    public Func<bool> cond;
    public float exitTime;

    public Dialog(string Text, Sprite Icon, Func<bool> Cond, float ExitTime)
    {
        text = Text;
        icon = Icon;
        cond = Cond;
        exitTime = ExitTime;
    }

}
