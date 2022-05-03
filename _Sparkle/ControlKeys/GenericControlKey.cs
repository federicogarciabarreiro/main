using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericControlKey : IControlKey
{
    public KeyCode key { get; set; }

    public string text { get; set; }

    public GenericControlKey(KeyCode Key, string Text)
    {
        key = Key;
        text = Text;
    }
}