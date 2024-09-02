using Scriptable;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Stage : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Vector2 spawnByAxis;
    [SerializeField] private int colorNumber = 1;
    [SerializeField] private float horizontalDistance;
    [SerializeField] private float verticalDistance;

    private int MaxBricksPerColor => (int) spawnByAxis.x * (int) spawnByAxis.y / colorNumber;
    private Dictionary<ColorType, int> colorAvailableDict = new();

    //Test
    private void Start()
    {
        SpawnBrick();
    }

    public void SpawnBrick()
    {
        //Not opt yet
        for (int i = 0; i < colorNumber; i++)
        {
            // Carefull when ColorType enum is changed
            colorAvailableDict.Add((ColorType) (i + 1), 0);
        }

        for (int i = 0; i < spawnByAxis.x; i++)
        {
            for(int j = 0; j < spawnByAxis.y; j++)
            {
                ColorType randomColor = colorAvailableDict.ElementAt(Random.Range(0, colorAvailableDict.Count)).Key;
                colorAvailableDict[randomColor]++;
                if(colorAvailableDict[randomColor] >= MaxBricksPerColor)
                {
                    colorAvailableDict.Remove(randomColor);
                }
                SimplePool.Spawn<Brick>(PoolType.Brick, spawnPoint.position + new Vector3(i*horizontalDistance, 0, j*verticalDistance), spawnPoint.rotation).OnInit(randomColor);

            }
        }
    }
}
