using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreTab : MonoBehaviour
{
    public float record;

    public static ScoreTab scoreTab;

    void Awake()
    {
        record = 0;

        if (scoreTab == null)
        {
            scoreTab = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (scoreTab != this)
        {
            Destroy(gameObject);
        }
    }

}
