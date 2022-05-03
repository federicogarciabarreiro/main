using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogBubble : MonoBehaviour {

    float alphaTimer = 1;
    float timer;
    float iconAlphaAmount;
    float textBoxAlphaAmount;
    float textAlphaAmount;
    public float maxTime;
    public bool canDissapear;


    private void Start()
    {
        iconAlphaAmount = GetComponent<Image>().color.a;
        textBoxAlphaAmount = transform.Find("Button").GetComponent<Image>().color.a;
        textAlphaAmount = transform.Find("Button").Find("Text").GetComponent<Text>().color.a;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if(timer > maxTime && canDissapear)
        {
            GetComponent<RectTransform>().anchoredPosition = Vector2.MoveTowards(GetComponent<RectTransform>().anchoredPosition, new Vector2(-600, GetComponent<RectTransform>().anchoredPosition.y), 300 * Time.deltaTime);
        }
    }
}
