using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class Level_Tutorial : MonoBehaviour, ILevel {

    Hero hero;
    Func<bool> enterEnemiesArea;
    private void Awake()
    {
        GameManager.me.im.Initialize();
        GameManager.me.dm.canvas = GameObject.Find("Canvas").transform;

        hero = Config.me.hero.GetComponent<Hero>();
    }

    void Start()
    {
        GameManager.me.SetLevelManager(this);
        enterEnemiesArea = () => Physics.OverlapSphere(Config.me.hero.transform.position, 10f, Config.me.enemyLayer).Any();

    }

    public void Enter()
    {
        StartCoroutine(Initialization());
    }

    public void Action()
    {
        GameManager.me.im.OutlineObject();
    }
    public void Exit()
    {
        StartCoroutine(ExitLevel());
    }

    IEnumerator Initialization()
    {
        yield return new WaitForSeconds(0.5f);
        GameManager.me.SaveCheckpoint();
        GameManager.me.dm.AddDialog("There's gotta be a way out of here... I must hurry", Speaker.Hero, () => true, 8);
        yield return new WaitForSeconds(0.01f);
        GameManager.me.em.SetEvent(GameEvent.WalkKeys);
        FindObjectOfType<AudioBG>().SetAlarmTimer(1f);
        while (true)
        {
            yield return new WaitForSeconds(10f);
            yield return new WaitUntil(() => Physics.OverlapSphere(Config.me.hero.transform.position, 10f, Config.me.enemyLayer).Any());
            FindObjectOfType<AudioBG>().SetAlarmTimer(1f);
        }

    }

    IEnumerator ExitLevel()
    {
        string text = string.Format("Loading...");
        GameManager.me.dm.AddDialog(text, Speaker.Saved, () => true, 2);
        yield return new WaitForSeconds(0.5f);
        HUDManager.me.BlackoutScreen(HUDManager.me.BlackBG, 0.02f);
        hero.Freeze(true);
        yield return new WaitForSeconds(4f);
        GameManager.me.Clear();
        SceneManager.LoadScene(2);
    }
}
