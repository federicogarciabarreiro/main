using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour {

    public static HUDManager me;
    public Transform telekinesisScrollWheel;
    public Image BlackBG;
    public Image WhiteBG;
    Hero hero;
    public List<Sprite> listOfSkillIcons;
    public Transform theEndText;
    public Transform thanksForPlaying;
    


    void Awake()
    {
        me = this;
    }
    private void Start()
    {
        hero = Config.me.hero.GetComponent<Hero>();
    }

    public void TurnTelekenisScroll(bool state)
    {
        telekinesisScrollWheel.gameObject.SetActive(state);
    }

    public void BlackoutScreen(Image bg, float speed)
    {
        StopAllCoroutines();
        StartCoroutine(BlackOutScreen(bg, speed));
    }

    public void LightupScreen(Image bg, float speed)
    {
        StopAllCoroutines();
        StartCoroutine(LightUpScreen(bg, speed));
    }

    public IEnumerator BlackOutScreen(Image bg, float speed)
    {
        var tempColor = bg.color;
        while (tempColor.a < 1)
        {
            tempColor.a += 0.01f;
            bg.color = tempColor;
            yield return new WaitForSeconds(speed);
        }
    }

    public IEnumerator LightUpScreen(Image bg, float speed)
    {
        var tempColor = bg.color;
        while (tempColor.a > 0)
        {
            tempColor.a -= 0.01f;
            bg.color = tempColor;
            yield return new WaitForSeconds(speed);
        }
    }

    //public void SetSkillIcon(MouseSkill cursorSkill, Skill skill)
    //{
    //    if(cursorSkill == MouseSkill.Primary)
    //    {
    //        primarySkillIcon.sprite = listOfSkillIcons[(int)skill];
    //    }

    //    if (cursorSkill == MouseSkill.Secondary)
    //    {
    //        secondarySkillIcon.sprite = listOfSkillIcons[(int)skill];
    //    }
    //}
}
