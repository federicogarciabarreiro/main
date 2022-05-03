using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Bullet : MonoBehaviour, ILiftable {


    AudioSource _as;
    bool canPlaySound = true;

    public Collider col { get; set; }
    Rigidbody rb;
    Material originalMaterial;
    public bool isLifted;
    public TrailRenderer trail;
    Hero hero;
    bool ILiftable.isLifted { get; set; }

    void Awake () {
        rb = GetComponent<Rigidbody>();
        SetContinuousRB();
        originalMaterial = GetComponent<Renderer>().material;
        col = GetComponent<Collider>();
        Destroy(gameObject, 10f);

 
        _as = GetComponent<AudioSource>();
    }

    private void Start()
    {
        hero = Config.me.hero.GetComponent<Hero>();
    }

    void Update () {
        if(!isLifted)
            rb.velocity = transform.forward * 25f;
	}

    public void Lift()
    {

        if (canPlaySound)
        {
            _as.pitch = 2f;
            _as.Play();
            canPlaySound = false;
        }

        isLifted = true;
        Hero caster = Config.me.hero.GetComponent<Hero>();
        Vector3 origin = transform.position;
        Vector3 destination = caster.transform.position + Vector3.up + caster.transform.forward * 2;
        rb.velocity = (destination - origin) * 3;
    }

    public void LiftEnd(Vector3 destination)
    {
  
        _as.pitch = 1.5f;
        _as.Play();

        Vector3 origin = transform.position;
        rb.velocity = (destination - origin) * 7.5f;
        gameObject.layer = 9;
    }

    public void UnselectedState()
    {
        GetComponent<Renderer>().material = originalMaterial;
    }

    public void TargetedState()
    {
        GetComponent<Renderer>().material = Config.me.hologramMaterial;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Enemy>())
        {
            other.GetComponent<Enemy>().Die();
            Destroy(gameObject);
        }

        if (other.GetComponent<Hero>())
        {
            if (Vector3.Dot(hero.transform.forward, transform.forward) < 0)
                hero.Die(Death.FrontHit);
            else
                hero.Die(Death.BackHit);
            Destroy(gameObject);
        }

        if (other.gameObject.layer == 12)
        {
            Destroy(gameObject);
        }
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public void SetContinuousRB()
    {
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
    }

    public Rigidbody GetRigidbody()
    {
        return rb;
    }
}
