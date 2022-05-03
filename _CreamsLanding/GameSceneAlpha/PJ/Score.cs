using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public float counter;
    public Text textCounter;
    public GameObject ScoreTab;

     void Start()
    {
        counter = 0;
    }

     void Update()
    {
        textCounter.text = "" + counter;

        if (Time.timeScale == 0)
        { if (counter >= ScoreTab.GetComponent<ScoreTab>().record)
        { ScoreTab.GetComponent<ScoreTab>().record = counter; }
        }
    }
    void OnTriggerEnter2D(Collider2D c)
    {
        if (c.gameObject.tag == "BlueItem")
        {
            counter = counter + 10f;
        }

        if (c.gameObject.tag == "RedItem")
        {
            counter = counter + 5f;
        }
    }

    void OnCollisionEnter2D(Collision2D c)
    {
    if (c.gameObject.tag == "Blocks")
            {
                counter = counter + 1f;
                       }
        
    }

}