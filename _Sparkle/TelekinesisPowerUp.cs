using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TelekinesisPowerUp : MonoBehaviour, ILoadable {

    public class CheckpointState
    {
        public Vector3 position;
        public Quaternion rotation;
        public bool exists;
        public bool canRespawn;
    }
    public CheckpointState checkpointState = new CheckpointState();

    public AudioClip ac;

    public List<Transform> unitsAffected = new List<Transform>();
    public Transform pipesTop;
    bool alreadyDestroyed;
    public GameObject implosionPrefab;

    private void Start()
    {
        AddToCheckpointList();
    }

    private void Update()
    {
        if(!alreadyDestroyed)
        {
            var objs = Physics.OverlapSphere(transform.position, UnityEngine.Random.Range(1f, 3.65f), Config.me.trasnparentFXMask);
            foreach (var item in objs)
            {
                item.GetComponent<Rigidbody>().isKinematic = true;
                Destroy(item.GetComponent<MeshCollider>());
                item.gameObject.AddComponent<ObjectAffectedByTelekinesis>();
                item.GetComponent<ObjectAffectedByTelekinesis>().pivot = transform;
                item.transform.parent = null;
                unitsAffected.Add(item.transform);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Hero>())
        {
            if(!alreadyDestroyed)
            {
                other.GetComponent<Hero>().AddForce((other.transform.position - transform.position).normalized * Time.deltaTime * 700, "JumpTrigger");
                 alreadyDestroyed = true;
                GameManager.me.em.SetEvent(GameEvent.TelekinesisPowerUp);
                foreach (var item in unitsAffected)
                {
                    item.GetComponent<ObjectAffectedByTelekinesis>().DeactivateEffect();
                    item.GetComponent<Rigidbody>().AddExplosionForce(1000, Config.me.hero.transform.position, 50);
                    Destroy(item.gameObject, 5f);
                }

                StartCoroutine(DestroyEffect());

            }
        }
    }

    IEnumerator DestroyEffect()
    {
        checkpointState.exists = false;
        GameObject _aux = SoundObject(ac);
        Destroy(_aux, 5f);

        GameObject implosion = Instantiate(implosionPrefab);
        implosion.transform.position = transform.position;
        Destroy(implosion, 2.5f);
        while (transform.localScale.x > 0.1f)
        {
            transform.localScale *= 0.95f;
            yield return new WaitForSeconds(0.01f);
        }
        if(pipesTop)
        {
            yield return new WaitForSeconds(1f);
            Camera.main.GetComponent<MainCamera>().SetFocusSpotCamera(new Vector3(2.64f, 3.63f, -293.4f), new Vector3(34.515f, -89.594f, 0), 60f);
            Camera.main.GetComponent<MainCamera>().SetCameraMode(CameraMode.FocusSpot);
            Camera.main.GetComponent<MainCamera>().SetCameraMode(CameraMode.Level01, 0.5f);
            pipesTop.GetComponent<Rigidbody>().isKinematic = false;
            pipesTop.GetComponent<Rigidbody>().AddExplosionForce(200, pipesTop.transform.position - Vector3.right, 5);
            pipesTop.GetComponent<Rigidbody>().AddTorque(transform.up * 100000, ForceMode.VelocityChange);
        }


        Destroy(gameObject);
    }

    GameObject SoundObject(AudioClip _ac)
    {
        GameObject _aux = new GameObject("Sound; " + _ac.ToString() + " - Parent; " + this.name.ToString());
        AudioSource _as = _aux.AddComponent<AudioSource>();
        _as.Stop();
        _as.clip = _ac;
        _as.volume = 0.25f;
        _as.pitch = 1.2f;
        _as.Play();
        return _aux;
    }

    public void SaveForCheckpoint()
    {
        if (this)
        {
            checkpointState.position = transform.position;
            checkpointState.rotation = transform.rotation;
            checkpointState.canRespawn = checkpointState.exists;
        }

    }

    public void LoadForCheckpoint()
    {
        if (!checkpointState.exists && checkpointState.canRespawn)
        {
            GameObject obj = Instantiate(Config.me.listOfPrefabs[(int)Prefabs.TelekinesisPowerUp]);
            obj.transform.position = checkpointState.position;
            obj.GetComponent<TelekinesisPowerUp>().checkpointState.position = checkpointState.position;
            obj.GetComponent<TelekinesisPowerUp>().checkpointState.rotation = checkpointState.rotation;
            obj.GetComponent<TelekinesisPowerUp>().checkpointState.canRespawn = checkpointState.canRespawn;
            GameManager.me.StartCoroutine(GameManager.me.RemoveFromCheckpointList(this, 0.5f));
        }
    }

    public void AddToCheckpointList()
    {
        GameManager.me.lisfOfCheckpointObjects.Add(this);
        checkpointState.exists = true;
    }

    public void RemoveFromCheckpointList()
    {
        GameManager.me.lisfOfCheckpointObjects.Remove(this);
    }
}
