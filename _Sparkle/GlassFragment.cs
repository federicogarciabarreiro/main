using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassFragment : MonoBehaviour, IBreakable, IRewindable {

    Rigidbody rb;
    float alpha = 1f;
    Renderer rend;
    Color color;
    public bool rewindON;
    private Memento _memento = new Memento();
    public Vector3 startPos;
    public Quaternion startRot;

    public class State : IState
    {
        public Vector3 pos;
        public Quaternion rotation;
        public State(Vector3 pos, Quaternion rotation)
        {
            this.pos = pos;
            this.rotation = rotation;
        }
    }
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rend = GetComponent<Renderer>();
        startPos = transform.position;
        startRot = transform.rotation;
    }

    public void StartRewindMode()
    {
        StartCoroutine(SaveState());
    }

    public void StopRewindMode()
    {
        StopAllCoroutines();
        _memento.states.Clear();
    }

    public void Update()
    {
        Rewind();
    }
    public void Break()
    {
        rb.isKinematic = false;
        rb.AddExplosionForce(1000, Config.me.hero.transform.position, 50);
    }

    IEnumerator DecreaseAlphaOfColor()
    {
        while(alpha > 0)
        {
            alpha -= Time.deltaTime/4;
            color.a = alpha;
            rend.material.color = color;
            yield return new WaitForSeconds(0.1f);
        }
        color.a = 0;
        rend.material.color = color;

    }
    public void SwitchBreakableState(bool state)
    {
        
    }

    public IEnumerator SaveState()
    {
        //Entry
        rb.isKinematic = false;
        //Action
        while (!rewindON)
        {
            _memento.states.Add(new State(transform.position, transform.rotation));
            yield return new WaitForSeconds(1 / _memento.framesPerSecond);
        }
        //Exit
        StartCoroutine(LoadState());
    }

    public IEnumerator LoadState()
    {
        //Entry
        rb.isKinematic = true;
        //Action
        while (rewindON)
        {
            if (_memento.states.Count > 0)
                _memento.states.RemoveAt(_memento.states.Count - 1);
            yield return new WaitForSeconds(1 / _memento.framesPerSecond);
        }
        //Exit
        StartCoroutine(SaveState());
    
    }

    public void Rewind()
    {
        if (rewindON && _memento.states.Count > 0)
        {
            var pos = (_memento.states[_memento.states.Count - 1] as State).pos;
            var rot = (_memento.states[_memento.states.Count - 1] as State).rotation;
            transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime * _memento.framesPerSecond);
            transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * _memento.framesPerSecond);
        }
    }
}
