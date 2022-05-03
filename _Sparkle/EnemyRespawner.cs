using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRespawner : MonoBehaviour {

    public GameObject enemyToRespawn;
    public GameObject enemyPrefab;
    public Transform startPos;
    public Transform posToWalk;
    public Door doorToOpen;

	void Start () {
        StartCoroutine(RespawnEnemy());
	}

    IEnumerator RespawnEnemy()
    {
        yield return new WaitUntil(() => enemyToRespawn.transform.position.y < -100);
        if (Config.me.hero.GetComponent<Hero>().listOfSkills[(int)Skill.MindControl].GetInteractiveObject() == enemyToRespawn.GetComponent<Enemy>() as IInteractable)
            (Config.me.hero.GetComponent<Hero>().listOfSkills[(int)Skill.MindControl] as MindControlSkill).Reset();
        //MonoBehaviour.Destroy(enemyToRespawn);
        //enemyToRespawn = Instantiate(enemyPrefab);
        enemyToRespawn.transform.position = startPos.position;
        enemyToRespawn.transform.forward = startPos.forward;

        enemyToRespawn.GetComponent<CharacterController>().enabled = true;
        enemyToRespawn.GetComponent<Enemy>().soldier.gameObject.SetActive(true);
        enemyToRespawn.GetComponent<BoxCollider>().enabled = true;
        enemyToRespawn.GetComponent<Enemy>().GetCurrentBrain().GetSM().SetState(EnemyActionState.Idle);

        yield return new WaitForSeconds(0.25f);
        enemyToRespawn.GetComponent<Enemy>().GetAnim().SetFloat("SpeedX", 10f);

        doorToOpen.GetComponent<IDoor>().Open();
        while(Vector3.Distance(enemyToRespawn.transform.position, posToWalk.transform.position) > 1f)
        {
            enemyToRespawn.transform.position = Vector3.MoveTowards(enemyToRespawn.transform.position, posToWalk.position, 3.5f * Time.deltaTime);
            yield return new WaitForSeconds(0.001f);
        }
        enemyToRespawn.GetComponent<Enemy>().GetAnim().SetFloat("SpeedX", 0f);
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(RespawnEnemy());
    }
}
