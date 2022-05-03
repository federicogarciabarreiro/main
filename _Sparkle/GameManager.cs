using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using System;

public class GameManager : MonoBehaviour
{
    int currentLevel;
    public InteractionsManager im;
    public EventManager em;
    public DialogManager dm;
    public static GameManager me;
    ILevel currentLevelManager;
    public List<ILoadable> lisfOfCheckpointObjects = new List<ILoadable>();
    public GameEvent currentEvent;

    void Awake()
    {
        me = this;
        DontDestroyOnLoad(this);
        im = new InteractionsManager();
        em = new EventManager();
    }

    private void Start()
    {

    }

    void Update ()
	{
        em.currentEvent.Action();
        if(currentLevelManager != null)
            currentLevelManager.Action();
	}

    public void SetLevelManager(ILevel level)
    {
        if(currentLevelManager != null)
            currentLevelManager.Exit();
        currentLevelManager = level;
        currentLevelManager.Enter();
    }

    public void ExitCurrentLevel()
    {
        currentLevelManager.Exit();
    }




    public IEnumerator RestartGame()
    {
        yield return new WaitForSeconds(1f);
        HUDManager.me.BlackoutScreen(HUDManager.me.BlackBG, 0.02f);
        yield return new WaitForSeconds(4f);
        HUDManager.me.LightupScreen(HUDManager.me.BlackBG, 0.01f);
        LoadCheckpoint();
    }

    public void SaveCheckpoint()
    { 
        foreach (var item in lisfOfCheckpointObjects)
            item.SaveForCheckpoint();
    }

    public void LoadCheckpoint()
    {
        GameManager.me.em.SetEvent(GameEvent.None);
        foreach (var item in lisfOfCheckpointObjects)
            item.LoadForCheckpoint();
    }

    public IEnumerator RemoveFromCheckpointList(ILoadable item, float time)
    {
        yield return new WaitForSeconds(time);
        lisfOfCheckpointObjects.Remove(item);

    }

    public void SetLevel(int amount)
    {
        currentLevel = amount;
    }

    public int GetLevel()
    {
        return currentLevel;
    }

    public void Clear()
    {
        dm.Clear();
        lisfOfCheckpointObjects.Clear();
        currentLevelManager = null;
        em.SetEvent(GameEvent.None);
    }
}
