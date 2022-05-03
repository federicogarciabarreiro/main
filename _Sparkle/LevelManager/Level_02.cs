using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class Level_02 : MonoBehaviour, ILevel
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
        GameManager.me.SetLevel(2);
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
        yield return new WaitForSeconds(1f);
    }

    IEnumerator ExitLevel()
    {
        Camera.main.GetComponent<MainCamera>().SetCameraLayer(CameraLayer.OnlyHero);
        Camera.main.GetComponent<MainCamera>().SetCameraMode(CameraMode.Portal);
        hero.GetAnim().SetTrigger("Floating");
        hero.enabled = false;
        yield return new WaitForSeconds(6f);
        HUDManager.me.BlackoutScreen(HUDManager.me.WhiteBG, 0.01f);
        yield return new WaitForSeconds(1.5f);
        GameManager.me.Clear();
        SceneManager.LoadScene(4);
    }
}