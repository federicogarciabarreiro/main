using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brain : MonoBehaviour {

    public Hero hero;

    void Update()
    {
        //1stSkill
        if (Input.GetKeyDown(Config.me.controlKeys[(int)ControlKey.First].key))
            hero.Interact(hero.skillTypes[(int)Skill.Lift]);

        //2ndSkill
        if (Input.GetKeyDown(Config.me.controlKeys[(int)ControlKey.Second].key))
            hero.Interact(hero.skillTypes[(int)Skill.SlowTime]);

        //3rdSkill
        if (Input.GetKeyDown(KeyCode.Q))
            hero.Interact(hero.skillTypes[(int)Skill.EnergyShield]);

        //4thSkill
        if (Input.GetKeyDown(KeyCode.F))
            hero.Interact(hero.skillTypes[(int)Skill.MindControl]);

        //Jump
        if (Input.GetKeyDown(Config.me.controlKeys[(int)ControlKey.Jump].key))
            hero.Jump();


        //Boost (start)
        //if (Input.GetKey(Config.me.controlKeys[(int)ControlKey.Boost].key))
            hero.Boost(1.5f, true);

        ////Boost (end)
        //if (Input.GetKeyUp(Config.me.controlKeys[(int)ControlKey.Boost].key))
        //    hero.Boost(1, false);
    }
}
