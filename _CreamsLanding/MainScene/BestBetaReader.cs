using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BestBetaReader : MonoBehaviour {

    public GameObject readRecord;
    public Text betaReader;
    
	void Start () {
        if (readRecord.GetComponent<ScoreTab>().record == 0)

            
        { betaReader.text = ""; }
        else
        {
            betaReader.text = "Best! -> " + readRecord.GetComponent<ScoreTab>().record;
        }
    }
	
}
