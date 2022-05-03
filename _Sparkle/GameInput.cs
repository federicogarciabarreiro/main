using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameInput : MonoBehaviour
{
    Hero hero;

    private void Start()
    {
        hero = Config.me.hero.GetComponent<Hero>();
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.F1))
            Config.me.hero.GetComponent<Hero>().Die(Death.FrontHit);
        if (Input.GetKeyDown(KeyCode.Escape))
            HUDManager.me.BlackoutScreen(HUDManager.me.BlackBG, 2f);

        if (Input.GetKeyDown(KeyCode.Escape))
            SceneManager.LoadScene(0);

        //if (Input.GetKeyDown(KeyCode.F9))
        //{
        //    hero.transform.position = new Vector3(2.1f, 0f, -426f);
        //    Camera.main.GetComponent<MainCamera>().SetCameraMode(CameraMode.Tutorial);
        //    Camera.main.transform.position = hero.transform.position;
        //    GameManager.me.SetLevel(0);
        //}
        //if (Input.GetKeyDown(KeyCode.F10))
        //{
        //    hero.transform.position = new Vector3(5.1f, 0f, -310f);
        //    Camera.main.GetComponent<MainCamera>().SetCameraMode(CameraMode.Level01);
        //    Camera.main.transform.position = hero.transform.position;
        //    GameManager.me.SetLevel(1);
        //}
        //if (Input.GetKeyDown(KeyCode.F11))
        //{
        //    hero.transform.position = new Vector3(7.1f, 0f, 21f);
        //    Camera.main.GetComponent<MainCamera>().SetCameraMode(CameraMode.Level02_A);
        //    Camera.main.transform.position = hero.transform.position;
        //    GameManager.me.SetLevel(2);
        //}
        //if (Input.GetKeyDown(KeyCode.F12))
        //{
        //    hero.transform.position = new Vector3(4.1f, 0f, 126f);
        //    Camera.main.GetComponent<MainCamera>().SetCameraMode(CameraMode.Level02_C);
        //    Camera.main.transform.position = hero.transform.position;
        //    GameManager.me.SetLevel(2);
        //}

        //if (Input.GetKeyDown(KeyCode.F8))
        //{
        //    hero.transform.position = new Vector3(1.2f, -0.6f, 189f);
        //    Camera.main.GetComponent<MainCamera>().SetCameraMode(CameraMode.Level03);
        //    Camera.main.transform.position = hero.transform.position;
        //    GameManager.me.SetLevel(3);
        //}
    }
}