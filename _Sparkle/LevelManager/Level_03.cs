using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class Level_03 : MonoBehaviour, ILevel
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
        GameManager.me.SetLevel(3);
        enterEnemiesArea = () => Physics.OverlapSphere(Config.me.hero.transform.position, 10f, Config.me.enemyLayer).Any();

    }

    public void Enter()
    {
        StartCoroutine(Initialization());
    }

    public void Action()
    {
        GameManager.me.im.OutlineObject();
        if (Input.GetKeyDown(KeyCode.F8))
            hero.transform.position = new Vector3(1.7f, -0.22f, 234.6f);
        if (Input.GetKeyDown(KeyCode.F9))
            hero.transform.position = new Vector3(-16.38f, -0.22f, 183.27f);
    }
    public void Exit()
    {
        StartCoroutine(ExitLevel());
    }

    IEnumerator Initialization()
    {
        yield return new WaitForSeconds(0.01f);
        hero.gameObject.SetActive(false);
        yield return new WaitForSeconds(2f);
        hero.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.01f);
        hero.transform.position = Config.me.portallvl02OutStartPos.position;
        hero.transform.forward = Config.me.portallvl02OutStartPos.forward;
        hero.enabled = true;
        hero.Freeze(true);

        hero.ApplyForce(hero.transform.forward * 13f);
        hero.RemoveForce(5f);
        hero.GetAnim().SetTrigger("HardLanding");
        yield return new WaitForSeconds(2.5f);
        Config.me.crystalPortalOutlvl2.gameObject.SetActive(false);
        hero.Freeze(false);
        yield return new WaitForSeconds(1f);
    }

    IEnumerator ExitLevel()
    {
        yield return new WaitForSeconds(1f);
    }
}