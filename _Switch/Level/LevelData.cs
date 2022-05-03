using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelData
{
    public List<Vector3> position;
    public List<Vector3> obstaclePosition;
    public List<Quaternion> rotation;

    public LevelData()
    {
        position = new List<Vector3>();
        obstaclePosition = new List<Vector3>();
        rotation = new List<Quaternion>();

        for (int i = 0; i < 36; i++)
        {
            position.Add(randomCircle(Vector3.zero, 6, (360f / 36) * i));
            obstaclePosition.Add(randomCircle(Vector3.zero, 5, (360f / 36) * i));
            rotation.Add(Quaternion.FromToRotation(Vector3.right, Vector3.zero - new Vector3(position[i].x, position[i].y, 0)));
        }
    }

    Vector3 randomCircle(Vector3 center, float radius, float ang)
    {
        Vector3 pos;
        pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
        pos.y = center.y + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
        pos.z = center.z;
        return pos;
    }
}