using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    public Transform personaje; 
    public GameObject gameOver; 
    public GameObject pauseMenu; 
    public Text totalCounter; 
    public GameObject counterPoint; 
    public Transform parallaxLimit;
    public float xToRestartParallax;
    
    void Start()
    {
        Time.timeScale = 1;
    }

    void Update()
    {
        xToRestartParallax = parallaxLimit.transform.position.x;
        transform.position = new Vector3(personaje.position.x, transform.position.y, transform.position.z);
        if (Time.timeScale == 0) 
        {
            gameOver.SetActive(true);
            Camera.main.GetComponent<AudioSource>().Stop();
            totalCounter.text = "" + counterPoint.GetComponent<Score>().counter;
        }


        if (Time.timeScale == 1)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                pauseMenu.SetActive(true);
                Time.timeScale = 0.0000001f;
            }
        }

                if(Time.timeScale == 0.0000001f) 
        {
            if (Input.GetKeyDown(KeyCode.F1))
            {
                pauseMenu.SetActive(false);
                Time.timeScale = 1;
            }
        }

            
        }

       

    }

