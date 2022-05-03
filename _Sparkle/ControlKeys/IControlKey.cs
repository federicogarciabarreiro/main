using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IControlKey {

    KeyCode key { get; set; }
    string text { get; set; }
}
