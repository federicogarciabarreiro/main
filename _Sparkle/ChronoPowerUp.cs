using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChronoPowerUp : MonoBehaviour {

    public AudioClip ac;

    bool alreadyDestroyed;
    public GameObject implosionPrefab;

	void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Hero>())
        {
            if(!alreadyDestroyed)
            {
                alreadyDestroyed = true;
                //other.GetComponent<Hero>().ActivateChronoKinesis();
                StartCoroutine(DestroyEffect());
                var objs = Physics.OverlapSphere(transform.position, 11f, Config.me.breakableMask);
                foreach (var item in objs)
                    item.GetComponent<IBreakable>().SwitchBreakableState(false);
            }

        }
    }

    IEnumerator DestroyEffect()
    {
        GameObject _aux = SoundObject(ac);
        Destroy(_aux, 5f);

        GameObject implosion = Instantiate(implosionPrefab);
        implosion.transform.position = transform.position;
        Destroy(implosion, 2.5f);
        while(transform.localScale.x > 0.1f)
        {
            transform.localScale *= 0.95f;
            yield return new WaitForSeconds(0.01f);
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
}

