using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILoadable {

    void SaveForCheckpoint();
    void LoadForCheckpoint();
    void AddToCheckpointList();
    void RemoveFromCheckpointList();
}
