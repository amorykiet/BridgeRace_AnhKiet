using Scriptable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Vector2 spawnByAxis;

    //Test
    private void Start()
    {
        SpawnBrick();
    }

    public void SpawnBrick()
    {
        for (int i = 0; i < spawnByAxis.x; i++)
        {
            for(int j = 0; j < spawnByAxis.y; j++)
            {
                SimplePool.Spawn<Brick>(PoolType.Brick, spawnPoint.position + new Vector3(i*4f, 0, j*2f), spawnPoint.rotation).OnInit(ColorType.Green);
            }
        }
    }
}
