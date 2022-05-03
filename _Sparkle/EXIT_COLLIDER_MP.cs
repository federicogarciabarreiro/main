using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EXIT_COLLIDER_MP : MonoBehaviour
{
    public Outline door;
    public Transform quitGame;

    private void OnMouseDown()
    {
        print("DOWN");
        Application.Quit();

    }

    private void OnMouseEnter()
    {
        print("ENTER");
        door.OutlineWidth = 6f;
        quitGame.gameObject.SetActive(true);
    }
    private void OnMouseExit()
    {
        print("EXIT");
        door.OutlineWidth = 0f;
        quitGame.gameObject.SetActive(false);
    }
}

