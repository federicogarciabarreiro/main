using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRewindable
{
    IEnumerator SaveState();
    IEnumerator LoadState();
    void Rewind();
}
