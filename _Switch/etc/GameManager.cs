using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
   
    public static GameManager instance { get; private set; }

    public EcoPool<Block> _pool;
    Level _level;
    int Bnum = 0;

    Block BlockFactory(Transform parent)
    {
        Block _block = Instantiate(Config.instance.blockPrefab);
        _block.name += " + " + ++Bnum;
        _block.transform.parent = parent;
        _block.gameObject.SetActive(false);
        return _block;
    }

    void Awake ()
    {
        instance = this;
        InitPool();
        InitLevel();
    }

    void InitPool()
    {
        Transform pooledParent = new GameObject("[Pooled]").transform;
        _pool = new EcoPool<Block>(() => BlockFactory(pooledParent), pool => EcoPool<Block>.ExpandPercent(pool, 50f));
        pooledParent.parent = transform;
    }

    void InitLevel()
    {
        _level = new GameObject("Level").AddComponent<Level>();
        _level.transform.parent = transform;
    }
}

