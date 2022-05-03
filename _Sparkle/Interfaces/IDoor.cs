using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDoor{

    void Open();
    void OpenOnly();
    void CloseOnly();
    void ReceiveInfo(int info);
}
