using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Level : MonoBehaviour {

    int _tilesCounter;
    List<Module> _map;
    LevelData _levelData;

    void Awake()
    {
        _tilesCounter = -5;
        _levelData = new LevelData();
        _map = new List<Module>();
        InvokeRepeating("InvokeLevelGenerator", 1, 0.05f);
    }

    private void Update()
    {
        foreach (var item in _map)
        {
            if ((int)Config.instance.playerTransform.position.z + 20 > item.index) item.Show();
            else item.Hide();
        }
    }

    void InvokeLevelGenerator()
    {
        StartCoroutine(LevelGenerator());
    }

    public IEnumerator LevelGenerator()
    {
        if (Config.instance.playerTransform.GetComponent<Rigidbody>().velocity != Vector3.zero)
        {
            _tilesCounter++;
            Module _sector = new Module(_tilesCounter, (133571 + _tilesCounter * 575441) & int.MaxValue, _levelData);
            _map.Add(_sector);
        }
        yield return new WaitForEndOfFrame();
    }
}