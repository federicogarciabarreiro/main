using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Config : MonoBehaviour
{
    public static Config instance { get; private set; }

    public Block blockPrefab;
    public Transform playerTransform;

    void Awake()
    {
        instance = this;
    }
}