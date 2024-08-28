using Scriptable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class Plane : MonoBehaviour
{
    //Test

    private void Start()
    {
        SpawnBrick();
    }

    public void SpawnBrick()
    {
        for (int i = 0; i < 3; i++)
        {
            Debug.Log("Spawn...");
            SimplePool.Spawn<Brick>(PoolType.Brick, transform.position + Vector3.forward * i, transform.rotation).OnInit(ColorType.Green);
        }
    }
}
