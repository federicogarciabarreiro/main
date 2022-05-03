using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fan : MonoBehaviour, IInteractable, ITimeSlowable {

    float maxDistance;
    public Transform wings;
    float rotationSpeed = 350f;
    float originalRotationSpeed;
    float forceMultiplier = 3f;
    float originalForceMultiplier;
    List<Material> originalMaterials = new List<Material>();
    Outline outline;
    public float dir;
    public bool incrementalForce;

    private void Start()
    {
        maxDistance = GetComponent<Collider>().bounds.size.z;
        originalRotationSpeed = rotationSpeed;
        originalForceMultiplier = forceMultiplier;
        outline = GetComponent<Outline>();
        AddToList();
    }

    private void Update()
    {
        wings.Rotate(0, 0, -rotationSpeed * Time.deltaTime);
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<Hero>())
        {
            if(!other.GetComponent<Hero>().GetShieldState())
            {
                if(incrementalForce)
                {
                    float multiplier = maxDistance - Vector3.Distance(transform.position, other.transform.position);
                    other.GetComponent<Hero>().ApplyForce(transform.forward * multiplier / forceMultiplier * dir);
                }
                else
                {
                    other.GetComponent<Hero>().ApplyForce(transform.forward * dir / forceMultiplier);
                }

            }
            else
            {
                other.GetComponent<Hero>().ApplyForce(Vector3.zero);
            }
        }
        if(other.GetComponentInParent<CubeScifiLiftable>())
        {
            other.GetComponentInParent<CubeScifiLiftable>().GetRigidbody().AddForce(transform.forward * dir * 400f * Time.deltaTime / forceMultiplier, ForceMode.Acceleration);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Hero>())
        {
            other.GetComponent<Hero>().ApplyForce(Vector3.zero);
        }
    }

    public void SlowDown(int xTimes)
    {
        rotationSpeed /= 5f;
        forceMultiplier *= 5f;
        foreach (Transform item in transform.Find("Turbine"))
        {
            originalMaterials.Add(item.GetComponent<Renderer>().material);
            item.GetComponent<Renderer>().material = Config.me.timeSlowMaterial;
        }
    }

    public void Reset()
    {
        rotationSpeed = originalRotationSpeed;
        forceMultiplier = originalForceMultiplier;
        int i = 0;
        foreach (Transform item in transform.Find("Turbine"))
        {
            item.GetComponent<Renderer>().material = originalMaterials[i];
            i++;
        }
        originalMaterials.Clear();

    }

    public Transform GetTransform()
    {
        return transform;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public bool CanInteract(Hero hero)
    {
        //RAY -------------------------------------------------------
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Transform target;
        if (Physics.Raycast(ray, out hit, 1000f, Config.me.objectsMask))
        {
            target = hit.transform;
        }
        else
        {
            target = null;
        }

        RaycastHit hit2;
        if (Physics.Raycast(transform.position, (hero.transform.position - transform.position).normalized, out hit2, 1000f, Config.me.objectsAndHeroAndWalls))
        {

        }

        return target != null && hero.GetCC().isGrounded && hit.transform.IsChildOf(transform) && hit2.transform == hero.transform && Vector3.Distance(hero.transform.position, transform.position) < 15f;

    }

    public void SwitchOutline(bool state, CursorTextureType type)
    {
        if (state) outline.OutlineWidth = 5f;
        else outline.OutlineWidth = 0f;
        CursorManager.me.SetCursorColor(state, type);
    }

    public void Interact(Skill interaction)
    {
        SwitchOutline(false, CursorTextureType.SECONDARY);
    }

    public void AddToList()
    {
        GameManager.me.im.AddInteractionObject(this, Skill.SlowTime);
    }

    public void RemoveFromList()
    {
        GameManager.me.im.RemoveInteractionObject(this, Skill.SlowTime);
    }
}
