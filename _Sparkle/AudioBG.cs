using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioBG : MonoBehaviour {

    public AudioSource bgMusic;
    public AudioSource alarm;
    float timer;
    //int dir = 1;

    private void Update()
    {
        timer -= Time.deltaTime / 20;
        //if (timer > 1f || timer < 0f)
        //    dir *= -1;

        alarm.volume = 0.5f * timer;
    }

    public void SetAlarmTimer(float amount)
    {
        timer = amount;
    }

}
