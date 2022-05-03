using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FusileChecker : MonoBehaviour, ILoadable {

    class CheckpointState
    {
        public int currentFusiles;
    }
    CheckpointState checkpointState = new CheckpointState();

    public int totalFusiles;
    int currentFusiles;
    IDoor door;

    private void Start()
    {
        AddToCheckpointList();
        door = GetComponent<IDoor>();
    }

    public void AddFusile()
    {
        currentFusiles++;
        Action();
    }

    public void RemoveFusile()
    {
        currentFusiles--;
        Action();
    }

    public void Action()
    {
        if (currentFusiles == totalFusiles)
            door.OpenOnly();
        else
            door.CloseOnly();
    }

    public void SaveForCheckpoint()
    {
        checkpointState.currentFusiles = currentFusiles;
    }

    public void LoadForCheckpoint()
    {
        currentFusiles = checkpointState.currentFusiles;
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
