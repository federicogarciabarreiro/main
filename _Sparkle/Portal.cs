using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour {

    public Transform crystal;
    int amountOfKeys;
    public bool endPortal;

    public void SetAmountOfKeys(int amount)
    {
        amountOfKeys += amount;
        if (amountOfKeys == 2)
        {
            TurnOnCrystal();
            if(endPortal)
            {
                StartCoroutine(endGame());
            }
        }

    }

    public void TurnOnCrystal()
    {
        crystal.gameObject.SetActive(true);
        GetComponent<AudioSource>().Play();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.GetComponent<Hero>())
        {
            GameManager.me.ExitCurrentLevel();
        }
    }

    public IEnumerator endGame()
    {
        yield return new WaitForSeconds(2f);
        HUDManager.me.BlackoutScreen(HUDManager.me.BlackBG, 0.05f);
        yield return new WaitForSeconds(7.5f);
        HUDManager.me.theEndText.gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);
        HUDManager.me.thanksForPlaying.gameObject.SetActive(true);

    }
}
