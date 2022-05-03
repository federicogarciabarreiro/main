using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class Level_01 : MonoBehaviour, ILevel
{
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
        GameManager.me.SetLevel(1);
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
        //hero.transform.position = Config.me.labStartPos.position;
        //hero.transform.forward = Config.me.labStartPos.forward;
        //foreach (var item in hero.heroSkills)
        //    item.Exit();
        ////hero.StopHandsInteractions();
        //Camera.main.transform.position = hero.transform.position;
        //Camera.main.GetComponent<MainCamera>().SetCameraMode(CameraMode.Level01);
        //HUDManager.me.LightupScreen(HUDManager.me.BlackBG, 0.01f);
        //hero.Freeze(false);
        //GameManager.me.em.SetEvent(GameEvent.None);
        yield return new WaitForSeconds(1f);

    }

    IEnumerator ExitLevel()
    {
        HUDManager.me.BlackoutScreen(HUDManager.me.BlackBG, 0.02f);
        hero.Freeze(true);
        yield return new WaitForSeconds(4f);
        GameManager.me.Clear();
        SceneManager.LoadScene(3);

    }
}
