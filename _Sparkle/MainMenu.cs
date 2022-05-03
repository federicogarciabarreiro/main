using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	public void LoadScene(int index)
    {
        GameManager.me.SetLevel(index-1);
        SceneManager.LoadScene(index);
    }

    public void Play()
    {

    }

    public void ChapterSelection()
    {

    }

    public void Credits()
    {

    }

    public void Exit()
    {
       
    }
}
