using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    public GameObject instruction;
    public GameObject playerSelect;
    public GameObject soundButton;

    void Start()
    {
        Time.timeScale = 1;
    }

    public void StartGame(string param)
    {
        if (param == "GameSceneAlpha")
        {
            Application.LoadLevel("GameSceneAlpha");
            soundButton.GetComponent<AudioSource>().Play();
        }

        if (param == "GameSceneBeta")
        {
            Application.LoadLevel("GameSceneBeta");
            soundButton.GetComponent<AudioSource>().Play();
        }

        if (param == "Exit")
        {
            soundButton.GetComponent<AudioSource>().Play();
            Application.Quit();
        }

        if (param == "Instruction")
        {
            instruction.SetActive(true);
            soundButton.GetComponent<AudioSource>().Play();
        }

        if (param == "InstructionClose")
        {
            instruction.SetActive(false);
            soundButton.GetComponent<AudioSource>().Play();
        }
 
        if (param == "MainSceneAlpha")
        {
            Application.LoadLevel("MainSceneAlpha");
            soundButton.GetComponent<AudioSource>().Play();
        }

        if (param == "MainSceneBeta")
        {
            Application.LoadLevel("MainSceneBeta");
            soundButton.GetComponent<AudioSource>().Play();
        }

        if (param == "PlayerS")
        {
            playerSelect.SetActive(true);
            soundButton.GetComponent<AudioSource>().Play();
        }

    }
}
