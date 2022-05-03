using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public enum Speaker
{
    Hero,
    Info,
    ChronoPowerUp,
    TelekinesisPowerUp,
    Saved,
    Telekinesis,
    EnergyShield
}

public enum ConditionToExit
{
    WaitSeconds,
    WaitForKey
}

public class DialogManager : MonoBehaviour{

    List<Dialog> dialogs = new List<Dialog>();
    //List<Func<bool>> listOfConditions = new List<Func<bool>>();
    public List<Sprite> iconSprites;
    public GameObject dialogBubblePrefab;
    public Transform canvas;

    private void Start()
    {
        DontDestroyOnLoad(this);
    }

    public void Update()
    {
        if (dialogs.Count > 0)
        {
            for (int i = 0; i < dialogs.Count; i++)
            {
                var bubble = dialogs[(dialogs.Count-1)-i].bubble;
                bubble.GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(bubble.GetComponent<RectTransform>().anchoredPosition, new Vector2(bubble.GetComponent<RectTransform>().anchoredPosition.x, 50 * (i+1)), 0.02f);
            }
        }
    }

    public void AddDialog(string text, Speaker speaker, Func<bool> condition, float exitTime)
    {
        Dialog newDialog = new Dialog(text, iconSprites[(int)speaker], condition, exitTime);
        CreateDialogBubble(newDialog);
        dialogs.Add(newDialog);
    }

    public void RemoveDialog(Dialog dialog)
    {
        dialogs.Remove(dialog);
    }

    public void CreateDialogBubble(Dialog dialog)
    {
        var obj = GameObject.Instantiate(dialogBubblePrefab);
        obj.transform.parent = canvas;
        obj.GetComponent<RectTransform>().anchoredPosition = new Vector2(50, 0);
        obj.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
        obj.GetComponent<Image>().sprite = dialog.icon;
        obj.transform.Find("Button").Find("Text").GetComponent<Text>().text = dialog.text;
        obj.transform.Find("Button").GetComponent<RectTransform>().sizeDelta = new Vector2(35 + RemoveColorString(dialog.text) * 7.75f, 35);
        obj.GetComponent<DialogBubble>().maxTime = dialog.exitTime;
        dialog.bubble = obj.transform;
        StartCoroutine(DeleteDialog(dialog, obj));
    }

    public IEnumerator DeleteDialog(Dialog dialog, GameObject obj)
    {
        yield return new WaitForSeconds(dialog.exitTime);
        yield return new WaitUntil(dialog.cond);
        obj.GetComponent<DialogBubble>().canDissapear = true;
        RemoveDialog(dialog);
        yield return new WaitForSeconds(3f);
        Destroy(dialog.bubble.gameObject);
    }

    int RemoveColorString(string text)
    {
        var restultingTexts = text.Split('<', '>');
        var newText = "";
        foreach (var item in restultingTexts)
        {
            if (!item.Contains("color=") && !item.Contains("/color"))
                newText += item;
        }
        return newText.Length;
    }

    public void Clear()
    {
        dialogs.Clear();
        StopAllCoroutines();
    }

}
