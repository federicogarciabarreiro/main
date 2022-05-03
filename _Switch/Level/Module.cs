using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Module
{
    [System.Serializable]
    public class Cell
    {
        public Vector3 position;
        public Quaternion rotation;
    }

    List<Cell> _list;
    List<Block> _blockList;
    public int index;

    public Module(int sectorIndex, int seed, LevelData levelData)
    {
        _list = new List<Cell>();
        index = sectorIndex;
        System.Random rnd = new System.Random(seed);

        for (int i = 0; i < 36; i++)
        {
            if (rnd.NextDouble() < 0.5)
            {
                float probability = 0.0001f * (sectorIndex + 18);
                if (probability > 0.025) probability = 0.025f;

                Cell _cell = new Cell();

                if (rnd.NextDouble() < probability) _cell.position = levelData.obstaclePosition[i] + Vector3.forward * sectorIndex;
                else _cell.position = levelData.position[i] + Vector3.forward * sectorIndex;

                _cell.rotation = levelData.rotation[i];
                _list.Add(_cell);
            }
        }
    }

    void Draw()
    {
        _blockList = new List<Block>();

        foreach (var item in _list)
        {
            Block _block = GameManager.instance._pool.Acquire();
            _block.transform.position = item.position;
            _block.transform.rotation = item.rotation;
            _block.OnAcquire();
            _blockList.Add(_block);
        }
    }

    public void Show()
    {
        if (_blockList != null)
        {
            foreach (var item in _blockList)
            {
                item.OnAcquire();
            }
        }
        else Draw();
    }

    public void Hide()
    {
        if (_blockList != null)
        {
            foreach (var item in _blockList)
            {
                item.OnRelease();
            }
        }
    }

}