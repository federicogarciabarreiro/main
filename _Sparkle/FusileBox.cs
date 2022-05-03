using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FusileType
{
    Yellow,
    Green,
    Red,
    Blue,
    Magenta,
    Pink,
    None
}

public class FusileBox : MonoBehaviour, ILoadable {

    class CheckpointState
    {
        public FusileType currentFusile;
    }
    CheckpointState checkpointState = new CheckpointState();

    public FusileType FusileAccepted;
    public FusileType currentFusile;
    public FusileChecker checker;

    private void Start()
    {
        AddToCheckpointList();
        currentFusile = FusileType.None;
    }

    public void SetFusile(FusileType fusile, bool turn)
    {
        currentFusile = fusile;
        if(fusile == FusileAccepted)
        {
            if (turn)
                checker.AddFusile();
            else
                checker.RemoveFusile();
        }

        if(!turn)
        {
            currentFusile = FusileType.None;
        }
            
    }

    public FusileType GetFusile()
    {
        return currentFusile;
    }

    public void SaveForCheckpoint()
    {
        checkpointState.currentFusile = currentFusile;
    }

    public void LoadForCheckpoint()
    {
        currentFusile = checkpointState.currentFusile;
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
