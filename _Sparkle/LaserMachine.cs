using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserMachine : MonoBehaviour {

    public LineRenderer lr;
    public LineRenderer lr2;
    Hero hero;
    EnergyShield energyShield;
    Transform laserImpact;
    Transform laserImpact2;
    DoorOpener doorOpener;

    void Start () {
        hero = Config.me.hero.GetComponent<Hero>();
        energyShield = Config.me.energyShield.GetComponent<EnergyShield>();
        laserImpact = transform.Find("LaserImpact");
        laserImpact2 = transform.Find("LaserImpact2");
    }
	

	void Update () {

        lr.SetPosition(0, transform.position);

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 100f, Config.me.objectsHeroShieldWalls))
        {
            lr.SetPosition(1, hit.point);
            laserImpact.position = hit.point - hit.normal * 0.15f;
            laserImpact.GetComponent<ParticleSystem>().Play();
            laserImpact.forward = hit.normal;


            if (hit.transform == hero.transform)
                hero.Die(Death.Laser);
            if (hit.transform == energyShield.transform.GetChild(0))
            {
                lr2.SetPosition(0, hit.point);
                Vector3 pos = hit.transform.position + hit.normal * 50f;
                //Vector3 newPos = new Vector3(pos.x * 10000f, transform.position.y, hero.transform.position.z);
                Vector3 newPos = new Vector3(pos.x, transform.position.y, pos.z);
                lr2.SetPosition(1, newPos);
                if (Physics.Raycast(hit.point, (newPos - hit.point).normalized, out hit, 100f, Config.me.objectsHeroShieldWalls))
                {
                    lr2.SetPosition(1, hit.point);
                    if (hit.transform.GetComponent<BreakableWall>())
                        hit.transform.GetComponent<BreakableWall>().Break();
                    if (hit.transform.GetComponent<Enemy>())
                        hit.transform.GetComponent<Enemy>().Die();
                    if (hit.transform.GetComponent<Hero>())
                    {
                        hit.transform.GetComponent<Hero>().Die(Death.Laser);
                    }
                        
                    if (hit.transform.GetComponent<DoorOpener>())
                    {
                        doorOpener = hit.transform.GetComponent<DoorOpener>();
                        doorOpener.OpenDoor();
                    }

                    laserImpact2.position = hit.point - hit.normal * 0.15f;
                    laserImpact2.GetComponent<ParticleSystem>().Play();
                    laserImpact2.forward = hit.normal;
                }
            }
            else
            {
                if (doorOpener)
                    doorOpener.CloseDoor();
                lr2.SetPosition(0, Vector3.zero);
                lr2.SetPosition(1, Vector3.zero);
                laserImpact2.GetComponent<ParticleSystem>().Stop();
            }
        }
        else
        {
            if (doorOpener)
                doorOpener.CloseDoor();
            laserImpact.GetComponent<ParticleSystem>().Stop();
            if (laserImpact2)
                laserImpact2.GetComponent<ParticleSystem>().Stop();
            lr.SetPosition(1, transform.position + transform.forward * 50f);
            if (lr2)
            {
                lr2.SetPosition(0, Vector3.zero);
                lr2.SetPosition(1, Vector3.zero);
            }
        }




    }
}
